using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<BaseEntity> playableCharacter;
    private List<BaseEntity> enemyCharacter;

    private void Awake()
    {
        playableCharacter = new List<BaseEntity>();
        enemyCharacter = new List<BaseEntity>();
    }

    public void AddPlayer(BaseEntity baseEntity)
    {
        playableCharacter.Add(baseEntity);
    }

    public void AddEnemy(BaseEntity baseEntity)
    {
        enemyCharacter.Add(baseEntity);
    }
    public bool HasPlayerById(int id)
    {
        // id 중복 체크용
        return playableCharacter.Exists(pc => pc.id == id);
    }
    public void RemovePlayer(int id)
    {
        playableCharacter.RemoveAll(pc => pc.id == id);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartBattle();
        }
    }

    public void StartGame()
    {
        //BattleManager.Instance.AddPlayableCharacter();
    }

    public void StartBattle()
    {
        BattleManager.Instance.BattleStartTrigger(playableCharacter, enemyCharacter);
    }

    public void EndGame()
    {
    }
}