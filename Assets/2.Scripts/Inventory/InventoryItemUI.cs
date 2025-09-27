using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI 인벤토리 아이템의 드래그 앤 드롭 동작을 처리하는 클래스
/// </summary>
public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggingObject
{
    /// <summary>
    /// 인벤토리 슬롯의 인덱스
    /// </summary>
    public int SlotIndex { get; private set; }
    public Transform original { get; private set; } // 드래그 전 원래 부모 Transform
    private Canvas baseCanvas;              // UI가 속한 캔버스
    private InGameInventoryUI owner;  // 인벤토리 UI 참조
    private RectTransform rect;             // 아이템의 RectTransform
    private RectTransform canvasRect;       // 캔버스의 RectTransform  
    //private Vector2 dragOffset;             // 드래그시 마우스와 아이템간의 오프셋
    private CanvasGroup cg;        // UI 투명도와 레이캐스트 제어용
    
    // IDraggingObject
    private eItemType itemType;
    private int itemId;
    private int amount;
    private DragOrigin origin = DragOrigin.Inventory;
    public  eItemType ItemType => itemType;
    public int ItemId => itemId;
    public int Amount => amount;
    public DragOrigin Origin => origin;

    private List<RaycastResult> hits = new();
    
    /// <summary>
    /// 컴포넌트 초기화 시 필요한 참조들을 가져옴
    /// </summary>
    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        
        if (baseCanvas) 
            canvasRect = baseCanvas.GetComponent<RectTransform>();
        if (owner == null) 
            owner = GetComponentInParent<InGameInventoryUI>();
    }

    /// <summary>
    /// 아이템 UI의 초기 설정을 수행
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="owner">소속된 인벤토리 UI</param>
    /// <param name="canvas">UI가 속한 캔버스</param>
    public void Setup(int slotIndex, InGameInventoryUI ui, Canvas canvas)
    {
        SlotIndex = slotIndex;
        owner = ui;
        baseCanvas = canvas != null ? canvas : baseCanvas;
        if (baseCanvas)
            canvasRect = baseCanvas.transform as RectTransform;
    }
    
    /// <summary>
    /// 드래그 시작시 호출
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        var image = GetComponent<UnityEngine.UI.Image>();
        if (image == null || image.sprite == null) return;

        var im = InventoryManager.Instance;
        var item = im.GetItemInSlot(SlotIndex);
        amount = im.GetSlotCount(SlotIndex);
        origin = DragOrigin.Inventory;
        
        // 드래그 시작 시 현재 부모 Transform 저장
        original = transform.parent;
        transform.SetParent(baseCanvas.transform, true);
        transform.SetAsLastSibling();
        
        if (cg)
        {
            cg.blocksRaycasts = false; // 드래그 중에 슬롯들이 레이캐스트 받게끔 설정
            cg.alpha = 0.5f; // 투명도 설정
        }
        UpdatePosition(eventData);
    }

    /// <summary>
    /// 드래그 중일 때 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData) => UpdatePosition(eventData);
    
    /// <summary>
    /// 드래그가 끝났을 때 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        IDroppingTarget dropTarget = FindDroppingTarget(eventData);
        
        if (dropTarget != null && dropTarget.Drop(this)) Debug.Log("드랍 완료");
        else
        {
            if (Origin == DragOrigin.Inventory)
            {
                InventoryManager.Instance.RemoveItemFromSlot(SlotIndex, 1);
                owner?.RefreshSlots();
            }

            if (transform.parent == baseCanvas.transform && original != null)
            {
                transform.SetParent(original, false);
                var rt = GetComponent<RectTransform>();
                if (rt)
                {
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;
                    rt.sizeDelta = new Vector2(100, 100);
                    rt.localScale = Vector3.one;
                }
            }
        }
        // 아이템이 캔버스에 직접 속해있고 원래 부모가 있다면 원래 위치로 되돌림
        // if (transform.parent == baseCanvas.transform && original != null)
        // {
        //     transform.SetParent(original, false);
        //     var rt = GetComponent<RectTransform>();
        //     if (rt)
        //     {
        //         rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        //         rt.anchoredPosition = Vector2.zero;
        //         rt.sizeDelta = new Vector2(100, 100);
        //         rt.localScale = Vector3.one;
        //     }
        // }

        if (cg)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

    /// <summary>
    /// 드래그 중인 아이템의 위치를 업데이트
    /// </summary>
    /// <param name="eventData">포인터 이벤트 데이터</param>
    private void UpdatePosition(PointerEventData eventData)
    {
        if (!canvasRect || !rect) return;
        var camera = eventData.pressEventCamera != null ? eventData.pressEventCamera : baseCanvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, camera,
                out var local))
            rect.anchoredPosition = local; // + dragOffset;
    }

    private IDroppingTarget FindDroppingTarget(PointerEventData eventData)
    {
        hits.Clear();
        var gr = baseCanvas
            ? baseCanvas.GetComponentInParent<GraphicRaycaster>()
            : GetComponentInParent<GraphicRaycaster>();
        if (gr == null || EventSystem.current == null) return null;
        
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (var hit in hits)
        {
            var target = hit.gameObject.GetComponentInParent<IDroppingTarget>();
            if (target != null) return target;
        }
        return null;
    }
}