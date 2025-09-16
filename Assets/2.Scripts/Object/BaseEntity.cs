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
    }

    public void Damaged(int value)
    {
        currentHp -= value;
        if (currentHp <= 0)
        {
            currentHp = 0;
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

    public virtual void Attack(BaseEntity baseEntity)
    {
    }

    public virtual void UseSkill(BaseEntity baseEntity)
    {

    }
}