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
    public float hpWeight = 0.3f;
    public float standardWeight = 0.25f;

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

    public void Damaged(int value)
    {
        currentHp -= value;
        if (currentHp <= 0)
        {
            currentHp = 0;
        }
    }

    public float GetTotalWeight() //개개인이 가지고 있는 가중치 합
    {
        float result = standardWeight + statEffect.AttackWeight(); //가중치 합
        GenerateWeightListUtility.CombineWeights(result); //TODO : 턴 부분에서 GenerateWeightListUtility.Clear(); 호출해줘야 됨
        return result;
    }

    public void LowHPStatEnemy() //적 hp가 낮을 때. TODO : 스킬 공격 시작할 때마다 호출하면서 검증해줘야 됨
    {
        double percentage = maxHp * 0.1 * 4;
        if (currentHp <= percentage) //현재 hp가 40% 이하일 때
        {
            standardWeight += hpWeight;
        }
        if (currentHp > percentage) //hp가 40% 초과일 때
        {
            standardWeight = 0.25f; //원래대로 복구
        }
    }

    public void LowHPStatPlayer() //아군 hp가 낮을 때.
    {
        double percentage = maxHp * 0.1;
        if (currentHp <= percentage) //현재 hp가 10% 이일 때
        {
            standardWeight += hpWeight;
        }
        if (currentHp > percentage) //hp가 10% 초과일 때
        {
            standardWeight = 0.25f;
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
    //private HpbarUI hpbarUI;
    public int id { get; protected set; }

    protected virtual void Awake()
    {
        SetData();
        //hpbarUI = GetComponentInParent<HpbarUI>();
    }

    public virtual void SetData()
    {
    }

    public virtual void Damaged(int value)
    {
        entityInfo.Damaged(value);
        //hpbarUI.UpdateUI();
    }

    public virtual void Attack(int dmg, BaseEntity baseEntity)
    {
    }

    public virtual void UseSkill(BaseEntity baseEntity)
    {

    }
    public virtual void StartTurn()
    {

    }
    public virtual void EndTurn()
    {

    }
}