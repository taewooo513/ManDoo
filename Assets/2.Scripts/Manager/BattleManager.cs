using System;
using System.Collections;
using System.Collections.Generic;
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
                if (enemyCharacters.Count >= pos && enemyCharacters[pos - 1] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        else
        {
            foreach (var pos in skillRange)
            {
                if (playableCharacters.Count >= pos && playableCharacters[pos - 1] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        return possibleSkillRange;
    }

    public void SwitchPlayerPosition(PlayableCharacter playableCharacterA, PlayableCharacter playableCharacterB)
    {
        int indexA = 0;
        int indexB = 0;

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
        playableCharacters[indexA] = playableCharacterB;
        playableCharacters[indexB] = playableCharacterA;

        var swapPos = playableCharacterB.transform.position;
        playableCharacterB.transform.position = playableCharacterA.transform.position;
        playableCharacterA.transform.position = swapPos;
    }
}