using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInventoryUI : UIBase
{
    // public Button[] slotButtons; // 10개 버튼 Inspector에서 연결
    private Canvas baseCanvas;
    [SerializeField] private InventoryItemUI itemIconPrefab;
    [SerializeField] private InventorySlotUI[] inventorySlots;
    private Item[] items;

    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
    }
    
    void Start()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].Init(i, this);
        }
        
        RefreshSlots();
        // for (int i = 0; i < slotButtons.Length; i++)
        // {
        //     int idx = i;
        //     slotButtons[i].onClick.AddListener(() => InventoryManager.Instance.UseItem(idx));
        // }
    }

    public void OnSlotClicked(int slotIndex)
    {
        // TODO: UseItem() 작성 후 사용하기.
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        // TODO:
        // 1. 모든 슬롯의 default icon 제거.

        foreach (var inventorySlot in inventorySlots) // 기존에 있던 아이템 아이콘 제거
        {
            for (int i = inventorySlot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(inventorySlot.transform.GetChild(i).gameObject);
            }
        }
        
        // TODO: 인벤토리 슬롯에 현재 가지고 있는 아이템 넣어놓기.
    }

    public void MoveItem(int from, int to)
    {
        if (from == to) return;
        if (from < 0 || from >= items.Length) return;
        if (to < 0 || to >= items.Length) return;

        // TODO: 아이템을 스왑하는 로직 추가
        RefreshSlots();
    }
}
