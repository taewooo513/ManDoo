using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseEntity> _playableCharacters;
    public List<BaseEntity> PlayableCharacters => _playableCharacters;


    private List<BaseEntity> _enemyCharacters;

    public List<BaseEntity> EnemyCharacters => _enemyCharacters;

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

    private void Start() //준비되면 주석 풀기.
    {
    }

    public void BattleStartTrigger(List<BaseEntity> playerList, List<BaseEntity> enemyList)
    {
        _playableCharacters = playerList;
        _enemyCharacters = enemyList;
        //전투 시작 UI 출력
        Turn();
    }

    public List<int> GetLowHpEntityIndexList(bool isPlayer)
    {
        List<int> indexList = new List<int>();
        if (isPlayer)
        {
            foreach (var item in _playableCharacters)
            {
                if (item.entityInfo.currentHp / (float)item.entityInfo.maxHp <= 0.4f)
                {
                    indexList.Add(_playableCharacters.IndexOf(item));
                }
            }
        }
        else
        {
            foreach (var item in _enemyCharacters)
            {
                if (item.entityInfo.currentHp / (float)item.entityInfo.maxHp <= 0.1f)
                {
                    indexList.Add(_enemyCharacters.IndexOf(item));
                }
            }
        }

        return indexList;
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
                    //nowTurnEntity.StartExtraTurn();
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

    private void Win()
    {
        Debug.Log("승리!");
        //승리 UI 출력
        EndBattle();
    }

    private void Lose()
    {
        Debug.Log("패배...");
        //패배 UI출력
        EndBattle();
    }

    private void EndBattle()
    {
        //TODO: 전투가 끝났을 때 공통적으로 해야하는 것...?
    }

    private void SetTurnQueue() //한번 섞은 후, 순서별 정렬, 플레이어 우선 정렬
    {
        int n = _playableCharacters.Count;
        int m = _enemyCharacters.Count;
        List<BaseEntity> tempPlayerList = _playableCharacters;
        List<BaseEntity> tempEnemyList = _enemyCharacters;
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

    //index와 대미지를 넣으면 공격합니다.
    public void AttackEntity(int index, int attackDamage)
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
    public void AttackEntity(List<int> indexList, int attackDamage)
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
                if (_playableCharacters[item] == entity)
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (var item in posList)
            {
                if (_enemyCharacters[item] == entity)
                {
                    return true;
                }
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
                if (_enemyCharacters.Count >= pos && _enemyCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        else
        {
            foreach (var pos in skillRange)
            {
                if (_playableCharacters.Count >= pos && _playableCharacters[pos] != null)
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
        if (nowTurnEntity is PlayableCharacter)
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
                    index = _enemyCharacters.IndexOf(character);
                    break;
                }
            }

            if (index == -1)
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
}