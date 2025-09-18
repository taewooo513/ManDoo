using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System.Linq;
using UnityEditorInternal;

public class Enemy : BaseEntity
{
    private EnemyData _data;
    private Skill[] _skills;
    private bool _hasExtraTurn = true;
    private Skill _attackSkill;

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
        _data = DataManager.Instance.Enemy.GetEnemyData(idx);
        entityInfo = new EntityInfo(
            _data.name, _data.health, _data.attack, _data.defense, _data.speed, _data.evasion, _data.critical
        );
        entityInfo.SetUpSkill(_data.skillId, this);
    }
    
    public override void StartTurn()
    {
        if (TryAttack(out int position)) //공격 시도 성공시
        {
            _hasExtraTurn = true;
        }
        else //공격 실패 시
        {
            _hasExtraTurn = false; //추가 공격도 실패
            if (position != -1)
            { 
                BattleManager.Instance.SwitchPosition(this, position); //이동
            }
        }
        
        EndTurn(_hasExtraTurn);
    }

    public override void EndTurn(bool hasExtraTurn = true)
    {
        //TODO : 지금 엔티티에 걸린 상태이상을 적용하고, 턴 수를 감소시킨다?
        _attackSkill = null; //선택한 스킬 비워주기
        BattleManager.Instance.EndTurn(hasExtraTurn);
    }

    public override void StartExtraTurn() //추가 공격 턴
    {
        if (TryAttack(out int position)) //공격 시도 성공시
        {
        }
        else //공격 실패 시
        {
            if (position != -1)
            { 
                BattleManager.Instance.SwitchPosition(this, position); //이동
            }
        }
        EndTurn(false);
    }

    private Skill GetRandomSkill()
    {
        var skillCandidates = new List<Skill>();
        var weights = new List<float>();
        float weight = Skill.defaultWeight;
        if (entityInfo.skills == null || entityInfo.skills.Length == 0) return null;
        BattleManager.Instance.GetLowHpSkillWeight(out float playerWeight, out float enemyWeight);
        //TODO : 범위 공격 - 스킬 랜덤으로 뽑아주는 부분에서, 랜덤으로뽑힌스킬.UseSkill 하면 된다고 함.

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

    private bool TryAttack(out int position) //스킬 선택, 타겟 선택
    {
        _attackSkill = GetRandomSkill(); //사용할 스킬 랜덤 선택
        if (_attackSkill == null) //스킬이 없을 때
        {
            position = -1; //사용 안 하겠다는 뜻. 이동도 못 함.
            return false;
        }
        var info = _attackSkill.skillInfo; //사용할 스킬 정보
        List<int> targetRange = BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>()); //타겟 가능한 범위 가져오기
        List<float> weights = BattleManager.Instance.GetWeightList(true); //타겟 가중치 리스트 가져옴
        int pickedIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(weights); //가중치 기반으로 랜덤하게 플레이어 인덱스를 선택

        var targetEntity = BattleManager.Instance.PlayableCharacters[pickedIndex]; //타겟
        
        if (CanUseSkill(_attackSkill))
        {
            if (targetRange.Contains(pickedIndex)) //선택한 인덱스(때리려는 적)가 타겟 가능한 위치에 있는지 체크
            {
                UseSkill(targetEntity); //기존 : Attack(dmg, targetEntity); //스킬 작동 흐름 : tryAttack -> UseSkill -> Attack 순서
                position = -1;
                return true;
            }
        }
        position = GetDesiredPosition(_attackSkill); //현재 enemy가 서 있는 위치
        return false;
    }

    public override void Attack(float dmg, BaseEntity targetEntity) //적->플레이어 공격
    {
        int index = Utillity.GetIndexInListToObject(BattleManager.Instance.PlayableCharacters, targetEntity); //이렇게 하면 attack - tryattack 연결하는 부분이 없음. 어떻게 연결?
        BattleManager.Instance.AttackEntity(index, (int)dmg); //TODO : 범위공격/단일공격 처리 안 되어있음. 스킬에서 해주는 건지?
    }

    public override void Support(float amount, BaseEntity baseEntity)
    {

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

    
    // private bool IsSingleTargetSkill(Skill skill)
    // {
    //     if (skill.skillInfo.targetType == TargetType.Single)
    //         return true;
    //     else return false;
    // }
*/