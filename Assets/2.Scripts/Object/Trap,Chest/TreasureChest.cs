using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public GameObject chestRewardUI; //상자 보상 UI
    public int id;

    public void Start()
    {
        chestRewardUI.SetActive(false);
    }

    public void OpenChestRewardUI() //상자 눌러서 오픈 시
    {
        chestRewardUI.SetActive(true);
        //TreasureRoom.IsOpen(id, true); //todo : 여기에서 id 값 필요할지, 현재로썬 연결 방법이 없는데 어떻게 할지. 추후 연결 필요함.
    }
}
