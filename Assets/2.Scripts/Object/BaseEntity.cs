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

    public virtual void Damaged(int value)
    {
        entityInfo.Damaged(value);
    }

    public virtual void Attack(int index)
    {
    }
}