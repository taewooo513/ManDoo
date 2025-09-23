using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : BaseRoom
{
    public Spawn spawn;
    //데이터테이블에서 id 챙겨오기
    public void Start() //UI 뜨는거 보려고 임시로 쓴거
    {
        UIManager.Instance.OpenUI<InGameBattleStartUI>();
        spawn = GetComponent<Spawn>(); //이거 되나?
    }

    public override void EnterRoom()
    {
        //전투 알림 팝업 띄우기 -> 배틀매니저에서 한다고 함
        UIManager.Instance.OpenUI<InGameBattleStartUI>();
        var battleData = DataManager.Instance.Battle.GetBattleData(1001); //TODO : 1001 수정해야됨
        spawn.EnemySpawn(battleData.battleEnemies); //적 소환
    }

    public override void ExitRoom()
    {
    }
}
