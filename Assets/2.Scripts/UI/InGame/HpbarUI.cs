using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpbarUI : MonoBehaviour
{
    private EntityInfo entityInfo;
    private Image hpBar;
    private void Awake()
    {
        entityInfo = GetComponentInParent<BaseEntity>().entityInfo;
        hpBar = GetComponentInChildren<Image>();
    }

    public void UpdateUI()
    {
        if (hpBar != null || entityInfo != null)
        {
            hpBar.fillAmount = 1f / entityInfo.maxHp * entityInfo.currentHp;
        }
    }
}
