using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 인벤토리의 각 슬롯 UI를 관리하는 클래스
/// </summary>
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IDropHandler, IDroppingTarget
{

    /// <summary>
    /// 현재 슬롯의 인덱스 번호
    /// </summary>
    public int SlotIndex { get; private set; }

    /// <summary>
    /// 슬롯에 표시되는 아이템 아이콘 UI
    /// </summary>
    public InventoryItemUI Icon { get; private set; }

    /// <summary>
    /// 이 슬롯이 속한 인벤토리 UI 인스턴스
    /// </summary>
    private InGameInventoryUI owner;
    
    /// <summary>
    /// 슬롯의 배경 이미지 컴포넌트
    /// </summary>
    [SerializeField] private Image slotBackground;

    /// <summary>
    /// 슬롯에 표시되는 아이콘 이미지 컴포넌트
    /// </summary>
    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI countText;
    
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
        if (countText == null)
            countText = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    /// <summary>
    /// 슬롯 초기화
    /// </summary>
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
    /// 슬롯 클릭 이벤트 처리
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        owner?.OnSlotClicked(SlotIndex);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 켜기 (Optional)
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 끄기 (Optional)
    }

    /// <summary>
    /// 아이템 드롭 이벤트 처리
    /// </summary>
    // public void OnDrop(PointerEventData eventData)
    // {
    //     // 드래그된 아이템 UI 컴포넌트 가져오기
    //     var draggedItem = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<InventoryItemUI>() : null;
    //     if (draggedItem == null) return;
    //     
    //     // 이동할 출발지와 목적지 슬롯 인덱스 설정
    //     int from = draggedItem.SlotIndex;
    //     int to = SlotIndex;
    //
    //     if (owner != null && owner.isTestMode)
    //     {
    //         var targetContainer = this.transform;
    //         var sourceContainer = draggedItem.original;
    //         
    //         var existingIcon = targetContainer.GetComponentInChildren<InventoryItemUI>(true);
    //         if (existingIcon != null && existingIcon != draggedItem && sourceContainer != null)
    //         {
    //             existingIcon.transform.SetParent(sourceContainer, false);
    //             ResetRectTransform(existingIcon.GetComponent<RectTransform>());
    //             existingIcon.Setup(from, owner, owner.baseCanvas);
    //         }
    //         
    //         draggedItem.transform.SetParent(targetContainer, false);
    //         ResetRectTransform(draggedItem.GetComponent<RectTransform>());
    //         draggedItem.Setup(to, owner, owner.baseCanvas);
    //         
    //         return;
    //     }
    //     
    //     // 인벤토리에서 아이템 이동 처리
    //     owner.MoveItem(from, to);
    // }

    public void OnDrop(PointerEventData eventData)
    {
        var draggingObject = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<IDraggingObject>() : null;
        if (draggingObject == null) return;
        Drop(draggingObject);
    }
    public bool CanDrop(IDraggingObject draggingObject)
    {
        if (draggingObject == null) return false;

        if (draggingObject.Origin == DragOrigin.Inventory)
        {
            if (draggingObject.SlotIndex == SlotIndex) return false;
            return true;
        }

        if (draggingObject.Origin == DragOrigin.Reward) return true;

        if (draggingObject.Origin == DragOrigin.Equipment)
            return InventoryManager.Instance.GetItemInSlot(SlotIndex) == null;
        
        return false;
    }

    public bool Drop(IDraggingObject draggingObject)
    {
        if (!CanDrop(draggingObject)) return false;

        var im = InventoryManager.Instance;

        if (draggingObject.Origin == DragOrigin.Inventory)
        {
            owner.MoveItem(draggingObject.SlotIndex, SlotIndex);
            return true;
        }

        if (draggingObject.Origin == DragOrigin.Reward)
        {
            if (im.TryAddItem(draggingObject.ItemType, draggingObject.ItemId, 1))
            {
                if (draggingObject is IRewardItem ri)
                    ri.Obtain(1);
                owner.RefreshSlots();
                return true;
            }
            return false;
        }

        if (draggingObject.Origin == DragOrigin.Equipment)
        {
            bool can = im.TryUnEquipToInventory(EquipmentSlotType.Weapon, SlotIndex);
            owner.RefreshSlots();
            return can;
        }
        return false;
    }

    public Transform RootObject => this.transform;

    /// <summary>
    /// 현재 슬롯의 아이콘 스프라이트 반환
    /// </summary>
    public Sprite GetIconSprite() => slotIcon ? slotIcon.sprite : null;
    
    /// <summary>
    /// 슬롯의 아이콘 이미지 설정
    /// </summary>
    public void SetIcon(Sprite icon)
    {
        if (slotIcon == null) return;
        slotIcon.sprite = icon;
        var hasIcon = (icon != null);
        slotIcon.enabled = hasIcon;
    }

    public void UpdateSlotCountText(int count) => countText.text = count.ToString();
    
    /// <summary>
    /// 슬롯 아이콘의 상호작용 가능 여부 설정
    /// </summary>
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
    /// RectTransform 컴포넌트를 기본값으로 초기화
    /// </summary>
    public void ResetRectTransform(RectTransform rt)
    {
        if (!rt) return;
        
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100, 100);
        rt.localScale = Vector3.one;
    }
}