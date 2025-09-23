using DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PMCInfoUI : BaseEntity
{
    private MercenaryData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleTypeText;
    public TextMeshProUGUI contractGoldText;

    [SerializeField] private int initID; // 용병 id

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

        if (nameText != null)
            nameText.text = data.name;
        if (roleTypeText != null)
            roleTypeText.text = data.roleType.ToString();
        if (contractGoldText != null)
            contractGoldText.text = data.contractGold.ToString();
    }

    public void OnClickHire()
    {
        int emptyIndex = InGamePMCUI.Instance.FindEmptySpawnIndex();
        if (emptyIndex == -1)
        {
            Debug.Log("빈 스폰 위치가 없습니다. PMC 소환 불가.");
            return;
        }
        InGamePMCUI.Instance.SpawnPMC(emptyIndex, initID);
    }
}

