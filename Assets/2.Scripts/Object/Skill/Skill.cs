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
            //Debug.Log(sd.effectId.Count);
            //skillEffects[i].Init(sd.effectId[i]);
        }
    }
}

public class Skill
{
    public SkillInfo skillInfo { get; private set; }
    public const float defaultWeight = 0.25f;
    SkillEffect[] skillEffects;
    BaseEntity baseEntity;
    public void Init(int id, BaseEntity entity)
    {
        skillInfo = new SkillInfo(id);
        baseEntity = entity;
    }

    public void Setting()
    {

    }

    public void UseSkill(BaseEntity targetEntity)
    {
        for (int i = 0; i < skillEffects.Length; i++)
        {
            skillEffects[i].ActiveEffect(baseEntity, targetEntity);
        }
    }
}