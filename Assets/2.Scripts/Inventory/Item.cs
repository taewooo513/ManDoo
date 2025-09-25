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
    private ConsumableData itemData;
    
    public ItemInfo(int id)
    {
        this.itemData = DataManager.Instance.Consumable.GetConsumableData(id);
        this.itemName = itemData.itemName;
        this.itemDescription = itemData.itemDescription;
        this.maxCount = itemData.maxCount;
        this.price = itemData.price;
        this.iconPathString = itemData.iconPathString;
    }
}
//[System.Serializable]
public class Item // Consumable Items
{
    public ItemInfo ItemInfo { get; private set; }
    public int ItemId { get; private set; }
    public Sprite icon; // 아이템 아이콘 -> 인벤토리나 장착 슬롯 등에 표시하기 위해 설정
    private EnemyData _data; // 이거 사용 하려나요?

    public Item(int id)
    {
        ItemId = id;
        Init(id);
    }
    public void Init(int id) => ItemInfo = new ItemInfo(id);
}
