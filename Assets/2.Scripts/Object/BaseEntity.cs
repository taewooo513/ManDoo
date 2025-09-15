using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected EntityInfo entityInfo;
    protected int id;

    protected void Awake()
    {
        SetData();
    }

    public virtual void SetData()
    {
    }

    public virtual void Damaged(int value)
    {
        entityInfo.Damaged(value);
    }

    public virtual void Attack(int index)
    {
    }
}