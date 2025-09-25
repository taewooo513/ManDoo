using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Item CreateItem(int id)
    {
        var item = new Item(id);
        //item.Init(id);
        item.icon = GetIcon(item);
        return item;
    }

    public Sprite GetIcon(Item item)
    {
        if (item == null || item.ItemInfo == null) return null;
        return GetIconByPath(item.ItemInfo.iconPathString);
    }

    public Sprite GetIconByPath(string path)
    {
        Debug.Log("GetIconByPath: " + path + "");
        if (string.IsNullOrEmpty(path)) return null;

        var sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
            Debug.LogError($"Item icon is not found at path: {path}.");
        return sprite;
    }

    public bool CanEquipItem(Item item, EquipmentSlotType slotType)
    {
        if (item == null || item.ItemInfo == null) return false;
        // TODO: 아이템/무기/악세서리 타입 비교해서 처리하는 로직 추가.
        return true;
    }
}
