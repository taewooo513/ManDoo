using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageRoom : BaseRoom //마을, 보스방
{
    public override void EnterRoom(int id)
    {
        base.EnterRoom(id); //플레이어 소환(위치 선정)
        //보스 생성
    }

    public override void ExitRoom(int id)
    {
        base.ExitRoom(id);
        //탐색한 방의 수
        //파티 초상화
        //플레이 보상
    }
}
