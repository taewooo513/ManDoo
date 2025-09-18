using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System.Linq;

public class Enemy : BaseEntity
{
    private EnemyData data;
    public void Start()
    {
        BattleManager.Instance.AddEnemyCharacter(this);
        Init(2001);
    }
    public void Init(int idx)
    {
        SetData(idx);
        SetSkill();
    }

    private void SetData(int idx)
    {
        this.id = idx;
        data = DataManager.Instance.Enemy.GetEnemyData(idx);
        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
    }

    private void SetSkill()
    {
        entityInfo.skills = new Skill[data.skillId.Count];
        int i = 0;
        foreach (var id in data.skillId)
        {
            Skill skill = new Skill();
            skill.Init(id, this);
            entityInfo.skills[i] = skill;
            i++;
        }
    }

    private Skill GetRandomSkill()
    {
<<<<<<< HEAD
        var skillCandidates = new List<Skill>();
        if (skills == null || skills.Length == 0) return null;

        var weights = new List<float>(); // 아군 가중치들
        BattleManager.Instance.GetLowHpSkillWeight(out float playerWeight, out float enemyWeight); // 만약 플레이어의 체력이 40프로 이하거나 아군 체력이 10프로 이하면 해당하는 entity 에 대한 가중치 증가.
        
        foreach (var skill in skills) // 현재 적군이 가지고 있는 스킬 순회
=======
        var possibleSkills = new List<Skill>();
        if (entityInfo.skills == null || entityInfo.skills.Length == 0) return null;
        var weights = new List<float>();
        BattleManager.Instance.GetLowHpSkillWeight(out float playerWeight, out float enemyWeight);

        foreach (var skill in entityInfo.skills)
>>>>>>> Develop
        {
            if (skill == null || skill.skillInfo == null) continue; 
            var info = skill.skillInfo;
            var desiredPosition = GetDesiredPosition(skill);
            skill.addedWeight = enemyWeight;
            float totalEnemyWeight = Skill.defaultWeight + skill.addedWeight;

            if (IsSingleTargetSkill(skill))
            {
                if (CanUseSkill(skill))
                {
                    // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
                    // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
                }

                else if (desiredPosition != -1)
                {
                    BattleManager.Instance.SwitchPosition(this, desiredPosition);
                    if (CanUseSkill(skill))
                    {
                        // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
                        // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
                    }
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
                    {
                        // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
                        // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
                    }
                }
            }
        }

        if (skillCandidates.Count == 0) return null;
        //else return possibleSkills[UnityEngine.Random.Range(0, possibleSkills.Count)]; // 나중에 삭제
        return RandomizeUtility.GetRandomSkillByWeight(skillCandidates);
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

    public override void Attack(float dmg, BaseEntity baseEntity)
    {
        base.Attack(dmg, baseEntity);
        var attackSkill = GetRandomSkill();
        var info = attackSkill.skillInfo;
        var targetRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>());
        int damage = entityInfo.attackDamage; // 여기서 스킬의 adRatio 곱하는걸로 기억하는데 어디로 간건지 물어보기.
        //int pickedIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(weights);
        if (IsSingleTargetSkill(attackSkill))
        {
            //int pickPlayer = 
        }
        // float targetIndex = BattleManager.Instance.GetLowHpSkillWeight()
            // var target = 
            // var targetIndicies = BattleManager.Instance.GetPossibleSkillRange(attackSkill.skillInfo.targetPos);
            // int damage = 
            //attackSkill.UseSkill();
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