using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatEffect
{
    private List<StatEffectInfo> _entityCurrentStatus = new();
    private SetTotalEffectStat _setTotalEffectStat;
    private float totalWeight = 0f;

    public void AttackPercentage() //가중치 계산/부여
    { //스킬 쓸 때마다 호출
        totalWeight = 0;
        
        //상태이상 효과에 따라 다른 가중치 부여
        for (int i = 0; i < _entityCurrentStatus.Count;)
        {
            for (int j = 0; j < _entityCurrentStatus[i].entityStatus.Count; j++)
            {
                var status = _entityCurrentStatus[i].entityStatus[j];
                if (status == StatusType.Normal) return;
                if (status == StatusType.Mark)
                {
                    totalWeight += SetTotalEffectStat.Mark;
                }

                if (status == StatusType.Buff)
                {
                    totalWeight += SetTotalEffectStat.Buff;
                }

                if (status == StatusType.Debuff)
                {
                    totalWeight += SetTotalEffectStat.Debuff;
                }

                if (status == StatusType.Guard)
                {
                    totalWeight += SetTotalEffectStat.Guard;
                }

                if (status == StatusType.Guardian)
                {
                    totalWeight += SetTotalEffectStat.Guardian;
                }

                if (status == StatusType.PlayerReactAtk)
                {
                    totalWeight += SetTotalEffectStat.PlayerReactAtk;
                }

                if (status == StatusType.PlayerReactSupport)
                {
                    totalWeight += SetTotalEffectStat.PlayerReactSupport;
                }
            }

            if (_entityCurrentStatus[i].duration <= 0) //버프 지속시간이 끝나면
            {
                RemoveStatus(_entityCurrentStatus[i]); //상태이상 제거
            }
            else
            {
                _entityCurrentStatus[i].duration--; //턴 감소
                i++;
            }
        }
    }
    
    public void AddStatus(StatEffectInfo status) //상태이상 추가
    {
        _entityCurrentStatus.Add(status); //상태 추가
    }

    public void RemoveStatus(StatEffectInfo status) //상태이상 제거
    {
        _entityCurrentStatus.Remove(status);
    }

    public void Clear() //리스트 내부의 배열 삭제(상태이상 초기화)
    {
      _entityCurrentStatus.Clear();
    }
}