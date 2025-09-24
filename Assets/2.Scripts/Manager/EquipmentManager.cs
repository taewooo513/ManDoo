using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType
{
    Weapon = 0,
    Accessory1 = 1,
    Accessory2 = 2,
}
public class EquipmentManager : Singleton<EquipmentManager>
{
    private Weapon equippedWeapon;
    // private readonly Accessories[] accessories; 장신구 데이터 테이블 추가시.
    public event Action<Weapon> OnWeaponChanged;
    // public event Action<EquipmentSlotType, accessories> OnAccessorySlotChanged;
    
    protected override void Awake()
    {
        base.Awake();
        // accessories = new Accessories[2]
    }

    // public Item GetEquippedItem(EquipmentSlotType slotType)
    // {
    //     return equippedItems != null ? equippedItems[(int)slotType] : null;
    // }
    //
    // public void SetEquippedItem(EquipmentSlotType slotType, Item item)
    // {
    //     if (equippedItems == null) return;
    //     equippedItems[(int)slotType] = item;
    //     OnSlotChanged?.Invoke(slotType, item);
    // }
    //
    // public bool CanEquip(Weapon weapon, EquipmentSlotType slotType)
    // {
    //     if (item == null || item.ItemInfo == null) return false;
    //
    //     if (slotType == EquipmentSlotType.Weapon)
    //     {
    //         weapon.weaponType
    //     }
    //     return true;
    // }
}
