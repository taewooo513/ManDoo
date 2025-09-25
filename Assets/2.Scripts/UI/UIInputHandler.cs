using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInputHandler : UIBase
{
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InGameInventoryUI inventoryUI;

    private InputActionMap map;
    private InputAction use;
    private InputAction drop;

    private readonly List<RaycastResult> hits = new();
    
    private void Awake()
    {
        if (raycaster == null)
            raycaster = GetComponentInParent<GraphicRaycaster>(true);
        if (inventoryUI == null)
        {
            var canvas = GetComponentInParent<Canvas>(true);
            if (canvas != null)
                inventoryUI = canvas.GetComponentInChildren<InGameInventoryUI>(true);
        }
        
        map = inputActions.FindActionMap("UI");
        use = map.FindAction("UseItem");
        drop = map.FindAction("DropItem");
    }

    private void OnEnable()
    {
        map.Enable();
        use.performed += OnUse;
        drop.performed += OnDrop;
    }

    private void OnDisable()
    {
        use.performed -= OnUse;
        drop.performed -= OnDrop;
        map.Disable();
    }

    private InventorySlotUI GetSlot()
    {
        if (raycaster == null || EventSystem.current == null)
            return null;
        
        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = Mouse.current.position.ReadValue();
        
        hits.Clear();
        raycaster.Raycast(pointer, hits);

        foreach (var hit in hits)
        {
            var slot = hit.gameObject.GetComponentInParent<InventorySlotUI>();
            if (slot != null)
                return slot;
        }
        return null;
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        var slot = GetSlot();
        if (slot == null) return;

        var item = InventoryManager.Instance.GetItemInSlot(slot.SlotIndex);
        if (item == null) return;
        
        if (InventoryManager.Instance.UseItemFromSlot(item.ItemId, 1))
            inventoryUI.RefreshSlots();
    }
    
    private void OnDrop(InputAction.CallbackContext context)
    {
        var slot = GetSlot();
        if (slot == null) return;

        var item = InventoryManager.Instance.GetItemInSlot((slot.SlotIndex));
        if (item == null) return;
        
        if (InventoryManager.Instance.RemoveItem(item.ItemId, 1))
            inventoryUI.RefreshSlots();
    }
}
