using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 게임 내 인벤토리 UI를 관리하는 클래스
/// </summary>
public class InGameInventoryUI : UIBase //inventorymanager 추가.
{
    // 테스트 모드 활성화 여부를 설정하는 플래그
    public bool isTestMode = true;
    public Canvas baseCanvas { get; private set; }
    [SerializeField] private InventorySlotUI[] inventorySlots;
    private void Awake()
    {
        // 부모 객체로부터 Canvas 컴포넌트 가져오기
        baseCanvas = GetComponentInParent<Canvas>();

        // 인벤토리 슬롯이 없으면 자식 객체들에서 가져옴
        if (inventorySlots == null || inventorySlots.Length == 0)
            inventorySlots = GetComponentsInChildren<InventorySlotUI>(includeInactive: true);
    }

    private void OnEnable()
    {
        InventoryManager.Instance.OnSlotChanged += HandleSlotChanged;
    }

    private void OnDisable()
    {
        InventoryManager.Instance.OnSlotChanged -= HandleSlotChanged;
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
        var item = InventoryManager.Instance.GetItemInSlot(slotIndex);
        if (item == null) return;
        
        // TODO: 사용/버리기 구현
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
        var im = InventoryManager.Instance;
        // 각 슬롯의 아이템 아이콘 업데이트
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var item = im.GetItemInSlot(i);
            var icon = im.GetIcon(item);
            inventorySlots[i].SetIcon(icon);
            inventorySlots[i].SetInteractableIcon(icon != null);
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
        InventoryManager.Instance.SwapItemInSlot(from, to);
    }

    private void HandleSlotChanged(int slotIndex, Item item)
    {
        if (isTestMode) return;
        if (inventorySlots == null || slotIndex >= inventorySlots.Length) return;
        
        var icon = InventoryManager.Instance.GetIcon(item);
        inventorySlots[slotIndex].SetIcon(icon);
        inventorySlots[slotIndex].SetInteractableIcon(icon != null);
    }
}