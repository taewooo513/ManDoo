using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotActiveTrap : MonoBehaviour
{
    public GameObject outline;

    public void Start()
    {
        outline = gameObject.GetComponent<GameObject>();
    }
}
