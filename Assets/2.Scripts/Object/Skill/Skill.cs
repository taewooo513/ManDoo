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
        this.enablePos = new List<int>();
        for (int i = 0; i < sd.enablePos.Count; i++)
        {
            this.enablePos.Add(sd.enablePos[i] - 1);
        }
        this.targetPos = new List<int>();
        for (int i = 0; i < sd.targetPos.Count; i++)
        {
            this.targetPos.Add(sd.targetPos[i] - 1);
        }
        this.iconPathString = sd.iconPathString;
        skillEffects = new SkillEffect[sd.effectId.Count];
        for (int i = 0; i < sd.effectId.Count; i++)
        {
            var datas = DataManager.Instance.Effect.GetEffectData(sd.effectId[i]);
            switch (datas.effectType)
            {
                case EffectType.Attack:
                    skillEffects[i] = new DamageSkill();
                    skillEffects[i].Init(datas);
                    break;
            }
        }
    }
}

public class Skill
{
    public SkillInfo skillInfo { get; private set; }
    public const float defaultWeight = 0.25f;
    public float addedWeight;
    BaseEntity baseEntity;
    public void Init(int id, BaseEntity entity)
    {
        skillInfo = new SkillInfo(id);
        baseEntity = entity;
        Setting();
    }

    private void Setting()
    {

    }

    public void UseSkill(BaseEntity targetEntity)
    {
        var val = BattleManager.Instance.GetPossibleSkillRange(skillInfo.targetPos);
        if (skillInfo.targetType == TargetType.Range)
        {
            for (int j = 0; j < val.Count; j++)
            {
                for (int i = 0; i < skillInfo.skillEffects.Length; i++)
                {
                    skillInfo.skillEffects[i].ActiveEffect(baseEntity, BattleManager.Instance._enemyCharacters[val[j]]);
                }
            }
        }
        else
        {
            for (int i = 0; i < skillInfo.skillEffects.Length; i++)
            {
                skillInfo.skillEffects[i].ActiveEffect(baseEntity, targetEntity);
            }
        }
    }

    public EffectType GetSkillType()
    {
        EffectType skillType;

        for (int i = 0; i < skillInfo.skillEffects.Length; i++)
        {

        }
        return skillInfo.skillEffects[0].GetEffectType();
    }
}