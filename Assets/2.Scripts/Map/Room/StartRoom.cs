using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : BaseRoom
{
    public override void EnterRoom(int id)
    {
        base.EnterRoom(id); //스폰 챙겨오기
        spawn.PlayableCharacterCreate(1004); //시작 플레이어(1004) 생성
        spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
        //인벤토리.gold += 1500;

        //인벤토리.포션 랜덤 3개
        //for(int i=0; i<3; i++){
        //int potionId = Random.Range(0, 포션.count);
        //인벤토리 += potionId; //포션 추가
        //}
    }
}
