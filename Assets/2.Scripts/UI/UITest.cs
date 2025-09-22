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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UIManager.Instance.OpenUI<PMCUI>();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIManager.Instance.CloseUI<PMCUI>();
        }
    }
}


