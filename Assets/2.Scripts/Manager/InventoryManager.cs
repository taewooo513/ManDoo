using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour // 없어도 될거같음. item manager 를 만들고 그 안에 UseItem() 생성. 
{
    public static InventoryManager Instance;

    public List<InventorySlot> slots = new List<InventorySlot>(10);

    void Awake()
    {
        Instance = this;
        // 초기화: 10개 슬롯 생성
        for (int i = 0; i < 10; i++)
            slots.Add(new InventorySlot());
    }

    public void UseItem(int slotIndex)
    {
        var slot = slots[slotIndex];
        if (!slot.IsEmpty)
        {
            //Debug.Log($"아이템 사용: {slot.item.itemName}");
            // 아이템 효과 발동 로직 여기에 추가
            slot.item = null; // 사용 후 비우기
        }
    }
}
