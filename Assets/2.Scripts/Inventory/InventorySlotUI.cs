using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리의 각 슬롯 UI를 관리하는 클래스
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IDropHandler
{
    /// <summary>
    /// 슬롯의 인덱스 번호
    /// </summary>
    public int SlotIndex { get; private set; }
    
    /// <summary>
    /// 이 슬롯을 소유한 인벤토리 UI 
    /// </summary>
    private InGameInventoryUI owner;
    
    /// <summary>
    /// 슬롯에 표시될 아이템 아이콘 UI
    /// </summary>
    public InventoryItemUI Icon { get; private set; }

    /// <summary>
    /// 슬롯의 배경 이미지
    /// </summary>
    [SerializeField] private Image slotBackground;

    /// <summary>
    /// 슬롯의 아이콘 이미지
    /// </summary>
    [SerializeField] private Image slotIcon;

    /// <summary>
    /// 컴포넌트 초기화
    /// </summary>
    private void Awake()
    {
        // 슬롯 배경 이미지 컴포넌트가 없으면 가져오기
        if (slotBackground == null)
            slotBackground = GetComponent<Image>();
        
        // 슬롯 아이콘 이미지 컴포넌트가 없으면 자식 오브젝트에서 찾기
        if (slotIcon == null)
        {
            var images = GetComponentsInChildren<Image>(includeInactive: true);
            foreach (var image in images)
            {
                if (image.gameObject == this.gameObject) continue;
                slotIcon = image;
                break;
            }
        }
        
        // 아이콘 UI 컴포넌트 가져오기
        if (slotIcon != null)
            Icon = slotIcon.GetComponent<InventoryItemUI>();
    }

    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="inGameInventoryUI">소유 인벤토리 UI</param>
    public void Init(int slotIndex, InGameInventoryUI inGameInventoryUI)
    {
        SlotIndex = slotIndex;
        owner = inGameInventoryUI;

        if (slotIcon != null)
        {
            //itemIcon.Setting();
        }
        
    }
    
    /// <summary>
    /// 슬롯 클릭 시 호출
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        // 선택된 슬롯의 인덱스 번호로 상위 인벤토리 UI의 슬롯 클릭 처리 호출
        owner?.OnSlotClicked(SlotIndex);
    }

    /// <summary>
    /// 마우스가 슬롯에 들어올 때 호출 
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 켜기 (Optional)
    }
    
    /// <summary>
    /// 마우스가 슬롯에서 나갈 때 호출
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 끄기 (Optional)
    }

    /// <summary>
    /// 아이템 드롭 시 호출되어 아이템 이동을 처리
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // 드래그된 아이템 UI 컴포넌트 가져오기
        var draggedItem = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<InventoryItemUI>() : null;
        if (draggedItem == null) return;

        // 이동할 출발지와 목적지 슬롯 인덱스 설정
        int from = draggedItem.SlotIndex;
        int to = SlotIndex;

        // 인벤토리에서 아이템 이동 처리
        owner.MoveItem(from, to);

        // 드래그된 아이템의 원래 부모와 새로운 부모 설정
        var sourceParent = draggedItem.original;
        var targetParent = this.transform;

        // 기존 슬롯에 있던 아이템 UI 처리
        var existingIcon = GetComponentInChildren<InventoryItemUI>(true);
        if (existingIcon != null && existingIcon != draggedItem && sourceParent != null)
        {
            existingIcon.transform.SetParent(sourceParent, false);
            existingIcon.transform.SetAsLastSibling();
            ResetRect(existingIcon.GetComponent<RectTransform>());
            existingIcon.Setup(from, owner, owner.baseCanvas);
        }
        
        // 드래그된 아이템 UI를 새로운 위치로 이동
        draggedItem.transform.SetParent(targetParent, false);
        draggedItem.transform.SetAsLastSibling();
        ResetRect(draggedItem.GetComponent<RectTransform>());
        draggedItem.Setup(to, owner, owner.baseCanvas);
    }

    /// <summary>
    /// 슬롯의 아이콘 이미지 설정
    /// </summary>
    /// <param name="icon">표시할 아이콘 스프라이트</param>
    public void SetIcon(Sprite icon)
    {
        if (slotIcon == null) return;

        slotIcon.sprite = icon;
        slotIcon.enabled = (icon != null);
    }

    /// <summary>
    /// 테스트 모드용 아이콘 표시 설정
    /// </summary>
    public void ShowIconForTestMode()
    {
        // 슬롯 배경 활성화
        if (slotBackground)
        {
            slotBackground.enabled = true;
            slotBackground.raycastTarget = true;
        }

        // 슬롯 아이콘 활성화
        if (slotIcon)
        {
            slotIcon.enabled = true;
            slotIcon.raycastTarget = true;
        }

        // 아이콘 UI 상호작용 활성화
        if (Icon)
        {
            var cg = Icon.GetComponent<CanvasGroup>();
            if (!cg) cg = Icon.gameObject.AddComponent<CanvasGroup>();
            cg.interactable = true;
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

    /// <summary>
    /// RectTransform 컴포넌트 초기화
    /// </summary>
    /// <param name="rt">초기화할 RectTransform</param>
    public void ResetRect(RectTransform rt)
    {
        if (!rt) return;
        
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100, 100);
        rt.localScale = Vector3.one;
    }
}