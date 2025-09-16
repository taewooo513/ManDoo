using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System;
using System.Linq;

public class Enemy : BaseEntity
{
    private EnemyData data;
    private Skill[] skills;
    private float[] percentage;
    private float standardPercentage = 0.25f;
    private int mark = -1;
    private int _id;
    private List<BaseEntity> _playableCharacters = BattleManager.Instance.PlayableCharacters; //배틀 매니저의 플레이어 주소 참조
    private List<BaseEntity> _enemyCharacters = BattleManager.Instance.EnemyCharacters;
    private List<(int,int)> playerPosition = BattleManager.Instance.GetPlayerPosition();
    private List<(int,int)> enemyPosition = BattleManager.Instance.GetEnemyPosition();
    
    public void Init(int id)
    {
        SetData(id);
        SetSkill();
    }

    private void SetData(int id)
    {
        this._id = id;
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

    private int GetCurrentPosition()
    {
        // enemyPosition 에서 받아온 item2 가 _id 와 일치하는 인덱스를 찾는다.
        // 그 후, 그 인덱스의 item1 을 리턴해서 현재 스킬을 사용하는 enemy 의 위치값을 넘겨준다.
        foreach (var position in enemyPosition)
        {
            if (position.Item2 == _id)
                return position.Item1;
        }
        return -1;
    }

    private Skill GetRandomSkill()
    {
        List<Skill> possibleSkills = new List<Skill>();

        foreach (var skill in skills)
        {
            bool atEnablePosition = skill.skillInfo.enablePos.Contains(GetCurrentPosition());
            bool atTargetPosition = playerPosition.Any(x => skill.skillInfo.targetPos.Contains(x.Item1));

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
            //if(playableCharacters.StatusType == StatusType.Mark){}
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