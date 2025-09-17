using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class SkillInfo
{
    public string skillName;
    public EffectType effectType;
    public float adRatio;
    public int constantValue;
    public int duration;
    public TargetType targetType;
    public List<int> enablePos;
    public List<int> targetPos;
    public string iconPathString;

    public SkillInfo(string skillName, EffectType effectType, float adRatio
        , int constantValue, int duration, TargetType targetType, List<int> enablePos, List<int> targetPos,
        string iconPathString)
    {
        this.skillName = skillName;
        this.effectType = effectType;
        this.adRatio = adRatio;
        this.constantValue = constantValue;
        this.duration = duration;
        this.targetType = targetType;
        this.enablePos = enablePos;
        this.targetPos = targetPos;
        this.iconPathString = iconPathString;
    }
}

public class Skill
{
    private SkillData sd;
    public SkillInfo skillInfo { get; private set; }

    public void Init(int id)
    {
        sd = DataManager.Instance.Skill.GetSkillData(id);
        skillInfo = new SkillInfo(
            sd.skillName, sd.effectType, sd.adRatio, sd.constantValue,
            sd.duration, sd.targetType, sd.enablePos, sd.targetPos, sd.iconPathString);
    }

    public void UseSkill()
    {
        
    }
}