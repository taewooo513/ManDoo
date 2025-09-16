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
    private List<(int, int)> playerPosition;
    private List<(int, int)> enemyPosition;

    public void Init(int id)
    {
        SetData(id);
        SetSkill();
        _id = id; //현재 Enemy id
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
        skills = new Skill[data.skillId.Count];
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
        enemyPosition = BattleManager.Instance.GetEnemyPosition();

        foreach (var position in enemyPosition)
        {
            if (position.Item2 == _id)
                return position.Item1;
        }
        return -1;
    }

    private Skill GetRandomSkill()
    {
        var possibleSkills = new List<Skill>();
        int currentPosition = GetCurrentPosition();
        if (currentPosition == -1) return null;

        foreach (var skill in skills)
        {
            if (IsSingleTargetSkill(skill))
            {
                if (CanUseSkill(skill))
                    possibleSkills.Add(skill);
                //else swapPosition 요청
                    
            }
            else
            {
                var info = skill.skillInfo;
                bool atEnablePosition = info.enablePos.Contains(currentPosition);
                var possibleSkillRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos);

                if (possibleSkillRange.Count > 0)
                    possibleSkills.Add(skill);
            }
        }

        if (possibleSkills.Count == 0) return null;
        else
            return possibleSkills[UnityEngine.Random.Range(0, possibleSkills.Count)];
    }

    private bool CanUseSkill(Skill skill)
    {
        int currentPosition = GetCurrentPosition();
        var playerPosition = BattleManager.Instance.GetPlayerPosition();
        var info = skill.skillInfo;

        bool atEnablePosition = info.enablePos.Contains(currentPosition);
        bool atTargetPosition = playerPosition.Any(x => info.targetPos.Contains(x.Item1));

        if (atEnablePosition && atTargetPosition)
            return true;
        else
            return false;
    }

    private bool IsSingleTargetSkill(Skill skill)
    {
        if (skill.skillInfo.targetType == TargetType.Single)
            return true;
        else return false;
    }

    public override void Attack(BaseEntity baseEntity)
    {
        base.Attack(baseEntity);
    }
}