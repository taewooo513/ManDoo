using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGamePlayerUI : UIBase
{
    public bool isTestMode = true;
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI jobText;

    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI currentHpUI;

    public GameObject[] skillUIObjects;
    
    [SerializeField] private EquipmentSlotUI[] equipmentSlots;
    
    public void UpdateUI(EntityInfo entityInfo, Skill[] skills)
    {
        nameText.text = entityInfo.name;
        hpUI.text = entityInfo.maxHp.ToString();
        currentHpUI.text = entityInfo.currentHp.ToString();

        for(int i = 0; i < skills.Length; i++)
        {
            Debug.Log("fds");
            skillUIObjects[i].GetComponent<SelectSkillButton>().SetButton(skills[i]);
        }
    }

    public void RefeshSlots()
    {
        if (equipmentSlots == null) return;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            // TODO: Icon refresh 로직 추가.
        }
    }
}