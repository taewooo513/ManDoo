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
    public InventoryItemUI itemIcon;
    
    /// <summary>
    /// 아이콘을 표시하는 Image 컴포넌트
    /// </summary>
    private Image image;

    /// <summary>
    /// 컴포넌트 초기화
    /// </summary>
    private void Awake()
    {
        if (itemIcon != null)
            image = itemIcon.GetComponent<Image>();
        else
            image = GetComponentInChildren<Image>();
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

        if (itemIcon != null)
        {
            //itemIcon.Setting();
        }
        
    }
    
    /// <summary>
    /// 슬롯 클릭 시 호출
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 선택된 슬롯의 인덱스 번호로 상위 인벤토리 UI의 슬롯 클릭 처리 호출
        owner.OnSlotClicked(SlotIndex);
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
    /// 아이템 드롭 시 호출
    /// </summary>
    public void OnDrop(PointerEventData eventData)
    {
        // TODO: 드래그 된 아이템 처리. 뭔가 인벤토리 매니저가 필요할거 같음.
    }

    /// <summary>
    /// 슬롯의 아이콘 이미지 설정
    /// </summary>
    /// <param name="icon">표시할 아이콘 스프라이트</param>
    public void SetIcon(Sprite icon)
    {
        if (image == null) return;
        if (icon == null)
            image.enabled = false;
        else
        {
            image.enabled = true;
            image.sprite = icon;
        }
    }
}