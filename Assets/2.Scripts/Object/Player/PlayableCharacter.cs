using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private MercenaryData data;
    public Skill[] skills { get; private set; }

    protected void Awake()
    {
        DataManager.Instance.Initialize();
        SetData(1001);
    }
    public void Start()
    {
        BattleManager.Instance.AddPlayableCharacter(this);
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
            skills[i] = new Skill();
            var skillData = DataManager.Instance.Skill.GetSkillData(data.skillId[i]);
        }
    }

    public override void Attack(float dmg, BaseEntity baseEntity)
    {
        base.Attack(dmg, baseEntity);
        // BattleManager.Instance.AttackEnemy(baseEntity);
    }

    public override void UseSkill(BaseEntity baseEntity)
    {
        base.UseSkill(baseEntity);
    }
    public override void StartTurn()
    {
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, skills);
    }
    public override void EndTurn()
    {

    }

}