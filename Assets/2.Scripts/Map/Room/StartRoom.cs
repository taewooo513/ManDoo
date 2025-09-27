using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : BaseRoom
{
    public override void Init(int id)
    {
        base.Init(id);
    }

    public override void EnterRoom()
    {
        base.EnterRoom(); //스폰 챙겨오기
        if (!isInteract) //첫 1회만 생성
        {
            spawn.PlayableCharacterCreate(1004); //시작 플레이어(1004) 생성
            isInteract = true;
        }
        spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)

        InventoryManager.Instance.TryAddItem(eItemType.Consumable,1001, 1500); //1500 골드 지급
        InventoryManager.Instance.TryAddItem(eItemType.Consumable, 2001, 3); //회복약 지급
        OnEventEnded();
    }
    public override void OnEventEnded()
    {
        base.OnEventEnded();
        InventoryManager.Instance.TryAddItem(eItemType.Consumable, 2001, 3); //회복약 지급
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground";
    }
}