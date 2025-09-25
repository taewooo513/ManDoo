using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : BaseRoom
{
    public override void EnterRoom(int id)
    {
        base.EnterRoom(id); //플레이어 소환(위치 선정)
    }
    
    public override void ExitRoom(int id)
    {
        base.ExitRoom(id);
        
        //ItemManager.Instance.AddItem(1001, randomGoldDropCount); //랜덤 개수대로 골드 아이템 추가
        if (randomPercentage < dropProb) //드랍 확률대로 아이템 떨어짐 (ex : 0.25% 확률로 아이템 떨어짐)
        {
            //ItemManager.Instance.AddItem(dropItem, 1); //아이템 추가 todo : 예시 코드라서 다른곳의 add함수 필요함. 추가하고 ui쪽에서 지우는것도 필요함
        }
    }
}
