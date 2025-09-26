using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{

    void Awake()
    {
        DataManager.Instance.Initialize();
        UIManager.Instance.OpenUI<InGameUIManager>();
        UIManager.Instance.OpenUI<InGamePlayerUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.OpenUI<UIInputHandler>();
        UIManager.Instance.OpenUI<InGameVictoryUI>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
            UIManager.Instance.OpenUI<InGameVictoryUI>();
        if(Input.GetKeyDown(KeyCode.H))
            UIManager.Instance.CloseUI<InGameVictoryUI>();
    }
}
