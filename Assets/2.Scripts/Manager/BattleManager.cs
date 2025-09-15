using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private BaseEntity[] playableCharacters;
    private BaseEntity[] enemyCharacters;
    private BaseEntity nowTurnEntity;
    private Queue<BaseEntity> turnEnetitys;

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

    // Enemy 에서 필요한 메서드들:
    /*
    
    public int GetTotalNumOfPlayerCharacters() // 적과 조우한 플레이어 캐릭터 수 반환
    {
    
    }

    */
}