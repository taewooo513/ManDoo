using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameVictoryUI : UIBase
{
    [SerializeField]
    private float closeTimer;
    protected override void OnOpen()
    {
        base.OnOpen();
        StartCoroutine(ClosePanel());
    }

    private IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(closeTimer);
        base.OnClose();
    }
}
