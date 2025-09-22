using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IDropHandler
{
    public int SlotIndex { get; private set; }
    private InGameInventoryUI owner;

    public void Init(int slotIndex, InGameInventoryUI inGameInventoryUI)
    {
        SlotIndex = slotIndex;
        owner = inGameInventoryUI;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        owner.OnSlotClicked(SlotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 켜기 (Optional)
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: 슬롯에 하이라이트 끄기 (Optional)
    }

    public void OnDrop(PointerEventData eventData)
    {
        // TODO: 드래그 된 아이템 처리. 뭔가 인벤토리 매니저가 필요할거 같음.
    }
}
