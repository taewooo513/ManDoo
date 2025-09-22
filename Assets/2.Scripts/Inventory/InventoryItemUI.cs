using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas baseCanvas;
    private InGameInventoryUI inGameInventoryUI;
    private RectTransform rect;
    private RectTransform canvasRect;
    private Vector2 dragOffset;
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData) 
    {
        transform.SetParent(baseCanvas.transform, false);
        transform.SetAsLastSibling();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out var local))
            dragOffset = rect.anchoredPosition - local;
        canvasGroup.blocksRaycasts = false; // 드래그 중에 슬롯들이 레이캐스트 받게끔 설정
        canvasGroup.alpha = 0.5f; // 투명도 설정
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out var local))
            rect.anchoredPosition = local + dragOffset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        inGameInventoryUI.RefreshSlots();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
