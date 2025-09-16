using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.Initialize();

        DataManager.Instance.Test();
    }


}
