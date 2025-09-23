using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 게임 내 인벤토리 UI를 관리하는 클래스
/// </summary>
public class InGameInventoryUI : UIBase
{
    // 테스트 모드 활성화 여부를 설정하는 플래그
    public bool isTestMode = true;

    /// <summary>
    /// 인벤토리 UI가 속한 Canvas 컴포넌트
    /// </summary> 
    public Canvas baseCanvas { get; private set; }
    
    /// <summary>
    /// 인벤토리의 각 슬롯 UI 배열
    /// </summary>
    [SerializeField] private InventorySlotUI[] inventorySlots;
    
    /// <summary>
    /// 각 슬롯에 저장된 아이템 배열
    /// </summary>
    private Item[] items;

    /// <summary>
    /// Awake에서 초기 설정을 수행
    /// </summary>
    private void Awake()
    {
        // 부모 객체로부터 Canvas 컴포넌트 가져오기
        baseCanvas = GetComponentInParent<Canvas>();

        // 인벤토리 슬롯이 없으면 자식 객체들에서 가져옴
        if (inventorySlots == null || inventorySlots.Length == 0)
            inventorySlots = GetComponentsInChildren<InventorySlotUI>(includeInactive: true);
        
        // items 배열이 없거나 슬롯 개수와 맞지 않으면 새로 생성
        if (items == null || items.Length != inventorySlots.Length)
            items = new Item[inventorySlots.Length];
        //inventorySlots = inventorySlots.OrderBy(slot => slot.transform.GetSiblingIndex()).ToArray();
    }
    
    /// <summary>
    /// 인벤토리 슬롯들을 초기화하고 UI를 새로고침
    /// </summary>
    void Start()
    {
        // 각 인벤토리 슬롯 초기화
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var slot = inventorySlots[i];
            slot.Init(i, this);
            if (slot.Icon != null)
                slot.Icon.Setup(i, this, baseCanvas);
        }
        // 슬롯 UI 새로고침
        RefreshSlots();
    }

    /// <summary>
    /// 슬롯이 클릭되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex">클릭된 슬롯의 인덱스</param>
    public void OnSlotClicked(int slotIndex)
    {
        var item = items[slotIndex];
        if (item == null) return;
        
        // item.UseItem();
        RefreshSlots();
    }

    /// <summary>
    /// 모든 인벤토리 슬롯의 UI를 새로고침하는 메서드
    /// </summary>
    public void RefreshSlots()
    {
        // 테스트 모드일 경우 테스트용 아이콘 표시
        if (isTestMode)
        {
            foreach (var slot in inventorySlots)
            {
                slot.ShowIconForTestMode();
            }

            return;
        }
        
        // 각 슬롯의 아이템 아이콘 업데이트
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var item = items[i];
            var icon = item != null ? item.icon : null;
            inventorySlots[i].SetIcon(icon);
        }
    }

    /// <summary>
    /// 아이템을 한 슬롯에서 다른 슬롯으로 이동시키는 메서드
    /// </summary>
    /// <param name="from">이동할 아이템이 있는 슬롯 인덱스</param>
    /// <param name="to">이동할 목표 슬롯 인덱스</param>
    public void MoveItem(int from, int to)
    {
        // 같은 위치거나 잘못된 인덱스인 경우 리턴
        if (from == to) return;
        if (from < 0 || from >= items.Length) return;
        if (to < 0 || to >= items.Length) return;

        // 아이템 교환이 가능한지 확인하고 교환 실행
        if (!ItemManager.Instance.CanSwapItem(items[from], from, to)) return;
        ItemManager.Instance.SwapItem(items, from, to);
        RefreshSlots();
    }
}