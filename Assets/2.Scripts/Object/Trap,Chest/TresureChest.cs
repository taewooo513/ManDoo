using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
    public GameObject chestRewardUI; //상자 보상 UI

    public void Start()
    {
        chestRewardUI.SetActive(false);
    }

    public void OpenChestRewardUI()
    {
        chestRewardUI.SetActive(true);
    }
}
