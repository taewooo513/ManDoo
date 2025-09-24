using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRemoveButton : MonoBehaviour
{
    public int spawnIndex;
    public Button button;

    void Awake()
    {
        button.onClick.AddListener(() => {
            if (PMCHire.Instance != null)
                PMCHire.Instance.RemovePlayerAt(spawnIndex);
        });
    }
}