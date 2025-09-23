using DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PMCInfo : BaseEntity
{
    private MercenaryData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleTypeText;
    public TextMeshProUGUI contractGoldText;

    [SerializeField] private int initID;

    public void Start()
    {
        SetData(initID);
    }

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );

        // UI 표시 추가!
        if (nameText != null)
            nameText.text = data.name;
        if (roleTypeText != null)
            roleTypeText.text = data.roleType.ToString();
        if (contractGoldText != null)
            contractGoldText.text = data.contractGold.ToString();


    }
}
