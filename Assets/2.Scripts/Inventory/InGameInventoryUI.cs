using UnityEngine;
using UnityEngine.UI;

// 게임 내 인벤토리 UI를 관리하는 클래스
public class InGameInventoryUI : UIBase
{
    // 인벤토리 UI가 속한 Canvas 컴포넌트
    public Canvas baseCanvas { get; private set; }
    
    // 인벤토리의 각 슬롯 UI 배열
    [SerializeField] private InventorySlotUI[] inventorySlots;
    
    // 각 슬롯에 저장된 아이템 배열
    private Item[] items;

    private void Awake()
    {
        // 부모 객체로부터 Canvas 컴포넌트 가져오기
        baseCanvas = GetComponentInParent<Canvas>();

        if (inventorySlots == null || inventorySlots.Length == 0)
            inventorySlots = GetComponentsInChildren<InventorySlotUI>(includeInactive: true);
        // items 배열이 없거나 슬롯 개수와 맞지 않으면 새로 생성
        if (items == null || items.Length != inventorySlots.Length)
            items = new Item[inventorySlots.Length];
    }
    
    void Start()
    {
        // 각 인벤토리 슬롯 초기화
        for (int i = 0; i < inventorySlots.Length; i++)
            inventorySlots[i].Init(i, this);
        
        // 슬롯 UI 새로고침
        RefreshSlots();
    }

    // 슬롯이 클릭되었을 때 호출되는 메서드
    public void OnSlotClicked(int slotIndex)
    {
        var item = items[slotIndex];
        if (item == null) return;
        
        // item.UseItem();
        RefreshSlots();
    }

    // 모든 인벤토리 슬롯의 UI를 새로고침하는 메서드
    public void RefreshSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var slot = inventorySlots[i];
            var item = items[i];

            // 아이템이 없거나 아이콘이 없는 경우
            if (item == null || item.icon == null)
            {
                slot.SetIcon(null);
                if (slot.itemIcon != null) 
                    slot.itemIcon.gameObject.SetActive(false);
            }
            // 아이템이 있는 경우
            else
            {
                if (slot.itemIcon != null)
                    slot.itemIcon.gameObject.SetActive(true);
                slot.SetIcon(item.icon);
            }
        }
    }

    // 아이템을 한 슬롯에서 다른 슬롯으로 이동시키는 메서드
    public void MoveItem(int from, int to)
    {
        // 같은 위치거나 잘못된 인덱스인 경우 리턴
        if (from == to) return;
        if (from < 0 || from >= items.Length) return;
        if (to < 0 || to >= items.Length) return;

        if (!ItemManager.Instance.CanSwapItem(items[from], from, to)) return;
        ItemManager.Instance.SwapItem(items, from, to);
        RefreshSlots();
    }
}