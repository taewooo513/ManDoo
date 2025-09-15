using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    private MercenaryData data;
    private Skill[] skills;

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);
        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
    }

    public void Init()
    {
    }

    public override void Attack(int index)
    {
        base.Attack(index);
    }
}