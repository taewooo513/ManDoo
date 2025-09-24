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
        Debug.Log(spawn);
        var battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 dt에서 배틀룸 id 받아오기
        spawn.EnemySpawn(battleData.battleEnemies); //적 소환
    }

    public override void ExitRoom()
    {
        //전투 보상
    }
}
