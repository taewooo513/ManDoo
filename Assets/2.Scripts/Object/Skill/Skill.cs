using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;
using System.Linq;

public class SkillInfo
{
    public string skillName;
    public TargetType targetType;
    public List<int> enablePos;
    public List<int> targetPos;
    public string iconPathString;
    public SkillEffect[] skillEffects;

    private SkillData sd;

    public SkillInfo(int id)
    {
        sd = DataManager.Instance.Skill.GetSkillData(id);

        this.skillName = sd.skillName;
        this.targetType = sd.targetType;
        this.enablePos = sd.enablePos;
        this.targetPos = sd.targetPos;
        this.iconPathString = sd.iconPathString;
        skillEffects = new SkillEffect[sd.effectId.Count];
        for (int i = 0; i < sd.effectId.Count; i++)
        {
            skillEffects[0].Init(sd.effectId[i]);
        }
    }
}

public class Skill
{
    public SkillInfo skillInfo { get; private set; }

    SkillEffect[] skillEffects;

    public void Init(int id)
    {
        skillInfo = new SkillInfo(id);
    }

    public void Setting()
    {

    }

    public void UseSkill(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        for (int i = 0; i < skillEffects.Length; i++)
        {
            skillEffects[i].ActiveEffect(actionEntity,targetEntity);
        }
    }
}