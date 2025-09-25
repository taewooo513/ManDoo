using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureChestRoom : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.OpenUI<InGameTreasureChestUI>();
    }

    private void OnClickTreasureChest()
    {
        Debug.Log("보물상자 클릭됨");   
    }
}
