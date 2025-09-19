using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityInfo
{
    public string name;
    public int currentHp;
    public int maxHp;
    public int attackDamage;
    public int defense;
    public int speed;
    public bool isDie;
    public float evasion;
    public float critical;
    public StatEffect statEffect;
    public float hpWeight = 0f;
    public float addWeight = 0.3f;
    public Skill[] skills;
    private readonly float _standardWeight = 0.25f;

    public EntityInfo(string name, int maxHp, int attackDamage, int defense, int speed, float evasion, float critical)
    {
        this.name = name;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.attackDamage = attackDamage;
        this.defense = defense;
        this.speed = speed;
        this.evasion = evasion;
        this.critical = critical;
        statEffect = new StatEffect();
    }

    public void Damaged(float value)
    {
        var hp = currentHp - value;
        currentHp = (int)hp;
        if (currentHp <= 0)
        {
            currentHp = 0;
        }
    }

    public float GetPlayableTargetWeight() //플레이어블 캐릭터의 타깃 가중치 합
    {
        float result = _standardWeight + statEffect.AttackWeight(); //가중치 합
        GenerateWeightListUtility.CombineWeights(result); //가중치를 리스트에 추가 //TODO : 턴 끝날 때 GenerateWeightListUtility.Clear(); 호출해줘야 됨
        return result;
    }

    public float GetEnemyTargetWeight() //enemy의 타깃 가중치 합
    {
        float result = _standardWeight;
        GenerateWeightListUtility.CombineWeights(result);
        return result;
    }

    //TODO : 스킬 확률 관리 부분에서, 스킬 효과에 따른 증감 가중치 주려면 구조 변경해야됨. curHP를 넘겨서 따로 작업?
    public bool LowHPStatEnemy() //적(플레이어블) hp가 낮을 때.
    {
        double percentage = maxHp * 0.4;
        if (currentHp <= percentage) //현재 hp가 40% 이하일 때
        {
            return true;
        }
        return false;
    }

    public bool LowHPStatPlayer() //아군(enemy) hp가 낮을 때.
    {
        double percentage = maxHp * 0.1;
        if (currentHp <= percentage) //현재 hp가 10% 이일 때
        {
            return true;
        }
        return false;
    }

    public void SetUpSkill(List<int> skillIdList, BaseEntity nowEntity)
    {
        skills = new Skill[skillIdList.Count];
        for (int i = 0; i < skillIdList.Count; i++)
        {
            skills[i] = new Skill();
            skills[i].Init(skillIdList[i], nowEntity);
        }
    }
}

public class BaseEntity : MonoBehaviour
{
    [SerializeField] public EntityInfo entityInfo;

    public EntityInfo GetEntityInfo
    {
        get { return entityInfo; }
    }
    private HpbarUI hpbarUI;
    public int id { get; protected set; }
    protected bool hasExtraTurn = true;
    public Action<BaseEntity> OnDied;

    protected virtual void Awake()
    {
        SetData();
        hpbarUI = GetComponentInChildren<HpbarUI>();
    }

    public virtual void Release()
    {
        OnDied -= BattleManager.Instance.EntityDead;
    }

    public virtual void SetData()
    {
    }

    public virtual void Damaged(float value)
    {
        entityInfo.Damaged(value);
        hpbarUI.UpdateUI();
        if (entityInfo.currentHp <= 0)
        {
            OnDied?.Invoke(this);
        }
    }

    public void BattleStarted()
    {
        OnDied += BattleManager.Instance.EntityDead;
    }

    public void BattleEnded()
    {
        OnDied -= BattleManager.Instance.EntityDead;
    }

    public virtual void Attack(float dmg, BaseEntity baseEntity)
    {

    }

    public virtual void Support(Skill skill, BaseEntity baseEntity)
    {

    }
    public virtual void UseSkill(BaseEntity baseEntity)
    {

    }
    public virtual void StartTurn()
    {

    }
    public virtual void EndTurn(bool hasExtraTurn = true)
    {

    }
    public virtual void StartExtraTurn()
    {

    }
}