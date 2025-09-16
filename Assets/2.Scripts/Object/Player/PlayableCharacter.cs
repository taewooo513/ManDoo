using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private MercenaryData data;
    public SkillInfo[] skills { get; private set; }

    protected void Awake()
    {
        DataManager.Instance.Initialize();
        SetData(1001);
    }

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );

        for (int i = 0; i < data.skillId.Count; i++)
        {
            var skillData = DataManager.Instance.Skill.GetSkillData(data.skillId[i]);
            skills[i] = new SkillInfo(skillData.skillName,skillData.effectType,skillData.adRatio
                ,skillData.constantValue,skillData.duration,skillData.targetType,skillData.enablePos,
                skillData.targetPos,skillData.iconPathString);

        }
    }

    public override void Attack(BaseEntity baseEntity)
    {
        base.Attack(baseEntity);
        // BattleManager.Instance.AttackEnemy(baseEntity);
    }

    public override void UseSkill(BaseEntity baseEntity)
    {
        base.UseSkill(baseEntity);
    }
}