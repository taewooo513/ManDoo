using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TotalBuffStat
{
    public float damagedValue = 0; // 입은피해량
    public int speed = 0;
    public float defense = 0;
    public float attackDmg = 0; //공격데미지
    public float evasionUp;
    public float totalWeight = 0;

    public void Reset()
    {
        damagedValue = 0;
        speed = 0;
        defense = 0;
        attackDmg = 0;
        evasionUp = 0;
        totalWeight = 0;
    }
}
public class Buff
{
    private List<BuffInfo> _entityCurrentStatus = new();
    private SetTotalBuffStat _setTotalEffectStat;
    private float _totalWeight = 0f;
    public TotalBuffStat totalStat;

    public Buff()
    {
        totalStat = new TotalBuffStat();
    }
    public float AttackWeight() //가중치 계산/부여
    { //스킬 쓸 때마다 호출
        _totalWeight = 0;
        totalStat.Reset();
        for (int i = 0; i < _entityCurrentStatus.Count; i++)
        {
            var buff = _entityCurrentStatus[i].buffType;
            switch (buff)
            {
                case BuffType.AttackUp:
                    break;
                case BuffType.AllStatUp:
                    break;
                case BuffType.CriticalUp:
                    break;
                case BuffType.EvasionUp:
                    break;
                case BuffType.SpeedUp:
                    break;
            }

            var deBuff = _entityCurrentStatus[i].deBuffType;
            switch (deBuff)
            {
                case DeBuffType.AttackDown:
                    break;
                case DeBuffType.DefenseDown:
                    break;
                case DeBuffType.SpeedDown:
                    break;
                case DeBuffType.EvasionDown:
                    break;
                case DeBuffType.CriticalDown:
                    break;
                case DeBuffType.AllStatDown:
                    break;
            }
        }
        return _totalWeight;
    }


    public void ReduceTurn(List<BuffType> buffTypes, List<DeBuffType> deBuffTypes)
    {
        foreach (var item in buffTypes)
        {
            for (int i = 0; i < _entityCurrentStatus.Count;)
            {
                if (_entityCurrentStatus[i].buffType == item)
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