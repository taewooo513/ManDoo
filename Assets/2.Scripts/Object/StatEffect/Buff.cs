﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class TotalBuffStat
{
    public float damagedValue = 0; // 입은피해량
    public int speed = 0;
    public float defense = 0;
    public float attackDmg = 0; //공격데미지
    public float evasionUp = 0;
    public float totalWeight = 0;
    public float critical = 0;
    public void Reset(EntityInfo entityInfo)
    {
        damagedValue = 0;
        var equipment = InventoryManager.Instance.GetEquippedWeapon();

        if (equipment != null && entityInfo != null)
        {
            speed = entityInfo.speed + equipment.speed;
            defense = entityInfo.defense + equipment.defense;
            attackDmg = entityInfo.attackDamage + equipment.attackDmg;
            evasionUp = entityInfo.evasion + equipment.evasion;
            critical = entityInfo.critical + equipment.critical;
        }

        else
        {
            speed = entityInfo.speed;
            defense = entityInfo.defense;
            attackDmg = entityInfo.attackDamage;
            evasionUp = entityInfo.evasion;
        }
        
        // 고치기 전 코드 일단 남겨 놨습니다, 혹시 모르니깐.
        // if (entityInfo.equipWeapon != null)
        // {
        //     speed = entityInfo.speed + entityInfo.equipWeapon.speed;
        //     defense = entityInfo.defense + entityInfo.equipWeapon.defense;
        //     attackDmg = entityInfo.attackDamage + entityInfo.equipWeapon.attackDmg;
        //     evasionUp = entityInfo.evasion + entityInfo.equipWeapon.evasion;
        // }
        // else
        // {
        //     speed = entityInfo.speed;
        //     defense = entityInfo.defense;
        //     attackDmg = entityInfo.attackDamage;
        //     evasionUp = entityInfo.evasion;
        // }
        totalWeight = 0;
    }
}
public class Buff
{
    public List<BuffInfo> _entityCurrentStatus = new();
    public SetTotalBuffStat _setTotalEffectStat;
    private float _totalWeight = 0f;
    public TotalBuffStat totalStat;

