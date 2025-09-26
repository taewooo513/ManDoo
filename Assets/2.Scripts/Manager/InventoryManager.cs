using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 시스템을 관리하는 매니저 클래스
/// </summary>
public enum EquipmentSlotType
{
    Weapon = 0,      // 무기 슬롯
    Accessory1 = 1,  // 첫번째 악세서리 슬롯
    Accessory2 = 2,  // 두번째 악세서리 슬롯
}

public class InventoryManager : Singleton<InventoryManager>
{
    private readonly int[] slotItemIds = new int[10];      // 슬롯에 들어있는 아이템 id
    private readonly eItemType[] slotItemTypes = new eItemType[10];
    private readonly int[] slotStackCounts = new int[10];  // 각 슬롯의 아이템 개수
    private readonly Item[] equippedItems = new Item[3];   // 장착된 아이템 배열
    private readonly Weapon equippedWeapon;
    public event Action<int, Item> OnSlotChanged;          // 슬롯 변경 시 호출되는 이벤트 (slot index, item)
    public event Action<EquipmentSlotType, Item> OnEquipChanged; // 장비 변경 시 호출되는 이벤트 (slot type, item)

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < slotItemIds.Length; i++) // 슬롯 초기화
            slotItemIds[i] = -1;
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가하는 메서드
    /// </summary>
    public bool TryAddItem(eItemType type, int id, int amount)
    {
        if (amount <= 0) return false; // 예외처리
        int maxStack = 0;
        
        if (type == eItemType.Consumable)
        {
            var tempItem = ItemManager.Instance.CreateItem(id); // 새 아이템 임시로 생성
            maxStack = Mathf.Max(1, tempItem.ItemInfo.maxCount); // id 로 조회한 아이템의 최대 한도수를 저장
            
            for (int i = 0; i < slotItemIds.Length && amount > 0; i++) // 기존에 있던 슬롯에 저장 시도
            {
                if (slotItemIds[i] != id) continue; // 다른 id 의 슬롯은 스킵
                var current = slotStackCounts[i]; // 현재 슬롯에 저장되어있는 아이템 개수
                slotItemTypes[i] = type;
                if (current == maxStack) continue; // 현재 슬롯이 꽉 차있으면 스킵

                var canAdd = Mathf.Min(maxStack - current, amount); // 최대한도와 현재 슬롯에 저장되어 있는 개수 비교
                slotStackCounts[i] += canAdd; // 현재 슬롯에 저장되어 있는 개수 업데이트
                amount -= canAdd; // 저장하고자 하는 개수를 저장한 만큼 차감
                UpdateSlot(i); // 슬롯 업데이트
            }

            for (int i = 0; i < slotItemIds.Length && amount > 0; i++) // 빈 슬롯에 저장
            {
                if (slotItemIds[i] != -1) continue; // 빈 슬롯이 아니면 스킵

                var targetSlot = Mathf.Min(maxStack, amount); // 슬롯에 저장할 수 있는 최대 개수와 현재 개수 중 작은 값 선택
                slotItemIds[i] = id; // 슬롯에 아이템 id 저장
                slotItemTypes[i] = type;
                slotStackCounts[i] = targetSlot; // 슬롯에 아이템 개수 저장
                amount -= targetSlot; // 저장한 만큼 amount 에서 차감
                UpdateSlot(i); // 슬롯 업데이트
            }
        }

        if (type == eItemType.Weapon)
        {
            maxStack = 1;
            for (int i = 0; i < slotItemIds.Length; i++)
            {
                if (slotStackCounts[i] >= maxStack) continue;

                slotItemIds[i] = id;
                slotItemTypes[i] = type;
                slotStackCounts[i] = maxStack;
                amount -= maxStack;
                UpdateSlot(i);
            }
        }
        
        if (amount > 0)
        {
            Debug.Log($"저장 한도 초과! 아이템 id: {id}. 부족한 수량: {amount}.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 제거하는 메서드
    /// </summary>
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
    }

    /// <summary>
    /// 특정 아이템의 전체 개수를 반환하는 메서드
    /// </summary>
    public int GetItemCount(int id)
    {
        int total = 0;
        for (int i = 0; i < slotItemIds.Length; i++)
        {
            if (slotItemIds[i] == id)
                total += slotStackCounts[i];
        }
        return total;
    }

    /// <summary>
    /// 인벤토리 슬롯의 총 개수를 반환하는 메서드
    /// </summary>
    public int GetSlotCount() => slotItemIds.Length;
    
    public eItemType GetItemType(int slotIndex) => slotItemTypes[slotIndex];

    /// <summary>
    /// 특정 슬롯의 아이템을 반환하는 메서드
    /// </summary>
    public Item GetItemInSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex)) return null;
        var id = slotItemIds[slotIndex];
        if (id == -1) return null;
        if (slotStackCounts[slotIndex] <= 0) return null;
        return ItemManager.Instance.CreateItem(id);
    }

    /// <summary>
    /// 두 슬롯의 아이템을 서로 교환하는 메서드
    /// </summary>
    public void SwapSlotItems(int from, int to)
    {
        if (!IsValidSlot(from) || !IsValidSlot(to) || from == to) return;
        
        (slotItemIds[to], slotItemIds[from]) = (slotItemIds[from], slotItemIds[to]);
        (slotStackCounts[to], slotStackCounts[from]) = (slotStackCounts[from], slotStackCounts[to]);
        (slotItemTypes[to], slotItemTypes[from]) = (slotItemTypes[from], slotItemTypes[to]);
        
        UpdateSlot(from);
        UpdateSlot(to);
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 설정하는 메서드 (테스트용)
    /// </summary>
    public void CustomizeSlot(int slotIndex, int id, int count = 1)
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

    /// <summary>
    /// 장착된 아이템을 반환하는 메서드
    /// </summary>
    public Item GetEquippedItem(EquipmentSlotType slotType) => equippedItems[(int)slotType];

    public Weapon GetEquippedWeapon() => equippedWeapon;

    /// <summary>
    /// 인벤토리에서 장비 슬롯으로 아이템을 장착하는 메서드
    /// </summary>
    public bool TryEquipFromInventory(int from, EquipmentSlotType to) // TODO: equippedWeapon 으로 변경
    {
        var item = GetItemInSlot(from);
        if (item == null) return false;
        if (slotItemTypes[from] != eItemType.Weapon) return false;
        if (!ItemManager.Instance.CanEquipItem(item, to)) return false;

        if (!UseItemFromSlot(from, 1)) return false;
        
        var prevSlot = equippedItems[(int)to];
        
        if (prevSlot != null)
            TryAddItem(eItemType.Weapon, prevSlot.ItemId, 1);
        
        equippedItems[(int)to] = item;
        OnEquipChanged?.Invoke(to, item);
        return true;
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 사용하는 메서드
    /// </summary>
    public bool UseItemFromSlot(int slotIndex, int amount)
    {
        if (!IsValidSlot(slotIndex) || amount <= 0) return false;
        if (slotItemIds[slotIndex] == -1 || slotStackCounts[slotIndex] < amount) return false;

        var tempItem = GetItemInSlot(slotIndex);
        var type = tempItem.ItemInfo.itemType;

        switch (type)
        {
            case ItemType.Gold: // && StoreManager.Instance.IsInStore();
                slotStackCounts[slotIndex] -= amount;
                if (slotStackCounts[slotIndex] <= 0)
                {
                    slotItemIds[slotIndex] = -1;
                    slotStackCounts[slotIndex] = 0;
                }
                UpdateSlot(slotIndex);
                break;
            
            case ItemType.BattleSupport: // && BattleManager.Instance.IsBattleInProgress()
                slotStackCounts[slotIndex] -= amount;
                if (slotStackCounts[slotIndex] <= 0)
                {
                    slotItemIds[slotIndex] = -1;
                    slotStackCounts[slotIndex] = 0;
                }
                UpdateSlot(slotIndex);
                break;

            case ItemType.ExplorerSupport: // && !BattleManager.Instance.IsBattleInProgress()
                slotStackCounts[slotIndex] -= amount;
                if (slotStackCounts[slotIndex] <= 0)
                {
                    slotItemIds[slotIndex] = -1;
                    slotStackCounts[slotIndex] = 0;
                }
                UpdateSlot(slotIndex);
                break;
        }
        return true;
    }
    
    /// <summary>
    /// 장비를 해제하고 인벤토리로 이동시키는 메서드
    /// </summary>
    public bool TryUnequipToInventory(EquipmentSlotType from) // TODO: EquippedWeapon 을 이용해서 변경
    {
        var equip = equippedItems[(int)from];
        if (equip == null) return false;
        
        TryAddItem(eItemType.Weapon, equip.ItemId, 1);
        equippedItems[(int)from] = null;
        OnEquipChanged?.Invoke(from, null);
        return true;
    }
    
    /// <summary>
    /// 슬롯 인덱스가 유효한지 검사하는 메서드
    /// </summary>
    private bool IsValidSlot(int slotIndex) => slotIndex >= 0 && slotIndex < slotItemIds.Length;

    /// <summary>
    /// 빈 슬롯에 아이템을 할당하는 메서드
    /// </summary>
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
    
    /// <summary>
    /// 슬롯 변경 이벤트를 발생시키는 메서드
    /// </summary>
    private void UpdateSlot(int slotIndex) => OnSlotChanged?.Invoke(slotIndex, GetItemInSlot(slotIndex));

    public int GetSlotCount(int slotIndex) => slotStackCounts[slotIndex];
}