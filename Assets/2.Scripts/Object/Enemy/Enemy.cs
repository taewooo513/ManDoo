using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System.Linq;

public class Enemy : BaseEntity
{
    private EnemyData data;
    private Skill[] skills;

    public void Init(int id)
    {
        SetData(id);
        SetSkill();
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

    private Skill GetRandomSkill()
    {
        var possibleSkills = new List<Skill>();
        if (skills == null || skills.Length == 0) return null;

        foreach (var skill in skills)
        {
            if (skill == null || skill.skillInfo == null) continue;
            var info = skill.skillInfo;
            var desiredPosition = GetDesiredPosition(skill);

            if (IsSingleTargetSkill(skill))
            {
                if (CanUseSkill(skill))
                    possibleSkills.Add(skill);
                else if (desiredPosition != -1)
                {
                    BattleManager.Instance.SwitchPosition(this, desiredPosition);
                    if (CanUseSkill(skill))
                        possibleSkills.Add(skill);
                }
            }

            else
            {
                bool atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);

                if (!atEnablePosition && desiredPosition != -1)
                {
                    BattleManager.Instance.SwitchPosition(this, desiredPosition);
                    atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);
                }

                if (atEnablePosition)
                {
                    var possibleSkillRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>());
                    if (possibleSkillRange != null && possibleSkillRange.Count > 0)
                        possibleSkills.Add(skill);
                }
            }
        }

        if (possibleSkills.Count == 0) return null;
        else return possibleSkills[UnityEngine.Random.Range(0, possibleSkills.Count)];
    }

    private bool CanUseSkill(Skill skill)
    {
        if (skill == null || skill.skillInfo == null) return false;
        var info = skill.skillInfo;
        var playerPosition = BattleManager.Instance.GetPlayerPosition();
        bool atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);
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
        var attackSkill = GetRandomSkill();
        // int targetIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(); -> Player 에서 가중치 리스트 받아와서 매개변수로 넣기
        // var target = 
        var targetIndicies = BattleManager.Instance.GetPossibleSkillRange(attackSkill.skillInfo.targetPos);
        // int damage = 
        // attackSkill.UseSkill(BaseEntity target);
        //
        // if (IsSingleTargetSkill(attackSkill))
        //     BattleManager.Instance.AttackEntity(targetIndex, damage);
        // else
        //     BattleManager.Instance.AttackEntity(targetIndicies, damage);

        
    }

    private int GetDesiredPosition(Skill skill)
    {
        if (skill == null || skill.skillInfo == null) return -1;
        var info = skill.skillInfo;
        if (info.enablePos == null || info.enablePos.Count == 0) return -1;

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