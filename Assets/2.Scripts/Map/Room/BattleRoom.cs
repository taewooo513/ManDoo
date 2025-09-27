using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : BattleTreasureEvent
{
    private int _dropGoldCount;
    private int _dropItem; //드랍하는 아이템id (골드x)
    private float _battleDropProb; //아이템 드랍 확률 (ex : 0.25)
    private float _goldRandomRatio; //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
    private int _randomGoldDropCount; //실제로 떨어지는 금화 개수
    private float _randomPercentage; //0~100 사이 중 랜덤 퍼센트 (랜덤 숫자 뽑기)
    public override void EnterRoom() //방 입장 시
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
    }
    
    public override void Init(int id)
    {
        if (!isInteract) //처음 입장시에만
        {
            spawn.EnemySpawn(battleData.battleEnemies); //적 소환
            isInteract = true;
        }

        base.Init(id);
        _dropGoldCount = battleData.dropGold; //골드 드랍 개수
        _dropItem = battleData.dropId; //드랍하는 아이템id (골드x)
        _battleDropProb = battleData.dropProb; //아이템 드랍 확률 (ex : 0.25)
        _goldRandomRatio = Random.Range(0.9f, 1.1f); //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
        _randomGoldDropCount = (int)(_dropGoldCount * _goldRandomRatio); //실제로 떨어지는 금화 개수
        _randomPercentage = Random.Range(0f, 100f);
        
    }

    public override void ExitRoom()
    {
        base.ExitRoom();
        int equipItemId;
        
        ItemManager.Instance.AddReward(eItemType.Consumable,1001, _randomGoldDropCount); //랜덤 개수대로 보상 ui에 골드 아이템 추가
        if (_randomPercentage < _battleDropProb) //드랍 확률대로 아이템 떨어짐 (ex : 0.25% 확률로 아이템 떨어짐)
        {
            ItemManager.Instance.AddReward(eItemType.Consumable, _dropItem, 1); //보상 ui에 아이템 추가
        }

        for (int i = 0; i < equipItemIds.Count; i++) //'죽은 플레이어가 죽기 전 가지고있던 장비' 리스트 순회하면서
        {
            equipItemId = equipItemIds[i];
            ItemManager.Instance.AddReward(eItemType.Weapon, equipItemId, 1); //보상 ui에 장비 추가하기
        }
        equipItemIds.Clear(); //장비 리스트 초기화
        OnEventEnded();
    }

    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground";
    }
}