using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpbarUI : MonoBehaviour
{
    private EntityInfo entityInfo;
    [SerializeField]
    private Image hpBar;

    private void Start()
    {
        entityInfo = GetComponentInParent<BaseEntity>().entityInfo;
    }
    public void UpdateUI()
    {
        if (hpBar != null || entityInfo != null)
        {
            hpBar.fillAmount = 1f / entityInfo.maxHp * entityInfo.currentHp;
        }
    }
}
