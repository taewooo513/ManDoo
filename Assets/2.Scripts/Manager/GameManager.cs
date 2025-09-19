using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<BaseEntity> playableCharacter;

    private void Awake()
    {
        playableCharacter = new List<BaseEntity>();
    }

    public void SelectPlayer(BaseEntity baseEntity)
    {
        playableCharacter.Add(baseEntity);
    }

    public void StartGame()
    {
        //BattleManager.Instance.AddPlayableCharacter();
    }

    public void StartBattle()
    {
        List<BaseEntity> baseEntities = new List<BaseEntity>();
        //BattleManager.Instance.AddEnemyCharacter();
        BattleManager.Instance.BattleStartTrigger(playableCharacter, baseEntities);
    }

    public void EndGame()
    {

    }
}
