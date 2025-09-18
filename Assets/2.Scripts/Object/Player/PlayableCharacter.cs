using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private MercenaryData data;
    protected void Awake()
    {

    }
    public void Start()
    {
        BattleManager.Instance.AddPlayableCharacter(this);
        SetData(1001);
    }

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
        entityInfo.SetUpSkill(data.skillId, this);
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
        base.StartTurn();
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills);
    }
    public override void EndTurn(bool hasExtraTurn = true)
    {

    }

}