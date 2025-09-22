using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class ItemInfo
{
    // private ItemData _itemData;
    public string itemName;
    
    
    public ItemInfo()
    {
        // _itemData = DataManager.Instance.Item.GetItemData(id);
    }
}
[System.Serializable]
public class Item // 데이터 테이블 생성 후 제대로 만들기.
{
    // public string itemName;
    // public Sprite icon;
    // 추가 정보(타입, 효과 등) 필요시 더 추가
    
    private EnemyData _data;
    // public ItemInfo GetItemInfo { get; private set; }
    
    public void Init()
    {
        
    }
    
    public void UseItem()
    {
        // TODO: switch 로 itemdata 에서 받아온 아이템 타입에 따라 다르게 아이템 사용
    }
}
