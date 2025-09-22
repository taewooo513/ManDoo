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
    public TextMeshProUGUI Money;

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
        entityInfo.SetUpSkill(data.skillId, this);

        // UI 표시 추가!
        if (nameText != null)
            nameText.text = data.name;
        if (roleTypeText != null)
            roleTypeText.text = data.roleType.ToString();
        //if (Weapon != null)
        //    Weapon.text = data.weaponLv; // MercenaryData에 무기레벨이 생기면
    }
}
