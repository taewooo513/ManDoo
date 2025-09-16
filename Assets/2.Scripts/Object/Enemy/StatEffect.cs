using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Normal, Mark, Buff, Debuff, Guard, Guardian, PlayerReactAtk, PlayerReactSupport
}

public class StatEffect
{
    //엔티티 인포 챙겨오기
    private List<StatusType> _playerCurrentStatus = new List<StatusType>();
    private List<StatusType> _enemyCurrentStatus =  new List<StatusType>();
    
    private List<BaseEntity> _playableCharacters; //배틀 매니저의 플레이어 주소 참조
    private List<BaseEntity> _enemyCharacters;

    private void Awake()
    {
        _playerCurrentStatus.Clear();
        _enemyCurrentStatus.Clear();
        _playableCharacters = BattleManager.Instance.PlayableCharacters;
        _enemyCharacters = BattleManager.Instance.EnemyCharacters;

        foreach (BaseEntity player in _playableCharacters) //플레이어 현재 상태리스트 담기
        {
            _playerCurrentStatus.AddRange(player.entityInfo.currentStatus);
        }
        
        foreach (BaseEntity entity in _enemyCharacters) //적 현재 상태리스트 담기
        {
            _playerCurrentStatus.AddRange(entity.entityInfo.currentStatus);
        }
    }
    
    private void AttackPercentage()
    {
        float total = 10;
        float rand = UnityEngine.Random.value * total;
        
        foreach (BaseEntity playableCharacters in _playableCharacters) //상태이상 효과에 따라 다른 가중치 부여
        {
            // if (playableCharacters.statEffect == StatusType.Mark) return;
            // if(playableCharacters.statEffect == StatusType.Mark){}
            // if(playableCharacters.StatusType == StatusType.Buff){}
            // if(playableCharacters.StatusType == StatusType.Debuff){}
            // if(playableCharacters.StatusType == StatusType.Guard){}
            // if(playableCharacters.StatusType == StatusType.Guardian){}
            // if(playableCharacters.StatusType == StatusType.PlayerReactAtk){}
            // if(playableCharacters.StatusType == StatusType.PlayerReactSupport){}
            
        }
    }

    public void AddStatus(StatusType status) //상태이상 추가
    {
        //if 시전자가 player라면
        _enemyCurrentStatus.Add(status);
        
        //if 시전자가 enemy라면
        _playerCurrentStatus.Add(status);
    }

    public void HasStatus(StatusType status) //현재 걸린 상태이상
    {
    }

    public void RemoveStatus(StatusType status) //상태이상 제거
    {
    }

    public void Clear()
    {
        for (int i = 0; i < _playerCurrentStatus.Count; i++)
        {
            _playerCurrentStatus.RemoveAt(i);
            _enemyCurrentStatus.RemoveAt(i);
        }
    }

    private void Mark()
    {
        //battlemanager.instance.
    }

    private void Buff() 
    {

    }

    private void Guard()
    {

    }

    private void PlayerReact() // 플레이어가 행동에 대한 가중치 계산
    {

    }

    private void SwapPosition()
    {

    }
}
