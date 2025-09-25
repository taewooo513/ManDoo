using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChestRoom : MonoBehaviour
{
    [SerializeField] private Button treasureChestButton;

    private void Start()
    {
        UIManager.Instance.OpenUI<InGameTreasureChestUI>();
        treasureChestButton.onClick.AddListener(OnClickTreasureChest);
    }

    private void OnClickTreasureChest()
    {
        Debug.Log("보물상자 클릭됨");
        UIManager.Instance.OpenUI<InGameVictoryUI>();
    }
}
