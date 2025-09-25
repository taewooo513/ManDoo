using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 인벤토리의 각 슬롯 UI를 관리하는 클래스
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IDropHandler
{

    public int SlotIndex { get; private set; }
    public InventoryItemUI Icon { get; private set; }
    private InGameInventoryUI owner;
    [SerializeField] private Image slotBackground;
    [SerializeField] private Image slotIcon;
    
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

        if (!owner.isTestMode)
        {
            SetIcon(null);
            SetInteractableIcon(false);
        }
    }
    

    /// <summary>
    /// 슬롯 클릭 시 호출
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
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

        if (owner != null && owner.isTestMode)
        {
            var targetContainer = this.transform;
            var sourceContainer = draggedItem.original;
            
            var existingIcon = targetContainer.GetComponentInChildren<InventoryItemUI>(true);
            if (existingIcon != null && existingIcon != draggedItem && sourceContainer != null)
            {
                existingIcon.transform.SetParent(sourceContainer, false);
                ResetRectTransform(existingIcon.GetComponent<RectTransform>());
                existingIcon.Setup(from, owner, owner.baseCanvas);
            }
            
            draggedItem.transform.SetParent(targetContainer, false);
            ResetRectTransform(draggedItem.GetComponent<RectTransform>());
            draggedItem.Setup(to, owner, owner.baseCanvas);
            
            return;
        }
        
        // 인벤토리에서 아이템 이동 처리
        owner.MoveItem(from, to);
    }

    /// <summary>
    /// 슬롯의 아이콘 이미지 설정
    /// </summary>
    /// <param name="icon">표시할 아이콘 스프라이트</param>
    
    public Sprite GetIconSprite() => slotIcon ? slotIcon.sprite : null;
    
    public void SetIcon(Sprite icon)
    {
        if (slotIcon == null) return;
        slotIcon.sprite = icon;
        var hasIcon = (icon != null);
        slotIcon.enabled = hasIcon;
    }

    public void SetInteractableIcon(bool interactable)
    {
        if (slotIcon == null) return;
        slotIcon.raycastTarget = interactable;
        if (Icon)
        {
            var cg = Icon.GetComponent<CanvasGroup>();
            if (!cg) cg = Icon.gameObject.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = interactable;
            cg.interactable = interactable;
            cg.alpha = interactable ? 1f : 0f;
        }
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
    public void ResetRectTransform(RectTransform rt)
    {
        if (!rt) return;
        
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100, 100);
        rt.localScale = Vector3.one;
    }
}