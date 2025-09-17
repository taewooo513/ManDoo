using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyButton : SelectEntityButton
{
    Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }

    public override void OnClickButton()
    {
        UIManager.Instance.OpenUI<InGameEnemyUI>().UpdateUI(enemy.entityInfo);
    }
}
