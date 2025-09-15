using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private MercenaryData data;
    private SkillInfo[] skills;

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);
        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );

        skills = new SkillInfo[data.skillId.Count];
        for (int i = 0; i < data.skillId.Count; i++)
        {
            var skillData = DataManager.Instance.Skill.GetSkillData(data.skillId[i]);
            skills[i] = new SkillInfo(skillData.skillName, skillData.effectType, skillData.adRatio,
                skillData.constantValue,
                skillData.duration, skillData.targetType, skillData.enablePos, skillData.targetPos,
                skillData.iconPathString);
        }
    }

    public void Init()
    {
    }

    public override void Attack(int index)
    {
        base.Attack(index);
    }
}