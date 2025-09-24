using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : BaseRoom
{
    public Spawn spawn;
    
    // void Start() //호출하는 부분에서 이런식으로 호출
    // {
    //     DataManager.Instance.Initialize();
    //     battleRoom = new BattleRoom();
    //     battleRoom.EnterRoom();
    // }

    public override void EnterRoom() //방 입장 시
    {
        GameObject spawnObject = new GameObject("Spawn");
        Spawn spawn = spawnObject.AddComponent<Spawn>(); //스폰 컴포넌트 챙겨오기
        var battleData = DataManager.Instance.Battle.GetBattleData(1001); //TODO : 1001 수정해야됨 //배틀데이터 dt에서 배틀룸 id 챙겨옴
        spawn.EnemySpawn(battleData.battleEnemies); //적 소환
    }

    public override void ExitRoom()
    {
    }
}
