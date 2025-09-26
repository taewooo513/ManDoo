using UnityEngine;

/// <summary>
/// 게임 내 인벤토리 UI를 관리하는 클래스
/// </summary>
public class InGameInventoryUI : UIBase
{
    // 테스트 모드 활성화 여부를 설정하는 플래그
    public bool isTestMode = false;
    // 테스트용 아이템 아이콘
    [SerializeField] private Sprite testIcon;
    // 테스트용 아이콘 개수
    [SerializeField] private int iconCount = 0;
    // 기본 캔버스 참조
    public Canvas baseCanvas { get; private set; }
    // 인벤토리 슬롯 UI 배열    
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
        // 인벤토리 매니저의 슬롯 변경 이벤트에 핸들러 등록
        InventoryManager.Instance.OnSlotChanged += HandleSlotChanged;
    }

    private void OnDisable()
    {
        // 인벤토리 매니저의 슬롯 변경 이벤트에서 핸들러 제거
        InventoryManager.Instance.OnSlotChanged -= HandleSlotChanged;
    }
    
    /// <summary>
    /// 인벤토리 슬롯들을 초기화하고 UI를 새로고침
    /// </summary>
    void Start()
    {
        // 각 인벤토리 슬롯 초기화
        //for (int i = 0; i < inventorySlots.Length; i++)
        //{
        //    var slot = inventorySlots[i];
        //    slot.Init(i, this);
        //    if (slot.Icon != null)
        //        slot.Icon.Setup(i, this, baseCanvas);

        //    if (!isTestMode)
        //    {
        //        slot.SetIcon(null);
        //        slot.SetInteractableIcon(false);
        //    }
        //}
        //// 슬롯 UI 새로고침
        //RefreshSlots();
    }
    
    /// <summary>
    /// 모든 인벤토리 슬롯의 UI를 새로고침하는 메서드
    /// </summary>
    public void RefreshSlots()
    {
        // 테스트 모드
        if (isTestMode)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                var itemCount = InventoryManager.Instance.GetSlotCount(i);
                var hasIcon = (testIcon != null) && (i < iconCount);
                inventorySlots[i].SetIcon(hasIcon ? testIcon : null);
                inventorySlots[i].SetInteractableIcon(hasIcon);
                inventorySlots[i].UpdateSlotCountText(itemCount);
            }
            return;
        }
        
        // 실제 게임 모드
        var im = InventoryManager.Instance;
        // 각 슬롯의 아이템 아이콘 업데이트
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var item = im.GetItemInSlot(i);
            var icon = (item != null) ? item.icon : null;
            inventorySlots[i].SetIcon(icon);
            inventorySlots[i].SetInteractableIcon(icon != null);
            inventorySlots[i].UpdateSlotCountText(im.GetSlotCount(i));
        }
    }

    /// <summary>
    /// 슬롯 변경 이벤트 처리 메서드
    /// </summary>
    /// <param name="slotIndex">변경된 슬롯의 인덱스</param>
    /// <param name="item">변경된 아이템</param>
    private void HandleSlotChanged(int slotIndex, Item item)
    {
        var slotCount = InventoryManager.Instance.GetSlotCount(slotIndex);
        if (isTestMode) return;
        if (inventorySlots == null || slotIndex < 0 || slotIndex >= inventorySlots.Length) return;
        var icon = (item != null) ? item.icon : null;
        var slot = inventorySlots[slotIndex];
        slot.SetIcon(icon);
        slot.SetInteractableIcon(icon != null);
        slot.UpdateSlotCountText(slotCount);
    }

    /// <summary>
    /// 아이템을 다른 슬롯으로 이동하는 메서드
    /// </summary>
    /// <param name="from">시작 슬롯 인덱스</param>
    /// <param name="to">목표 슬롯 인덱스</param>
    public void MoveItem(int from, int to)
    {
        if (from == to) return;
        
        // 테스트 모드
        if (isTestMode) return;
        
        // 실제 게임 모드
        InventoryManager.Instance.SwapSlotItems(from, to);
    }
    
    /// <summary>
    /// 슬롯 클릭 이벤트 처리 메서드
    /// </summary>
    /// <param name="slotIndex">클릭된 슬롯의 인덱스</param>
    public void OnSlotClicked(int slotIndex)
    {
        var item = InventoryManager.Instance.GetItemInSlot(slotIndex);
        if (item == null) return;
        
        // TODO: 해제/사용 구현
        RefreshSlots();
    }
}