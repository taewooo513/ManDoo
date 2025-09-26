using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : BaseRoom
{
    // void Start() //호출하는 부분에서 이런식으로 호출
    // {
    //     DataManager.Instance.Initialize();
    //     battleRoom = new BattleRoom();
    //     battleRoom.EnterRoom();
    // }

    public override void EnterRoom(int id) //방 입장 시 todo : 방 호출하는 부분에서 id 랜덤돌려서 넣어줘야 됨
    {
        base.EnterRoom(id); //플레이어 소환(위치 선정)
        var battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 데이터테이블에 접근
        spawn.EnemySpawn(battleData.battleEnemies); //적 소환
    }

    public override void ExitRoom(int id)
    {
        base.ExitRoom(id);
        int equipItemId;
        
        //ItemManager.Instance.AddItem(1001, randomGoldDropCount); //랜덤 개수대로 골드 아이템 추가
        if (randomPercentage < dropProb) //드랍 확률대로 아이템 떨어짐 (ex : 0.25% 확률로 아이템 떨어짐)
        {
             //ItemManager.Instance.AddItem(dropItem, 1); //승리 ui에 아이템 추가 todo : 예시 코드라서 다른곳의 add함수 필요함. 추가하고 ui쪽에서 지우는것도 필요함
        }

        for (int i = 0; i < equipItemIds.Count; i++) //'죽은 플레이어가 죽기 전 가지고있던 장비' 리스트 순회하면서
        {
            equipItemId = equipItemIds[i];
            //ItemManager.Instance.AddItem(equipItemId, 1); //승리 ui에 장비 추가하기
        }
        equipItemIds.Clear(); //리스트 초기화
    }
}