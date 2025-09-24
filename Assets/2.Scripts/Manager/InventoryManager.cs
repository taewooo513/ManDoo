using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum EquipmentSlotType
{
    Weapon = 0,
    Accessory1 = 1,
    Accessory2 = 2,
}

public class InventoryManager : Singleton<InventoryManager>
{
    private readonly Dictionary<int, int> itemCounts = new(); // key: item id; value: item amount;
    private readonly int[] slotItemIds = new int[10];
    private readonly Item[] equippedItems = new Item[3];
    public event Action<int, Item> OnSlotChanged; // slot index, item
    public event Action<EquipmentSlotType, Item> OnEquipChanged; // slot type, item
    
    public void AddItem(int id, int amount)
    {
        if (amount <= 0) return;

        if (itemCounts.ContainsKey(id))
            itemCounts[id] += amount;
        else
            itemCounts[id] = amount;

        if (!IsShownInSlots(id))
            AssignToEmptySlot(id);
    }

    public bool RemoveItem(int id, int amount = 1)
    {
        if (!itemCounts.TryGetValue(id, out int count)) return false;
        if (count < amount) return false;
        count -= amount;
        if (count <= 0)
        {
            itemCounts.Remove(id);
            RemoveFromSlots(id);
        }
            
        else
            itemCounts[id] = count;
        return true;
    }
    public int GetItemCount(int id) => itemCounts.TryGetValue(id, out int count) ? count : 0;

    public int GetSlotCount(int id) => slotItemIds.Length;

    public Item GetItemForSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return null;
        var item = slotItemIds[slotIndex];
        if (item == -1) return null;
        if (GetItemCount(item) <= 0) return null;
        return ItemManager.Instance.CreateItem(item);
    }

    public void SwapSlotItems(int from, int to)
    {
        if (!IsValidSlot(from) || !IsValidSlot(to) || from == to) return;
        (slotItemIds[to], slotItemIds[from]) = (slotItemIds[from], slotItemIds[to]);
        UpdateSlot(from);
        UpdateSlot(to);
    }

    public void SetSlotItemId(int slotIndex, int id)
    {
        if (!IsValidSlot(slotIndex)) return;
        slotItemIds[slotIndex] = id;
        UpdateSlot(slotIndex);
    }

    public Item GetEquipped(EquipmentSlotType slotType) => equippedItems[(int)slotType];

    public bool TryEquipFromInventory(int from, EquipmentSlotType to)
    {
        var item = GetItemForSlot(from);
        if (item == null) return false;
        if (!ItemManager.Instance.CanEquipItem(item, to)) return false;

        if (!RemoveItem(item.ItemId, 1)) return false;
        slotItemIds[from] = 0;
        UpdateSlot(from);

        var prevSlot = equippedItems[(int)to];
        if (prevSlot != null)
            AddItem(prevSlot.ItemId, 1);

        equippedItems[(int)to] = item;
        OnEquipChanged?.Invoke(to, item);
        return true;
    }

    public bool TryUnequipToInventory()
    {
        return true;
    }
    
    private bool IsValidSlot(int slotIndex) => slotIndex >= 0 && slotIndex < slotItemIds.Length;

    private bool IsShownInSlots(int id)
    {
        return true;
    }

    private void AssignToEmptySlot(int id)
    {
        
    }

    private void RemoveFromSlots(int id)
    {
        
    }

    private void UpdateSlot(int slotIndex)
    {
        
    }
    // public bool RemoveItem(int itemId, int weaponId, int accessoryId, int amout)
    // {
    //     if (!inventoryItems.ContainsKey(itemId)) return false;
    //     if (inventoryItems[itemId] < amout) return false;
    //     
    //     inventoryItems[itemId] -= amout;
    //     if (inventoryItems[itemId] <= 0)
    //     {
    //         inventoryItems.Remove(itemId);
    //         int index = FindSlotIndexOf(itemId);
    //         if (index != -1)
    //             RaiseSlotChanged(index);
    //     }
    //     return true;
    // }
    //
    // public Item GenerateItem(int itemId, int weaponId, int accessoryId)
    // {
    //     var item = new Item();
    //     item.Init(itemId);
    //     item.icon = GetIcon(item);
    //     return item;
    // }
    //
    // public void SwapItems(int from, int to)
    // {
    //     if (!IsValidSlot(from))
    // }
    
    // public Item GetItemInSlot(int slotIndex)
    // {
    //     return (inventoryItems != null && slotIndex < inventoryItems.Length) ? inventoryItems[slotIndex] : null;
    // }
    //
    // public void SetItemInSlot(int slotIndex, Item item)
    // {
    //     if (inventoryItems != null && slotIndex < inventoryItems.Length)
    //     {
    //         inventoryItems[slotIndex] = item;
    //         OnSlotChanged?.Invoke(slotIndex, item);
    //     }
    // }

    // public void LoadItemDataFromPlayer(int id, int amount)
    // {
    //     foreach (var item in inventoryItems)
    //     {
    //         if (item != null)
    //         {
    //             item = ItemInfo.Id
    //         }
    //     }
    //     // if (source == null)
    //     // {
    //     //     inventoryItems = Array.Empty<Item>();
    //     //     return;
    //     // }
    //     //
    //     // inventoryItems = new Item[source.Length];
    //     // Array.Copy(source, inventoryItems, source.Length);
    //     // for (int i = 0; i < source.Length; i++)
    //     //     OnSlotChanged?.Invoke(i, inventoryItems[i]);
    // }
    //
    // public void LoadWeaponDataFromPlayer(int id, int amout)
    // {
    //     
    // }
    //
    // // public Item[] SendIVDataToPlayer()
    // // {
    // //     if (inventoryItems == null) return Array.Empty<Item>();
    // //     var result = new Item[inventoryItems.Length];
    // //     Array.Copy(inventoryItems, result, inventoryItems.Length);
    // //     return result;
    // // }
    
    // public bool CanSwapItem(int from, int to)
    // {
    //     if (inventoryItems == null) return false;
    //     if (!((uint)from < inventoryItems.Length && (uint)to < inventoryItems.Length)) return false;
    //     return true;
    // }
    // public void SwapItemInSlot(int from, int to)
    // {
    //     if (!CanSwapItem(from, to)) return;
    //     (inventoryItems[to], inventoryItems[from]) = (inventoryItems[from], inventoryItems[to]);
    //     OnSlotChanged?.Invoke(from, inventoryItems[from]);
    //     OnSlotChanged?.Invoke(to, inventoryItems[to]);
    // }

    // public Sprite GetIcon(Item item)
    // {
    //     if (item == null || item.ItemInfo == null) return null;
    //     var path = item.ItemInfo.iconPathString;
    //     var load = Resources.Load<Sprite>(path);
    //     if (load != null)
    //         item.icon = load;
    //     return item.icon;
    // }
}
