using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 아이템을 관리하는 매니저 클래스
/// </summary>
public enum eItemType
{
    Consumable,  // 소비 아이템
    Weapon,      // 무기 아이템  
    Accessory,   // 장신구 아이템
}

public class ItemManager : Singleton<ItemManager>
{
    private Dictionary<(eItemType, int id), int> rewards = new(); // key: type & id, value: count

    public Dictionary<(eItemType, int id), int> GetRewards() => new(rewards);
    //private Dictionary<(eItemType, int[] id), int[]> rewardArr = new(); // key: type & id, value: count

    /// <summary>
    /// ID에 해당하는 아이템을 생성하는 메서드
    /// </summary>
    /// <param name="id">생성할 아이템의 ID</param>
    /// <returns>생성된 아이템 객체</returns>

    // public eItemType GetItemKind(int id)
    // {
    //
    // }

    public Item CreateItem(int id) 
    {
        var item = new Item(id);
        item.icon = GetItemIcon(item);
        return item;
    }

    public Weapon CreateWeapon(int id)
    {
        var weapon = new Weapon(id);
        weapon.icon = GetWeaponIcon(weapon);
        return weapon;
    }

    /// <summary>
    /// 아이템의 아이콘 스프라이트를 가져오는 메서드
    /// </summary>
    public Sprite GetItemIcon(Item item)
    {
        if (item == null || item.ItemInfo == null) return null;
        return GetIconByPath(item.ItemInfo.iconPathString);
    }

    /// <summary>
    /// 무기의 아이콘 스프라이트를 가져오는 메서드
    /// </summary>
    public Sprite GetWeaponIcon(Weapon weapon)
    {
        if (weapon == null) return null;
        return GetIconByPath(weapon.GetIconPath());
    }

    // public Sprite GetAccessoryIcon(Accessory accessory)
    // {
    //     if (accessory == null) return null;
    //     return GetIconByPath(accessory.GetGameObjectPath());
    // }

    /// <summary>
    /// 경로에 해당하는 아이콘 스프라이트를 로드하는 메서드
    /// </summary>
    public Sprite GetIconByPath(string path)
    {
        Debug.Log("GetIconByPath: " + path + "");
        if (string.IsNullOrEmpty(path)) return null;

        var sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
            Debug.LogError($"Item icon is not found at path: {path}.");
        return sprite;
    }
    
    public void UseItem(eItemType type, int id)
    {   
        // TODO: 아이템 사용 로직
        
    }

    public Item GetItemInUse(eItemType type, int id)
    {
        // TODO: 사용중인 아이템 리턴
        return null;
    }
    
    /// <summary>
    /// 아이템을 해당 슬롯에 장착할 수 있는지 확인하는 메서드
    /// </summary>
    public bool CanEquipItem(Item item, EquipmentSlotType slotType)
    {
        if (item == null || item.ItemInfo == null) return false;
        // TODO: 아이템/무기/악세서리 타입 비교해서 처리하는 로직 추가.
        return true;
    }

    /// <summary>
    /// 보상 아이템을 추가하는 메서드
    /// </summary>
    public void AddReward(eItemType type, int id, int amount) // 보상 -> 승리 UI
    {
        if (amount == 0) return;
        var key = (type, id);
        if (rewards.ContainsKey(key))
            rewards[key] += amount;
        else
            rewards[key] = amount;
    }

    public void AddReward(eItemType type, List<int> ids, List<int> amounts)
    {
        if (ids.Count != amounts.Count) return;
        
        for (int i = 0; i < ids.Count; i++)
        {
            var key = (type, ids[i]);
            int amount = amounts[i];
            if (amount <= 0) continue;

            if (rewards.ContainsKey(key))
                rewards[key] += amount;
            else
                rewards[key] = amount;
        }
    }

    /// <summary>
    /// 보상 목록을 초기화하는 메서드
    /// </summary>
    public void ClearRewards() => rewards.Clear();
}