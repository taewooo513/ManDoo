using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : UIBase
{
    private Action<Skill> buttonSkillActiveAction; //

    public void OnClickSkillButton(Skill skill)
    {
        buttonSkillActiveAction.Invoke(skill);
    }
    public void AddSkillButtonAction(Action<Skill> action)
    {
        buttonSkillActiveAction += action;
    }
    public void RemoveSkillButtonAction(Action<Skill> action)
    {
        buttonSkillActiveAction -= action;
    }
}
