using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UI 인벤토리 아이템의 드래그 앤 드롭 동작을 처리하는 클래스
/// </summary>
public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 인벤토리 슬롯의 인덱스
    /// </summary>
    public int SlotIndex { get; private set; }
    
    private Canvas baseCanvas;              // UI가 속한 캔버스
    private InGameInventoryUI inGameInventoryUI;  // 인벤토리 UI 참조
    private RectTransform rect;             // 아이템의 RectTransform
    private RectTransform canvasRect;       // 캔버스의 RectTransform  
    private Vector2 dragOffset;             // 드래그시 마우스와 아이템간의 오프셋
    private CanvasGroup canvasGroup;        // UI 투명도와 레이캐스트 제어용
    private Transform original;             // 드래그 전 원래 부모 Transform
    
    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (baseCanvas != null)
            canvasRect = baseCanvas.GetComponent<RectTransform>();
        if (inGameInventoryUI == null)
            inGameInventoryUI = GetComponentInParent<InGameInventoryUI>();
    }

    // public void Setup(int slotIndex, InGameInventoryUI owner, Canvas canvas)
    // {
    //     SlotIndex = slotIndex;
    //     inGameInventoryUI = owner;
    //     
    // }
    
    /// <summary>
    /// 드래그 시작시 호출
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 필수 컴포넌트가 없으면 드래그 처리하지 않음
        if (baseCanvas == null || canvasRect == null || rect == null) return;
        
        // 드래그 시작 시 현재 부모 Transform 저장
        original = transform.parent;
        
        // 캔버스를 새로운 부모로 설정하고 최상위 자식으로 설정
        transform.SetParent(baseCanvas.transform, false);
        transform.SetAsLastSibling();
        
        // 마우스 위치와 아이템 위치 사이의 오프셋 계산
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out var local))
            dragOffset = rect.anchoredPosition - local;

        // 캔버스 그룹이 있다면 드래그 중 설정 적용
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false; // 드래그 중에 슬롯들이 레이캐스트 받게끔 설정
            canvasGroup.alpha = 0.5f; // 투명도 설정
        }
    }

    /// <summary>
    /// 드래그 중일 때 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치를 캔버스의 로컬 좌표로 변환하여 아이템의 위치를 업데이트
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out var local))
            rect.anchoredPosition = local + dragOffset;
    }
    
    /// <summary>
    /// 드래그가 끝났을 때 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 아이템이 캔버스에 직접 속해있고 원래 부모가 있다면 원래 위치로 되돌림
        if (transform.parent == baseCanvas.transform && original != null)
            transform.SetParent(original, false);
        
        // 인벤토리 UI가 있다면 슬롯들을 새로고침
        if (inGameInventoryUI != null)
            inGameInventoryUI.RefreshSlots();

        // 캔버스 그룹이 있다면 드래그 종료 시 설정 복원
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true; // 레이캐스트 차단 해제
            canvasGroup.alpha = 1f;            // 투명도 원래대로
        }
    }
    
    /// <summary>
    /// 아이템을 클릭했을 때 호출
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO: 
    }
}