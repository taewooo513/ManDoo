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

}
