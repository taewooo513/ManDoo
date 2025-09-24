using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class ItemInfo
{
    public ItemType itemType;
    public int consumableSkillId;
    public string itemName;
    public string itemDescription;
    public int maxCount;
    public int price;
    public string iconPathString;
    
    
    public ItemInfo()
    {
        // _itemData = DataManager.Instance.Item.GetItemData(id);
    }
}
[System.Serializable]
public class Item // 데이터 테이블 생성 후 제대로 만들기.
{
    // public string itemName;
    public Sprite icon; // 아이템 아이콘 -> 인벤토리나 장착 슬롯 등에 표시하기 위해 설정
    // 추가 정보(타입, 효과 등) 필요시 더 추가
    
    private EnemyData _data;
    // public ItemInfo GetItemInfo { get; private set; }
    
    public void Init()
    {
        
    }
}
