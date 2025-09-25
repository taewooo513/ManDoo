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

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < slotItemIds.Length; i++)
            slotItemIds[i] = -1;
    }
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

    public Item GetItemInSlot(int slotIndex)
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

    public bool TryEquipFromInventory(int from, EquipmentSlotType to) // 인벤토리에서 장착해제
    {
        var item = GetItemInSlot(from);
        if (item == null) return false;
        if (!ItemManager.Instance.CanEquipItem(item, to)) return false;

        if (!RemoveItem(item.ItemId, 1)) return false;
        slotItemIds[from] = -1;
        UpdateSlot(from);

        var prevSlot = equippedItems[(int)to];
        if (prevSlot != null)
            AddItem(prevSlot.ItemId, 1);

        equippedItems[(int)to] = item;
        OnEquipChanged?.Invoke(to, item);
        return true;
    }

    public bool TryUnequipToInventory(EquipmentSlotType from) // 장비창에서 인벤창으로 옮기면서 장착해제
    {
        var equip = equippedItems[(int)from];
        if (equip == null) return false;
        
        AddItem(equip.ItemId, 1);
        equippedItems[(int)from] = null;
        OnEquipChanged?.Invoke(from, null);
        return true;
    }
    
    private bool IsValidSlot(int slotIndex) => slotIndex >= 0 && slotIndex < slotItemIds.Length;

    private bool IsShownInSlots(int id)
    {
        foreach (var i in slotItemIds)
        {
            if (i == id)
                return true;
        }
        return false;
    }

    private void AssignToEmptySlot(int id)
    {
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            var slot = slotItemIds[i];
            if (slot == -1)
            {
                slotItemIds[i] = id;
                UpdateSlot(i);
                return;
            }
        }
    }

    private void RemoveFromSlots(int id)
    {
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            var slot = slotItemIds[i];
            if (slot != -1 && slot == id)
            {
                slotItemIds[i] = -1;
                UpdateSlot(i);
            }
        }
    }

    private void UpdateSlot(int slotIndex) => OnSlotChanged?.Invoke(slotIndex, GetItemInSlot(slotIndex));
}
