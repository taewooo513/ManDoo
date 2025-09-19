using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInventoryUI : UIBase
{
    public Button[] slotButtons; // 10개 버튼 Inspector에서 연결

    void Start()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int idx = i;
            slotButtons[i].onClick.AddListener(() => InventoryManager.Instance.UseItem(idx));
        }
    }
}
