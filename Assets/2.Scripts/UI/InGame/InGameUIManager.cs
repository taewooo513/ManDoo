using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : UIBase
{
    private Action<Skill> buttonSkillActiveAction;

    public bool isSkillSelected = false;
    public Skill selectedSkill = null;

    public void OnClickSkillButton(Skill skill)
    {
        buttonSkillActiveAction?.Invoke(skill); // null 체크
        isSkillSelected = true;
        selectedSkill = skill;
    }

    public void AddSkillButtonAction(Action<Skill> action)
    {
        buttonSkillActiveAction += action;
    }

    public void RemoveSkillButtonAction(Action<Skill> action)
    {
        buttonSkillActiveAction -= action;
    }

    public void OpenInventoryUI()
    {
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
    }

    public void CloseInventoryUI()
    {
        UIManager.Instance.OpenUI<InGameEnemyUI>();
        UIManager.Instance.CloseUI<InGameInventoryUI>();
    }

    public void DeselectSkill()
    {
        isSkillSelected = false;
        selectedSkill = null;
    }
}
        
