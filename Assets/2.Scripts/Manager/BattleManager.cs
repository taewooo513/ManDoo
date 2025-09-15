using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleManager : Singleton<BattleManager>
{
    private BaseEntity[] playableCharacters;
    private BaseEntity[] enemyCharacters;
    private BaseEntity nowTurnEntity;
    private Queue<BaseEntity> turnEnetitys;

    private PlayableCharacter nowPlayableCharacter;

    protected override void Awake()
    {
        base.Awake();
        playableCharacters = new BaseEntity[4];
        enemyCharacters = new BaseEntity[4];
    }

    public void Init()
    {

    }

    public void AttackEnemy(int damageValue, int index)
    {
        enemyCharacters[index].Damaged(damageValue);
    }

    public void AttackPlayer(int damageValue, int index)
    {
        playableCharacters[index].Damaged(damageValue);
    }

    public void SelectPlayer(int index)
    {
        nowPlayableCharacter = (PlayableCharacter)playableCharacters[index];
    }
}