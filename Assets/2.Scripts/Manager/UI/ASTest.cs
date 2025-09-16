using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASTest : MonoBehaviour
{
    
    void Start()
    {
        UIManager.Instance.OpenUI<UIStatusPanel>();
        UIManager.Instance.OpenUI<MonsterInfo>();

    }

    
}