    public bool IsStun()
    {
        foreach (var item in _entityCurrentStatus)
        {
            if (item.deBuffType == DeBuffType.Stun)
            {
                Debug.Log("1");
                return true;
            }
        }
        return false;
    }
    public Buff()
    {
        totalStat = new TotalBuffStat();
    }
    public float AttackWeight(EntityInfo entityInfo) // 버프 가중치 계산
    { //스킬 쓸 때마다 호출
        _totalWeight = 0;
        totalStat.Reset(entityInfo);
        for (int i = 0; i < _entityCurrentStatus.Count; i++)
        {
            Debug.Log(totalStat.attackDmg.ToString() + " : " + _entityCurrentStatus[i].adRatio.ToString());
            var buff = _entityCurrentStatus[i].buffType;
            switch (buff)
            {
                //공격력 × (1 - 감소비율) hp -= (((dmg + a )- (def + a)) * 계수 +  고정) * 2
                case BuffType.AttackUp:
                    totalStat.attackDmg = (int)((totalStat.attackDmg * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    break;
                //(스탯 × 계수) + 고정수치 (각 항목별 동일 방식 적용)
                case BuffType.AllStatUp:
                    totalStat.attackDmg = (int)((totalStat.attackDmg * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    totalStat.speed += _entityCurrentStatus[i].constantValue;
                    totalStat.evasionUp = (int)((totalStat.evasionUp * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    break;
                //(치명타율% × 계수) + 고정수치 (소수점은 버림)
                case BuffType.CriticalUp:
                    totalStat.critical = (int)((totalStat.critical * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    break;
                //(회피율 % × 계수) +고정수치(소수점은 버림)
                case BuffType.EvasionUp:
                    totalStat.evasionUp = (int)((totalStat.evasionUp * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    break;
                //방어력 × (1 - 감소비율)
                case BuffType.DefenseUp:
                    totalStat.defense = (int)((totalStat.defense * _entityCurrentStatus[i].adRatio + _entityCurrentStatus[i].constantValue));
                    break;
                case BuffType.SpeedUp:
                    totalStat.speed += _entityCurrentStatus[i].constantValue;
                    break;
            }

            var deBuff = _entityCurrentStatus[i].deBuffType;
            switch (deBuff)
            {
                //공격력 × (1 - 감소비율)
                case DeBuffType.AttackDown:
                    totalStat.attackDmg = (int)(totalStat.attackDmg * (1f - _entityCurrentStatus[i].adRatio));
                    break;
                //각 스탯 × 계수→ 각 스탯 × (1 - 감소비율)
                case DeBuffType.AllStatDown:
                    totalStat.defense = (int)(totalStat.defense * (1f - _entityCurrentStatus[i].adRatio));
                    totalStat.evasionUp = (int)(totalStat.evasionUp * (1f - _entityCurrentStatus[i].adRatio));
                    totalStat.critical = (int)(totalStat.critical * (1f - _entityCurrentStatus[i].adRatio));
                    totalStat.attackDmg = (int)(totalStat.attackDmg * (1f - _entityCurrentStatus[i].adRatio));
                    break;
                //치명타율 × (1 - 감소비율)→ 소수점 이하 버림
                case DeBuffType.CriticalDown:
                    totalStat.critical = (int)(totalStat.critical * (1f - _entityCurrentStatus[i].adRatio));
                    break;
                //회피율 × (1 - 감소비율)→ 소수점 이하 버림
                case DeBuffType.EvasionDown:
                    totalStat.evasionUp = (int)(totalStat.evasionUp * (1f - _entityCurrentStatus[i].adRatio));
                    break;
                //방어력 × (1 - 감소비율)
                case DeBuffType.DefenseDown:
                    totalStat.defense = (int)(totalStat.defense * (1f - _entityCurrentStatus[i].adRatio));
                    break;
                //스피드 - 감소 수치
                case DeBuffType.SpeedDown:
                    totalStat.speed -= _entityCurrentStatus[i].constantValue;
                    break;
                case DeBuffType.Damaged:
                    totalStat.damagedValue = _entityCurrentStatus[i].constantValue;
                    break;
            }
        }
        return _totalWeight;
    }

    public void ReduceTurn(List<BuffType> buffTypes, List<DeBuffType> deBuffTypes) // 리스트에 담은 타입 삭제
    {
        foreach (var item in buffTypes)
        {
            for (int i = 0; i < _entityCurrentStatus.Count; i++)
            {
                if (_entityCurrentStatus[i].buffType == item)
                {
                    _entityCurrentStatus[i].duration--;
                    if (_entityCurrentStatus[i].duration <= 0)
                    {
                        _entityCurrentStatus.Remove(_entityCurrentStatus[i]);
                        break;
                    }
                }
            }
        }
        foreach (var item in deBuffTypes)
        {
            for (int i = 0; i < _entityCurrentStatus.Count; i++)
            {
                if (_entityCurrentStatus[i].deBuffType == item)
                {
                    _entityCurrentStatus[i].duration--;
                    if (_entityCurrentStatus[i].duration <= 0)
                    {
                        _entityCurrentStatus.Remove(_entityCurrentStatus[i]);
                    }
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public void AddStatus(BuffInfo status) //상태이상 추가
    {
        _entityCurrentStatus.Add(status); //상태 추가
        _entityCurrentStatus.Sort((e, e1) =>
        {
            if (e.buffType == BuffType.None && e1.buffType != BuffType.None)
            {
                return 1;
            }
            if (e.buffType != BuffType.None && e1.buffType == BuffType.None)
            {
                return -1;
            }
            if (e.buffType == BuffType.None && e1.buffType == BuffType.None)
            {
                return e.duration.CompareTo(e1.duration);
            }
            if (e.deBuffType == DeBuffType.None && e1.deBuffType == DeBuffType.None)
            {
                return e.duration.CompareTo(e1.duration);
            }
            return 0;
        });

    }

    public void RemoveStatus(BuffInfo status) //상태이상 제거
    {
        _entityCurrentStatus.Remove(status);
    }

    public void Clear() //리스트 내부의 배열 삭제(상태이상 초기화)
    {
        _entityCurrentStatus.Clear();
    }

}