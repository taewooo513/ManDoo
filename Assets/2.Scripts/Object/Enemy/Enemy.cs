using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System;

public class Enemy : BaseEntity
{
    private EnemyData data;
    private Skill[] skills;
    private float[] percentage;
    private float standardPercentage = 0.25f;
    private int mark = -1;
    private int _id;
    private List<BaseEntity> _playableCharacters = BattleManager.Instance.PlayableCharacters; //배틀 매니저의 플레이어 주소 참조
    
    public void Init(int id)
    {
        SetData(id);
        SetSkill();
        _id = id;
    }

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Enemy.GetEnemyData(id);
        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
    }

    private void SetSkill()
    {
        int i = 0;
        foreach (var id in data.skillId)
        {
            Skill skill = new Skill();
            skill.Init(id);
            skills[i] = skill;
            i++;
        }
    }
     
    private Skill GetRandomSkill()
    {
        List<Skill> possibleSkills = new List<Skill>();
        int enemyPosition = -1; // temp 값 => 나중에 BattleManager 에서 받아오기
        int playerPosition = -1; // temp 값 => 나중에 BattleManager 에서 받아오기

        // for (int i = 0; i < skills.Length; i++)
        // {
        //     int enablePosLength = skills[i].skillInfo.enablePos.Count;
        //     int targetPosLength = skills[i].skillInfo.targetPos.Count;
        //     for (int j = 0; j < enablePosLength; j++)
        //     {
        //         if (skills[i].skillInfo.enablePos[j] == enemyPosition)
        //         {
        //             for (int k = 0; k < targetPosLength; k++)
        //             {
        //                 if (skills[i].skillInfo.targetPos[j] == playerPosition)
        //                 {
        //                     possibleSkills.Add(skills[i]);
        //                     break;
        //                 }
        //             }
        //         }
        //     }
        // }

        foreach (var skill in skills)
        {
            bool atEnablePosition = skill.skillInfo.enablePos.Contains(enemyPosition);
            bool atTargetPosition = skill.skillInfo.targetPos.Contains(playerPosition);

            if (atEnablePosition && atTargetPosition)
                possibleSkills.Add(skill);
        }

        if (possibleSkills.Count == 0) return null;

        else
            return possibleSkills[UnityEngine.Random.Range(0, possibleSkills.Count)];
    }

    public override void Attack(BaseEntity baseEntity)
    {
        base.Attack(baseEntity);
    }

    private void AttackPercentage()
    {
        float total = 10;
        float rand = UnityEngine.Random.value * total;

        foreach (BaseEntity playableCharacters in _playableCharacters)
        {
            if(playableCharacters.StatusType == StatusType.Mark){}
        }

        //상태이상 효과에 따라 다른 가중치 부여
        // if (StatusType.Mark)
        // {
        // }
        
        //리스트 초기화
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