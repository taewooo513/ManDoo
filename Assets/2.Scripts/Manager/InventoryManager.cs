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
    private readonly int[] slotItemIds = new int[10]; // 슬롯에 들어있는 아이템 id
    private readonly int[] slotStackCounts = new int[10];
    private readonly Item[] equippedItems = new Item[3];
    public event Action<int, Item> OnSlotChanged; // slot index, item
    public event Action<EquipmentSlotType, Item> OnEquipChanged; // slot type, item

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < slotItemIds.Length; i++)
            slotItemIds[i] = -1;
    }
    public void AddItem(int id, int amount) // 아이템 저장
    {
        if (amount <= 0) return; // 예외처리
        
        var tempItem = ItemManager.Instance.CreateItem(id); // 새 아이템 임시로 생성
        int maxStack = Mathf.Max(1, tempItem.ItemInfo.maxCount); // id 로 조회한 아이템의 최대 한도수를 저장

        for (int i = 0; i < slotItemIds.Length; i++) // 기존에 있던 슬롯에 저장 시도
        {
            if (slotItemIds[i] != id) continue; // 다른 id 의 슬롯은 스킵
            var current = slotStackCounts[i]; // 현재 슬롯에 저장되어있는 아이템 개수 
            if (current == maxStack) continue; // 현재 슬롯이 꽉 차있으면 스킵

            var canAdd = Mathf.Min(maxStack - current, amount); // 최대한도와 현재 슬롯에 저장되어 있는 개수 비교
            slotStackCounts[i] += canAdd; // 현재 슬롯에 저장되어 있는 개수 업데이트
            amount -= canAdd; // 저장하고자 하는 개수를 저장한 만큼 차감
            UpdateSlot(i); // 슬롯 업데이트
        }

        for (int i = 0; i < slotItemIds.Length; i++) // 빈 슬롯에 저장
        {
            if (slotItemIds[i] != -1) continue; // 빈 슬롯이 아니면 스킵

            var targetSlot = Mathf.Min(maxStack, amount); // 슬롯에 저장할 수 있는 최대 개수와 현재 개수 중 작은 값 선택
            slotItemIds[i] = id; // 슬롯에 아이템 id 저장
            slotStackCounts[i] = targetSlot; // 슬롯에 아이템 개수 저장
            amount -= targetSlot; // 저장한 만큼 amount 에서 차감
            UpdateSlot(i); // 슬롯 업데이트
        }

        if (amount > 0)
            Debug.Log($"저장 한도 초과! 아이템 id: {id}. 부족한 수량: {amount}.");
        
        // itemCounts.TryGetValue(id, out int currentCount); // 현재 저장되어있는 id 를 가진 아이템의 개수를 반환
        // int updatedCount = Mathf.Min(currentCount + amount, maxStack); // 현재 아이템 개수랑 최대한도를 비교해서 업데이트
        // itemCounts[id] = updatedCount;
        
        // // if (itemCounts.ContainsKey(id) && maxCount < Item.ItemInfo.maxCount)
        // //     itemCounts[id] += amount;
        // // else
        // //     itemCounts[id] = amount;
        //
        // if (!IsShownInSlots(id))
        //     AssignToEmptySlot(id);
    }

    public bool RemoveItem(int id, int amount = 1)
    {
        if (amount <= 0) return true;
        int remaining = amount;

        for (int i = 0; i < slotItemIds.Length && remaining > 0; i++)
        {
            if (slotItemIds[i] != id) continue;
            var current = slotStackCounts[i];
            if (current <= 0) continue;
            
            var canRemove = Mathf.Min(current, remaining);
            slotStackCounts[i] -= canRemove;
            remaining -= canRemove;
            if (slotStackCounts[i] <= 0)
                slotItemIds[i] = -1;
            UpdateSlot(i);
        }
        return remaining <= 0;
        // if (!itemCounts.TryGetValue(id, out int count)) return false;
        // if (count < amount) return false;
        // count -= amount;
        // if (count <= 0)
        // {
        //     itemCounts.Remove(id);
        //     RemoveFromSlots(id);
        // }
        //     
        // else
        //     itemCounts[id] = count;
        // return true;
    }
    public int GetItemCount(int id)
    {
        int total = 0;
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            if (slotItemIds[i] == id)
                total += slotStackCounts[i];
        }
        return total;
        //itemCounts.TryGetValue(id, out int count) ? count : 0;
    }

    public int GetSlotCount() => slotItemIds.Length;

    public Item GetItemInSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return null;
        var id = slotItemIds[slotIndex];
        if (id == -1) return null;
        if (slotStackCounts[slotIndex] <= 0) return null;
        return ItemManager.Instance.CreateItem(id);
    }

    public void SwapSlotItems(int from, int to)
    {
        if (!IsValidSlot(from) || !IsValidSlot(to) || from == to) return;
        
        (slotItemIds[to], slotItemIds[from]) = (slotItemIds[from], slotItemIds[to]);
        (slotStackCounts[to], slotStackCounts[from]) = (slotStackCounts[from], slotStackCounts[to]);
        
        UpdateSlot(from);
        UpdateSlot(to);
    }

    public void CustomizeSlot(int slotIndex, int id, int count = 1) // 테스트용으로 사용할 수도 있음
    {
        if (!IsValidSlot(slotIndex)) return;

        if (id < 0 || count <= 0)
        {
            slotItemIds[slotIndex] = -1;
            slotStackCounts[slotIndex] = 0;
        }
        else
        {
            slotItemIds[slotIndex] = id;
            slotStackCounts[slotIndex] = count;
        }
        UpdateSlot(slotIndex);
    }

    public Item GetEquipped(EquipmentSlotType slotType) => equippedItems[(int)slotType];

    public bool TryEquipFromInventory(int from, EquipmentSlotType to) // 인벤토리에서 장착해제
    {
        var item = GetItemInSlot(from);
        if (item == null) return false;
        if (!ItemManager.Instance.CanEquipItem(item, to)) return false;

        if (!UseItemFromSlot(from, 1)) return false;

        var prevSlot = equippedItems[(int)to];
        if (prevSlot != null)
            AddItem(prevSlot.ItemId, 1);

        equippedItems[(int)to] = item;
        OnEquipChanged?.Invoke(to, item);
        return true;
    }

    private bool UseItemFromSlot(int slotIndex, int amount)
    {
        if (!IsValidSlot(slotIndex) || amount <= 0) return false;
        if (slotItemIds[slotIndex] == -1 || slotStackCounts[slotIndex] < amount) return false;

        slotStackCounts[slotIndex] -= amount;
        if (slotStackCounts[slotIndex] <= 0)
        {
            slotItemIds[slotIndex] = -1;
            slotStackCounts[slotIndex] = 0;
        }
        UpdateSlot(slotIndex);
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

    private void AssignToEmptySlot(int id)
    {
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            var slot = slotItemIds[i];
            if (slot == -1)
            {
                slotItemIds[i] = id;
                slotStackCounts[i] = 1;
                UpdateSlot(i);
                return;
            }
        }
    }
    
    // private bool IsShownInSlots(int id)
    // {
    //     foreach (var i in slotItemIds)
    //     {
    //         if (i == id)
    //             return true;
    //     }
    //     return false;
    // }
    
    // private void RemoveFromSlots(int id)
    // {
    //     for (int i = 0; i < slotItemIds.Length; i++)
    //     {
    //         var slot = slotItemIds[i];
    //         if (slot != -1 && slot == id)
    //         {
    //             slotItemIds[i] = -1;
    //             UpdateSlot(i);
    //         }
    //     }
    // }

    private void UpdateSlot(int slotIndex) => OnSlotChanged?.Invoke(slotIndex, GetItemInSlot(slotIndex)); // TODO: 개수 표시하는 UI 만들면 이벤트 변경
}
