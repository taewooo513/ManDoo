using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System.Linq;

public class Enemy : BaseEntity
{
    private EnemyData data;
    private Skill[] skills;
    private bool hasExtraTurn = true;
    private bool usedSkill = false;

    public void Start()
    {
        BattleManager.Instance.AddEnemyCharacter(this);
        Init(2001);
    }
    public void Init(int idx)
    {
        SetData(idx);
        //SetSkill(); //스킬 자체를 셋업하는 부분이 엔티티인포에 들어있어서 사용x
    }

    private void SetData(int idx)
    {
        this.id = idx;
        data = DataManager.Instance.Enemy.GetEnemyData(idx);
        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
        entityInfo.SetUpSkill(data.skillId, this);
    }

    // private void SetSkill()
    // {
    //     skills = new Skill[data.skillId.Count];
    //     int i = 0;
    //     foreach (var id in data.skillId)
    //     {
    //         Skill skill = new Skill();
    //         skill.Init(id, this);
    //         entityInfo.skills[i] = skill;
    //         i++;
    //     }
    // }

    private Skill GetRandomSkill()
    {
        var skillCandidates = new List<Skill>();
        var weights = new List<float>();
        float weight = Skill.defaultWeight;
        if (entityInfo.skills == null || entityInfo.skills.Length == 0) return null;
        BattleManager.Instance.GetLowHpSkillWeight(out float playerWeight, out float enemyWeight);


        for (int i = 0; i < entityInfo.skills.Length; i++)
        {
            var skill = entityInfo.skills[i];
            if (skill == null || skill.skillInfo == null) continue;

            var info = skill.skillInfo;
            if (info.enablePos == null || info.targetPos == null) continue;

            bool isAttackSkill = false;
            bool isSupportSkill = false;
            var effectArray = info.skillEffects;

            for (int j = 0; j < effectArray.Length; j++)
            {
                var effect = effectArray[j];
                var effectType = effect.GetEffectType();
                if (effectType == EffectType.Attack || effectType == EffectType.Debuff)
                    isAttackSkill = true;
                if (effectType == EffectType.Heal || effectType == EffectType.Protect)
                    isSupportSkill = true;
            }

            if (isAttackSkill)
                skill.addedWeight = playerWeight + weight;
            if (isSupportSkill)
                skill.addedWeight = enemyWeight + weight;

            skillCandidates.Add(skill);
        }

        if (skillCandidates.Count == 0) return null;
        return RandomizeUtility.GetRandomSkillByWeight(skillCandidates);
    }

    private bool CanUseSkill(Skill skill) //스킬이 사용 가능한 위치 enemy가 서있는지, 스킬 범위 내에 플레이어가 서 있는지
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

    public override void StartTurn()
    {
        //1순위 - 스킬 사용 //만약에 skill 그게 null이다
        //if(skill == null) hasExtraTurn = false;
        //2순위 - 이동 //스킬을 사용할 수 있는 곳으로 이동한다.
        //3순위 - 턴 넘기기 //혹시 이동도 못해, 그러면 턴 넘기기

        //다시 여기로 돌아옴
        //EndTurn(hasExtraTurn);
        if (usedSkill)
        {
            
        }
    }

    public override void EndTurn(bool hasExtraTurn = true)
    {
        //지금 엔티티에 걸린 상태이상을 적용하고, 턴 수를 감소시킨다.
        BattleManager.Instance.EndTurn();
    }

    public override void StartExtraTurn()
    {
        //1순위 - 스킬 사용
        //2순위 - 이동
        //3순위 - 턴 넘기기
    }

    // private bool IsSingleTargetSkill(Skill skill)
    // {
    //     if (skill.skillInfo.targetType == TargetType.Single)
    //         return true;
    //     else return false;
    // }

    public override void Attack(float dmg, BaseEntity baseEntity) //적->플레이어 공격
    {
        base.Attack(dmg, baseEntity);
        var attackSkill = GetRandomSkill(); //사용할 스킬 랜덤 선택
        var info = attackSkill.skillInfo; //사용할 스킬 정보
        List<int> targetRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>()); //타겟 가능한 범위 가져오기
        List<float> weights = BattleManager.Instance.GetWeightList(true); //타겟 가중치 리스트 가져옴
        //List<float> weights = GenerateWeightListUtility.GetWeights(); //타겟 가중치 리스트 가져옴
        int pickedIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(weights); //가중치 기반으로 랜덤하게 플레이어 인덱스를 선택

        if (CanUseSkill(attackSkill))
        {
            if (targetRange.Contains(pickedIndex)) //선택한 인덱스(때리려는 적)가 타겟 가능한 위치에 있는지 체크
            {
                attackSkill.UseSkill(BattleManager.Instance.PlayableCharacters[pickedIndex]); //스킬 사용
                EndTurn();
            }
            else //없으면 위치 바꾸기
            {
                int position = GetDesiredPosition(attackSkill); //현재 enemy가 서 있는 위치
                BattleManager.Instance.SwitchPosition(this, position);
                EndTurn();
            }
        }
    }

    private int GetDesiredPosition(Skill skill) //현재 엔티티 위치 읽는 함수
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

/*
private Skill GetRandomSkill()
{
    // bool[] isSupportSkill = new bool[entityInfo.skills.Length];
    // bool[] isAttackSkill = new bool[entityInfo.skills.Length];

    // for (int i = 0; i < entityInfo.skills.Length; i++)
    // {
    //     for (int j = 0; j < entityInfo.skills[i].skillInfo.skillEffects.Length; j++)
    //     {
    //         var effectType = entityInfo.skills[i].skillInfo.skillEffects[j].GetEffectType();
    //         if (effectType == EffectType.Attack || effectType == EffectType.Debuff)
    //             isAttackSkill[i] = true;
    //         else if (effectType == EffectType.Heal || effectType == EffectType.Protect)
    //             isSupportSkill[i] = true;
    //         else continue;
    //     }
    // }
    // foreach (var skill in entityInfo.skills)
    //     {

    //         if (skill == null || skill.skillInfo == null) continue;
    //         var info = skill.skillInfo;
    //         var desiredPosition = GetDesiredPosition(skill);

    //         //if ()
    //         skill.addedWeight = enemyWeight;
    //         float supportSkillWeight = Skill.defaultWeight + skill.addedWeight;

    //         if (IsSingleTargetSkill(skill))
    //         {
    //             if (CanUseSkill(skill))
    //             {
    //                 // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
    //                 // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
    //             }

    //             else if (desiredPosition != -1)
    //             {
    //                 BattleManager.Instance.SwitchPosition(this, desiredPosition);
    //                 if (CanUseSkill(skill))
    //                 {
    //                     // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
    //                     // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
    //                 }
    //             }
    //         }

    //         else
    //         {
    //             bool atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);

    //             if (!atEnablePosition && desiredPosition != -1)
    //             {
    //                 BattleManager.Instance.SwitchPosition(this, desiredPosition);
    //                 atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);
    //             }

    //             if (atEnablePosition)
    //             {
    //                 var possibleSkillRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>());
    //                 if (possibleSkillRange != null && possibleSkillRange.Count > 0)
    //                 {
    //                     // TODO: Player 중에 체력이 40프로 이하인 플레이어가 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다
    //                     // TODO: Enemy 중에 체력이 10프로 이하인 적이 존재 한다면 skill 의 가중치를 + 0.3 하고 possibleSkills.Add(skill); 한다.
    //                 }
    //             }
    //         }
    //     }


    // //else return possibleSkills[UnityEngine.Random.Range(0, possibleSkills.Count)]; // 나중에 삭제
    }
*/