using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    //public List<ItemData> itemData = new List<ItemData>();
    
    public bool CanUseItem(Item item)
    {
        if (item == null) return false;
        
        // TODO: 나중에 타입/효과 데이터 테이블에서 받아솨서 switch 문으로 작성
        
        return false;
    }
}
