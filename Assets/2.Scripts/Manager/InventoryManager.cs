using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    private Item[] inventoryItems;
    //private readonly Dictionary<string, Sprite> iconCashe = new();
    public event Action<int, Item> OnSlotChanged;
    protected override void Awake()
    {
        base.Awake();
        
        if (inventoryItems == null || inventoryItems.Length == 0)
            inventoryItems = new Item[10]; // TODO: 이거 나중에 플레이어가 현재 가지고 있는 아이템만 hold 하게끔 설정.
    }
    
    public int GetItemCount => inventoryItems.Length;

    public Item GetItemInSlot(int slotIndex)
    {
        return (inventoryItems != null && slotIndex < inventoryItems.Length) ? inventoryItems[slotIndex] : null;
    }

    public void SetItemInSlot(int slotIndex, Item item)
    {
        if (inventoryItems != null && slotIndex < inventoryItems.Length)
        {
            inventoryItems[slotIndex] = item;
            OnSlotChanged?.Invoke(slotIndex, item);
        }
    }

    public void LoadIVDataFromPlayer(Item[] source)
    {
        if (source == null)
        {
            inventoryItems = Array.Empty<Item>();
            return;
        }
        
        inventoryItems = new Item[source.Length];
        Array.Copy(source, inventoryItems, source.Length);
        for (int i = 0; i < source.Length; i++)
            OnSlotChanged?.Invoke(i, inventoryItems[i]);
    }

    public Item[] SendIVDataToPlayer()
    {
        if (inventoryItems == null) return Array.Empty<Item>();
        var result = new Item[inventoryItems.Length];
        Array.Copy(inventoryItems, result, inventoryItems.Length);
        return result;
    }
    
    public bool CanSwapItem(int from, int to)
    {
        if (inventoryItems == null) return false;
        if (!((uint)from < inventoryItems.Length && (uint)to < inventoryItems.Length)) return false;
        return true;
    }
    public void SwapItemInSlot(int from, int to)
    {
        if (!CanSwapItem(from, to)) return;
        (inventoryItems[to], inventoryItems[from]) = (inventoryItems[from], inventoryItems[to]);
        OnSlotChanged?.Invoke(from, inventoryItems[from]);
        OnSlotChanged?.Invoke(to, inventoryItems[to]);
    }

    public Sprite GetIcon(Item item)
    {
        if (item == null || item.ItemInfo == null) return null;
        var path = item.ItemInfo.iconPathString;
        var load = Resources.Load<Sprite>(path);
        if (load != null)
            item.icon = load;
        return item.icon;
    }
}
