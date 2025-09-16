using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseEntity> playableCharacters;
    private List<BaseEntity> enemyCharacters;

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
    public int GetTotalNumOfPlayerCharacters() // 적과 조우한 플레이어 캐릭터 수 반환
    {
        return playableCharacters.Count;
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

    // public void SwitchEnemyPosition() {} 
}