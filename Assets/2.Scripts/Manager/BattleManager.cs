using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class BattleManager : Singleton<BattleManager>
{
    public List<BaseEntity> _playableCharacters;
    public List<BaseEntity> PlayableCharacters => _playableCharacters;

    public List<BaseEntity> _enemyCharacters;
    public List<BaseEntity> EnemyCharacters => _enemyCharacters;
    
    public BaseRoom baseRoom; //TODO : 이거 연결해야됨
    public Weapon weapon; //TODO : 이거 연결해야됨

    private BaseEntity nowTurnEntity;
    private PlayableCharacter nowSeletePlayableCharacter;

    private Skill nowSkill;
    private System.Random _random = new System.Random();
    private Queue<BaseEntity> _turnQueue;

    protected override void Awake()
    {
        base.Awake();
        _playableCharacters = new List<BaseEntity>();
        _enemyCharacters = new List<BaseEntity>();
        _turnQueue = new Queue<BaseEntity>();
    }

    private float GetAverageSpeed()
    {
        float sum = 0;
        foreach (var item in _playableCharacters)
        {
            sum += item.entityInfo.speed;
        }

        foreach (var item in _enemyCharacters)
        {
            sum += item.entityInfo.speed;
        }

        return sum / (_playableCharacters.Count + _enemyCharacters.Count);
    }

    public void BattleStartTrigger(List<BaseEntity> playerList, List<BaseEntity> enemyList)
    {
        foreach (var item in playerList)
        {
            _playableCharacters.Add(item);
            item.BattleStarted();
        }

        foreach (var item in enemyList)
        {
            _enemyCharacters.Add(item);
            item.BattleStarted();
        }
        UIManager.Instance.OpenUI<InGameBattleStartUI>(); //전투 시작 UI 출력
        Turn();
    }

    public void GetLowHpSkillWeight(out float playerSkillWeight, out float enemySkillWeight) //스킬 가중치
    {
        playerSkillWeight = 0.0f;
        enemySkillWeight = 0.0f;
        foreach (var item in _playableCharacters)
        {
            if (item.entityInfo.LowHPStatPlayer()) //플레이어블 캐릭터 체력이 40% 일때
            {
                playerSkillWeight += 0.3f; //플레이어블 캐릭터의 스킬 가중치 추가(공격/스킬 우선 사용)
            }
        }

        foreach (var item in _enemyCharacters)
        {
            if (item.entityInfo.LowHPStatEnemy()) //enemy 체력이 10% 이하일 때
            {
                enemySkillWeight += 0.3f; //enemy 캐릭터의 스킬 가중치 추가(보호/힐 우선 사용)
            }
        }
    }
    private void Turn() //한 턴
    {
        if (_turnQueue.Count == 0)
        {
            SetTurnQueue();
        }

        nowTurnEntity = _turnQueue.Peek();
        nowTurnEntity.StartTurn();
    }

    public void EndTurn(bool hasExtraTurn = true)
    {
        if (_playableCharacters.Count == 0)
        {
            Lose();
        }

        else if (_enemyCharacters.Count == 0)
        {
            Win();
        }
        else
        {
            if (hasExtraTurn)
            {
                if ((nowTurnEntity.entityInfo.speed - GetAverageSpeed()) / 10 >= UnityEngine.Random.value)
                {
                    nowTurnEntity.StartExtraTurn();
                    if (_playableCharacters.Count == 0)
                    {
                        Lose();
                    }

                    else if (_enemyCharacters.Count == 0)
                    {
                        Win();
                    }
                }
            }

            _turnQueue.Dequeue();
            Turn();
        }
    }
    //private void BattleRun()
    //{
    //    Debug.Log("전투회피!");
    //    //전투회피
    //    UIManager.Instance.OpenUI<InGameBattleRunButton>();
    //    EndBattle();
    //}

    private void Win()
    {
        Debug.Log("승리!");
        //승리 UI 출력
        weapon.AddWeaponExp(20); //숙련도 지급
        UIManager.Instance.OpenUI<InGameVictoryUI>();
        EndBattle();
    }

    private void Lose()
    {
        Debug.Log("패배...");
        //패배 UI출력
        weapon.AddWeaponExp(5);
        UIManager.Instance.OpenUI<InGameLoseUI>();
        EndBattle();
    }

    private void EndBattle()
    {
        //TODO: 전투가 끝났을 때 공통적으로 해야하는 것...?
        baseRoom.spawn.PlayableCharacterPosition(_playableCharacters); //현재 플레이어 위치 넘기기
        foreach (var item in _playableCharacters)
        {
            item.Release();
        }

        foreach (var item in _enemyCharacters)
        {
            item.Release();
        }

        _playableCharacters.Clear();
        _enemyCharacters.Clear();
    }

    private void SetTurnQueue() //한번 섞은 후, 순서별 정렬, 플레이어 우선 정렬
    {
        int n = _playableCharacters.Count;
        int m = _enemyCharacters.Count;
        List<BaseEntity> tempPlayerList = new();
        List<BaseEntity> tempEnemyList = new();

        foreach (var item in _playableCharacters)
        {
            tempPlayerList.Add(item);
        }

        foreach (var item in _enemyCharacters)
        {
            tempEnemyList.Add(item);
        }
        while (n > 1)
        {
            n--;

            int k = _random.Next(n + 1);
            (tempPlayerList[k], tempPlayerList[n]) = (tempPlayerList[n], tempPlayerList[k]);
        }

        while (m > 1)
        {
            m--;

            int k = _random.Next(m + 1);
            (tempEnemyList[k], tempEnemyList[m]) = (tempEnemyList[m], tempEnemyList[k]);
        }

        tempPlayerList.Sort((a, b) => b.entityInfo.speed.CompareTo(a.entityInfo.speed));
        tempEnemyList.Sort((a, b) => b.entityInfo.speed.CompareTo(a.entityInfo.speed));
        while (tempPlayerList.Count != 0 || tempEnemyList.Count != 0)
        {
            if (tempPlayerList.Count == 0)
            {
                foreach (var item in tempEnemyList)
                {
                    _turnQueue.Enqueue(item);
                }

                tempEnemyList.Clear();
                break;
            }

            if (tempEnemyList.Count == 0)
            {
                foreach (var item in tempPlayerList)
                {
                    _turnQueue.Enqueue(item);
                }

                tempPlayerList.Clear();
                break;
            }

            if (tempPlayerList[0].entityInfo.speed >= tempEnemyList[0].entityInfo.speed)
            {
                _turnQueue.Enqueue(tempPlayerList[0]);
                tempPlayerList.RemoveAt(0);
            }
            else
            {
                _turnQueue.Enqueue(tempEnemyList[0]);
                tempEnemyList.RemoveAt(0);
            }
        }
    }

    //특정 Entity를 보내면 Position을 return합니다.
    public int? FindEntityPosition(BaseEntity baseEntity)
    {
        if (baseEntity is PlayableCharacter)
        {
            return _playableCharacters.IndexOf(baseEntity);
        }

        return _enemyCharacters.IndexOf(baseEntity);
    }

    public void AttackEntity(BaseEntity baseEntity)
    {
        baseEntity.Damaged(nowTurnEntity.entityInfo.attackDamage);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _enemyCharacters.Count; i++)
        {
            _enemyCharacters[i].Release();
        }
        for (int i = 0; i < _playableCharacters.Count; i++)
        {
            _playableCharacters[i].Release();
        }
    }


    //public List<BaseEntity> SelectEntityRange(List<int> targetPos, BaseEntity tagetEntity, List<BaseEntity> tagetList)
    //{
    //    List<BaseEntity> result = new List<BaseEntity>();
    //    int index = Utillity.GetIndexInListToObject<BaseEntity>(tagetList, tagetEntity);
    //    if (tagetList.Count < index)
    //    {
    //        return result;
    //    }

    //    for (int i = 0; i < targetPos.Count; i++)
    //    {
    //        if (targetPos[i] < index)
    //        {
    //            continue;
    //        }
    //    }
    //}

    //index와 대미지를 넣으면 공격합니다.
    public void AttackEntity(int index, float attackDamage)
    {
        if (nowTurnEntity is PlayableCharacter)
        {
            _enemyCharacters[index].Damaged(attackDamage);
        }
        else
        {
            _playableCharacters[index].Damaged(attackDamage);
        }
    }

    //범위 공격에 적합한 타입입니다.
    public void AttackEntity(List<int> indexList, float attackDamage)
    {
        if (nowTurnEntity is PlayableCharacter)
        {
            foreach (var index in indexList)
            {
                _enemyCharacters[index].Damaged(attackDamage);
            }
        }
        else
        {
            foreach (var index in indexList)
            {
                _enemyCharacters[index].Damaged(attackDamage);
            }
        }
    }

    //플레이어블 캐릭터 리스트에 캐릭터를 추가합니다.
    public void AddPlayableCharacter(PlayableCharacter playableCharacter)
    {
        _playableCharacters.Add(playableCharacter);
    }

    //적 캐릭터 리스트에 캐릭터를 추가합니다.
    public void AddEnemyCharacter(Enemy enemy)
    {
        _enemyCharacters.Add(enemy);
    }

    //안쓰지 않을까요?
    public void AttackEnemy(int damageValue, int index)
    {
        _enemyCharacters[index].Damaged(damageValue);
    }

    //안쓰지 않을까요?
    public void AttackPlayer(int damageValue, int index)
    {
        _playableCharacters[index].Damaged(damageValue);
    }

    public int GetTotalNumOfPlayerCharacters() // 적과 조우한 플레이어 캐릭터 수 반환
    {
        return _playableCharacters.Count;
    }

    //자기 자신->this, enablePos List 보내주세요 -> 쓸 수 있는지 안되는지 알려줍니다.
    public bool IsEnablePos(BaseEntity entity, List<int> posList)
    {
        if (nowTurnEntity is PlayableCharacter)
        {
            foreach (var item in posList)
            {
                if (_playableCharacters.Count > item && _playableCharacters[item] == entity)
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (var item in posList)
            {
                if (_enemyCharacters.Count > item && _enemyCharacters[item] == entity)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsTargetInList(List<int> targetPos)
    {
        if (nowTurnEntity is PlayableCharacter)
        {
            foreach (var item in targetPos)
            {
                if (_enemyCharacters.Count > item)
                {
                    return true;
                }
            }

            return false;
        }
        foreach (var item in targetPos)
        {
            if (_playableCharacters.Count > item)
            {
                return true;
            }
        }
        return false;
    }

    //플레이어 위치 받아오는 함수
    public List<(int, int)> GetPlayerPosition()
    {
        return new List<(int, int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }

    //적 위치 받아오는 함수
    public List<(int, int)> GetEnemyPosition()
    {
        return new List<(int, int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }


    //날 것의 스킬 범위를 던져주면 "때릴 수 있는" 적의 위치 리스트를 반환합니다. 범위 공격에 적합합니다.
    public List<int> GetPossibleSkillRange(List<int> skillRange)
    {
        List<int> possibleSkillRange = new List<int>();
        if (nowTurnEntity is PlayableCharacter)
        {
            foreach (var pos in skillRange)
            {
                if (_enemyCharacters.Count > pos && _enemyCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        else
        {
            foreach (var pos in skillRange)
            {
                if (_playableCharacters.Count > pos && _playableCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }

        return possibleSkillRange;
    }

    public void SwitchPlayerPosition(PlayableCharacter playableCharacterA, PlayableCharacter playableCharacterB)
    {
        int indexA = -1;
        int indexB = -1;

        for (int i = 0; i < _playableCharacters.Count; i++)
        {
            if (_playableCharacters[i] == playableCharacterA)
            {
                indexA = i;
            }
            else if (_playableCharacters[i] == playableCharacterB)
            {
                indexB = i;
            }
        }
        
        if (indexA == -1 || indexB == -1) return;
        _playableCharacters[indexA] = playableCharacterB;
        _playableCharacters[indexB] = playableCharacterA;

        SwapEntityTransform(playableCharacterA, playableCharacterB);
    }

    //게임 오브젝트의 위치를 바꿔줍니다.
    private void SwapEntityTransform(BaseEntity entityA, BaseEntity entityB)
    {
        (entityB.transform.position, entityA.transform.position) =
            (entityA.transform.position, entityB.transform.position);
    }

    //entity의 위치를 desiredPosition index와 변경합니다.
    public void SwitchPosition(BaseEntity entity, int desiredPosition)
    {
        if (entity is PlayableCharacter)
        {
            var index = -1;
            foreach (var character in _playableCharacters)
            {
                if (character == entity)
                {
                    index = _playableCharacters.IndexOf(character);
                    break;
                }
            }

            if (index == -1)
            {
                return;
            }

            // 구조 분해를 이용한 스왑 예시: 고맙다! 라이더야!
            // (a, b) = (b, a) 형태로 한 줄로 스왑 가능
            (_playableCharacters[index], _playableCharacters[desiredPosition]) =
                (_playableCharacters[desiredPosition], _playableCharacters[index]);
            SwapEntityTransform(_playableCharacters[index], _playableCharacters[desiredPosition]);
        }
        else
        {
            var index = -1;
            foreach (var character in _enemyCharacters)
            {
                if (character == entity)
                {
                    index = _enemyCharacters.IndexOf(entity);
                    break;
                }
            }
            
            if (index == -1 || index == desiredPosition || desiredPosition >= _enemyCharacters.Count)
            {
                return;
            }
            
            if (index + 1 == desiredPosition || index - 1 == desiredPosition)
            {
                (_enemyCharacters[index], _enemyCharacters[desiredPosition]) =
                    (_enemyCharacters[desiredPosition], _enemyCharacters[index]);
                SwapEntityTransform(_enemyCharacters[index], _enemyCharacters[desiredPosition]);
            }
            else
            {
                //index가 현재 바꾸고 싶어하는 entity의 위치
                if (index > desiredPosition)
                {
                    (_enemyCharacters[index - 1], _enemyCharacters[index]) = (
                        _enemyCharacters[index], _enemyCharacters[index - 1]);
                    SwapEntityTransform(_enemyCharacters[index - 1], _enemyCharacters[index]);
                }
                else
                {
                    (_enemyCharacters[index + 1], _enemyCharacters[index]) = (
                        _enemyCharacters[index], _enemyCharacters[index + 1]);
                    SwapEntityTransform(_enemyCharacters[index + 1], _enemyCharacters[index]);
                }
            }
        }
    }

    public List<float> GetWeightList(bool isPlayer) //타겟 가중치 리스트
    {
        if (isPlayer)
        {
            foreach (var item in _playableCharacters)
            {
                item.entityInfo.GetPlayableTargetWeight(); //playable 가중치 가져오기
            }

            return GenerateWeightListUtility.GetWeights();
        }

        foreach (var item in _enemyCharacters)
        {
            item.entityInfo.GetEnemyTargetWeight(); //enemy 가중치 가져오기
        }
        return GenerateWeightListUtility.GetWeights();
    }

    public void EntityDead(BaseEntity entity)
    {
        var index = FindEntityPosition(entity);
        if (index == null) return;
        if (entity is PlayableCharacter)
        {
            for (int i = (int)index; i < _playableCharacters.Count - 1; i++)
            {
                SwitchPosition(entity, i + 1);
            }
            _playableCharacters.RemoveAt(_playableCharacters.Count - 1);
            entity.Release();
            Destroy(entity.gameObject);
            return;
        }

        for (int i = (int)index; i < _enemyCharacters.Count - 1; i++)
        {
            SwitchPosition(entity, i + 1);
        }
        RemoveDeadEntityFromTurnQueue(entity);
        //TODO: 이후 적 사망시 보상 연결은 여기서? 아니면 Enemy에서?
        entity.Release();
        _enemyCharacters.RemoveAt(_enemyCharacters.Count - 1);
        Destroy(entity.gameObject);
    }

    private void RemoveDeadEntityFromTurnQueue(BaseEntity entity)
    {
        int loopTime = _turnQueue.Count;
        for (int i = 0; i < loopTime; i++)
        {
            var item = _turnQueue.Dequeue();
            if (item == entity)
            {
                continue;
            }
            _turnQueue.Enqueue(item);
        }
    }
    
}