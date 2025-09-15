using UnityEngine;
using System;
using System.Collections.Generic;
using DataTable;

public class Enemy : BaseEntity
{
    List<Skill> skillList = new List<Skill>();
    private float percentage;
    private float standardPercentage = 0.25f;
    private int mark = -1;
    private int _id;
    private EnemyData ed;

    public void Init(int id)
    {
        _id = id;
        ed = DataManager.Instance.Enemy.GetEnemyData(_id);
        SetSkill();
    }

    private void SetSkill()
    {
        foreach (var id in ed.skillId)
        {
            Skill skill = new Skill();
            skill.Init(id);
            skillList.Add(skill);
        }
    }

    public Skill GetRandomSkill()
    {
        int rand = UnityEngine.Random.Range(1, 4);
        return skillList[rand];
    }

    public override void Attack(int index)
    {
        
    }

    public void OnClickSelectEnemy()
    {

    }

    public void AttackPercentage()
    {

    }

    public void Mark()
    {

    }

    public void Buff()
    {
        
    }

    public void Guard()
    {

    }

    public void PlayerReact()
    {

    }
}

/*

    public bool GetDroppedItem(int dropTable, out GameObject droppedItems)
    {
        float total = 0f;
        foreach (var drop in DropItemTables[dropTable].DropItemDatas)
        {
            total += drop.Percent;
        }

        float random = UnityEngine.Random.value * total;
        int id = 0;
        foreach (var drop in DropItemTables[dropTable].DropItemDatas)
        {
            random -= drop.Percent;
            if (random <= 0f)
            {
                id = drop.ID;
                break;
            }
        }

        if (id == 0)
        {
            droppedItems = null;
            return false;
        }

        droppedItems = ItemDatas[id].Prefab;
        return true;
    }

*/