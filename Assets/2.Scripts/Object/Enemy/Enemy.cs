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

    private void SetSkill() // monobehaviour 는 new 를 사용할 수 없어서 Skill.cs 변경 요청.
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
            var desiredPosition = GetDesiredPosition(skill);

            if (IsSingleTargetSkill(skill))
            {
                if (CanUseSkill(skill))
                    possibleSkills.Add(skill);
                else
                {
                    if (desiredPosition != -1)
                    {
                        BattleManager.Instance.SwitchPosition(this, desiredPosition);
                        // 아래 코드는 만약 적이 스킬 사용 가능 구역으로 이동 후 바로 스킬을 사용 할 수 있는지 판단 후 작성.
                        // if (CanUseSkill(skill))
                        //     possibleSkills.Add(skill);
                    }
                }

            }
            else
            {
                var info = skill.skillInfo;
                bool atEnablePosition = info.enablePos.Contains(currentPosition);

                if (!atEnablePosition)
                {
                    if (desiredPosition != -1)
                    {
                        BattleManager.Instance.SwitchPosition(this, desiredPosition);
                        currentPosition = GetCurrentPosition();
                        atEnablePosition = info.enablePos.Contains(currentPosition);
                    }
                }
                
                else
                {
                    var possibleSkillRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos);
                    if (possibleSkillRange.Count > 0)
                        possibleSkills.Add(skill);
                }

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

    private int GetDesiredPosition(Skill skill)
    {
        var info = skill.skillInfo;
        if (info.enablePos.Count == 0) return -1;
        var currentIndex = BattleManager.Instance.FindEntityPosition(this) ?? -1;
        if (currentIndex < 0) return -1;

        var entities = BattleManager.Instance.EnemyCharacters;
        int entityCount = entities?.Count ?? 0;
        if (entityCount == 0) return -1;

        foreach (var position in info.enablePos)
        {
            int targetIndex = position;

            if (targetIndex >= 0 && targetIndex < entityCount)
            {
                if (targetIndex != currentIndex)
                    return targetIndex;
            }
            return targetIndex;
        }
        return -1;
    }
}