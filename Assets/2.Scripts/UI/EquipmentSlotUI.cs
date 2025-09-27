using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IDroppingTarget
{
    [SerializeField] private EquipmentSlotType slotType = 0;
    public EquipmentSlotType GetSlotType() => slotType;
    [SerializeField] private Image icon;
    
    private void Awake()
    {
        if (icon == null)
            icon = GetComponentInChildren<Image>(true);
    }

    private void OnEnable() => InventoryManager.Instance.OnWeaponEquipChanged += HandleWeaponChanged;

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnWeaponEquipChanged -= HandleWeaponChanged;
    }

    private void HandleWeaponChanged(Weapon weapon)
    {
        if (slotType == EquipmentSlotType.Weapon)
            RefreshIcon();
    }
    public bool CanDrop(IDraggingObject obj)
    {
        if (obj == null) return false;
        
        if (obj.Origin != DragOrigin.Inventory) return false;
        if (slotType != EquipmentSlotType.Weapon) return false;
        if (obj.ItemType != eItemType.Weapon) return false;
        if (obj.SlotIndex < 0) return false;
        return true;
    }

    public bool Drop(IDraggingObject obj)
    {
        if (!CanDrop(obj)) return false;
        
        bool can = InventoryManager.Instance.TryEquipFromInventory(obj.SlotIndex, slotType);
        if (can)
            RefreshIcon();
        return can;
    }

    public Transform RootObject => this.transform;

    public void RefreshIcon()
    {
        var equippied = InventoryManager.Instance.GetEquippedWeapon();
        var equipmentIcon = (equippied != null) ? equippied.icon : null;

        if (icon != null)
        {
            icon.sprite = equipmentIcon;
            icon.enabled = (equipmentIcon != null);
        }
    }
}
