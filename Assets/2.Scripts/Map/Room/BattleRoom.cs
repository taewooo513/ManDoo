using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : BaseRoom
{
    public Spawn spawn;

    public BattleRoom()
    {
        GameObject spawnObject = new GameObject("Spawn");
        spawn = spawnObject.AddComponent<Spawn>();
        Debug.Log(spawn);
    }

    public override void EnterRoom() //방 입장 시
    {
        var battleData = DataManager.Instance.Battle.GetBattleData(1001); //TODO : 1001 수정해야됨 //배틀데이터 dt에서 id 챙겨옴
        spawn.EnemySpawn(battleData.battleEnemies); //적 소환
    }

    public override void ExitRoom()
    {
    }
}
