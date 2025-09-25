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

        InventoryManager.Instance.TryAddItem(1001, 1500); //1500 골드 지급
        InventoryManager.Instance.TryAddItem(2100, 3); //회복약 지급
    }
}