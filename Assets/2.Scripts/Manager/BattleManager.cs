using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseEntity> playableCharacters;
    public List<BaseEntity> PlayableCharacters => playableCharacters;
    
    
    private List<BaseEntity> enemyCharacters;

    public List<BaseEntity> EnemyCharacters => enemyCharacters;

    private BaseEntity nowTurnEntity;
    private PlayableCharacter nowSeletePlayableCharacter;

    private Skill nowSkill;

    protected override void Awake()
    {
        base.Awake();
        playableCharacters = new List<BaseEntity>();
        enemyCharacters = new List<BaseEntity>();
    }

    public int? FindEntityPosition(BaseEntity baseEntity)
    {
        if (baseEntity is PlayableCharacter)
        {
            return playableCharacters.IndexOf(baseEntity);
        }
        return enemyCharacters.IndexOf(baseEntity);
    }
    public void AttackEntity(BaseEntity baseEntity)
    {
        baseEntity.Damaged(nowTurnEntity.entityInfo.attackDamage);
    }
    public void AddPlayableCharacter(PlayableCharacter playableCharacter)
    {
        playableCharacters.Add(playableCharacter);
    }

    public void AddEnemyCharacter(Enemy enemy)
    {
        enemyCharacters.Add(enemy);
    }
    public void AttackEnemy(int damageValue, int index)
    {
        enemyCharacters[index].Damaged(damageValue);
    }

    public void AttackPlayer(int damageValue, int index)
    {
        playableCharacters[index].Damaged(damageValue);
    }

    public int GetTotalNumOfPlayerCharacters() // 적과 조우한 플레이어 캐릭터 수 반환
    {
        return playableCharacters.Count;
    }
    
    //플레이어 위치 받아오는 함수
    public List<(int,int)> GetPlayerPosition()
    {
        return new List<(int,int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }

    //적 위치 받아오는 함수
    public List<(int,int)> GetEnemyPosition()
    {
        return new List<(int,int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }

    public List<int> GetPossibleSkillRange(List<int> skillRange)
    {
        List<int> possibleSkillRange = new List<int>();
        if (nowTurnEntity is PlayableCharacter)
        {
            foreach (var pos in skillRange)
            {
                if (enemyCharacters.Count >= pos && enemyCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        else
        {
            foreach (var pos in skillRange)
            {
                if (playableCharacters.Count >= pos && playableCharacters[pos] != null)
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

        for (int i = 0; i < playableCharacters.Count; i++)
        {
            if (playableCharacters[i] == playableCharacterA)
            {
                indexA = i;
            }
            else if (playableCharacters[i] == playableCharacterB)
            {
                indexB = i;
            }
        }
        if(indexA == -1 || indexB == -1) return;
        playableCharacters[indexA] = playableCharacterB;
        playableCharacters[indexB] = playableCharacterA;

        SwapEntityTransform(playableCharacterA, playableCharacterB);
    }

    private void SwapEntityTransform(BaseEntity entityA, BaseEntity entityB)
    {
        (entityB.transform.position, entityA.transform.position) = (entityA.transform.position, entityB.transform.position);
    }
    //SwitchPosition(entity, desiredPosition-1);
    public void SwitchPosition(BaseEntity entity, int desiredPosition)
    {
        if (nowTurnEntity is PlayableCharacter)
        {
            var index = -1;
            foreach (var character in playableCharacters)
            {
                if (character == entity)
                {
                    index = playableCharacters.IndexOf(character);
                    break;
                }
            }

            if (index == -1)
            {
                return;
            }
            // 구조 분해를 이용한 스왑 예시: 고맙다! 라이더야!
            // (a, b) = (b, a) 형태로 한 줄로 스왑 가능
            (playableCharacters[index], playableCharacters[desiredPosition]) = (playableCharacters[desiredPosition], playableCharacters[index]);
            SwapEntityTransform(playableCharacters[index], playableCharacters[desiredPosition]);
        }
        else
        {
            var index = -1;
            foreach (var character in enemyCharacters)
            {
                if (character == entity)
                {
                    index = enemyCharacters.IndexOf(character);
                    break;
                }
            }

            if (index == -1)
            {
                return;
            }

            if (index + 1 == desiredPosition || index - 1 == desiredPosition)
            {
                (enemyCharacters[index], enemyCharacters[desiredPosition]) = (enemyCharacters[desiredPosition], enemyCharacters[index]);
                SwapEntityTransform(enemyCharacters[index], enemyCharacters[desiredPosition]);;
            }
            else
            {
                if (index > desiredPosition)
                {
                    (enemyCharacters[desiredPosition - 1], enemyCharacters[desiredPosition]) = (enemyCharacters[desiredPosition], enemyCharacters[desiredPosition - 1]);
                    SwapEntityTransform(enemyCharacters[desiredPosition-1], enemyCharacters[desiredPosition]);
                }
                else
                {
                    (enemyCharacters[desiredPosition], enemyCharacters[desiredPosition]) = (enemyCharacters[desiredPosition], enemyCharacters[desiredPosition]);
                    SwapEntityTransform(enemyCharacters[desiredPosition], enemyCharacters[desiredPosition]);
                }
            }
        }
    }
}