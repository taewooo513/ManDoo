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

    [SerializeField] private int playerIndex;     // <- 추가!
    [SerializeField] private int spawnIndex;   // <- 추가!
    [SerializeField] private int initID;       // 기존 변수 사용

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

    public void OnClickHire()
    {
        int emptyIndex = InGamePMCUI.Instance.FindEmptySpawnIndex();
        if (emptyIndex == -1)
        {
            Debug.Log("빈 스폰 위치가 없습니다. PMC 소환 불가.");
            return;
        }
        InGamePMCUI.Instance.SpawnPMC(playerIndex, emptyIndex, initID);
    }
}

