using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<BaseEntity> _playableCharacter;
    public List<BaseEntity> PlayableCharacter => _playableCharacter;
    
    private List<BaseEntity> _enemyCharacter;

    private void Awake()
    {
        _playableCharacter = new List<BaseEntity>();
        _enemyCharacter = new List<BaseEntity>();
    }

    public void AddPlayer(BaseEntity baseEntity)
    {
        _playableCharacter.Add(baseEntity);
    }

    public void AddEnemy(BaseEntity baseEntity)
    {
        _enemyCharacter.Add(baseEntity);
    }
    public bool HasPlayerById(int id)
    {
        // id 중복 체크용
        return _playableCharacter.Exists(pc => pc.id == id);
    }
    public void RemovePlayer(int id)
    {
        _playableCharacter.RemoveAll(pc => pc.id == id);
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
        BattleManager.Instance.BattleStartTrigger(_playableCharacter, _enemyCharacter);
    }

    public void EndGame()
    {
    }
    
    public void PlayableCharacterPosition(List<BaseEntity> playerPositionList) //캐릭터 스폰(위치 지정)
    {
        _playableCharacter = playerPositionList;
    }
}