using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatEffect
{
    private List<StatEffectInfo> _entityCurrentStatus = new();
    private SetTotalEffectStat _setTotalEffectStat;

    private void AttackPercentage() //가중치 부여
    {
        float total = 10;
        float rand = UnityEngine.Random.value * total;
        
        //상태이상 효과에 따라 다른 가중치 부여
        for (int i = 0; i < _entityCurrentStatus.Count;)
        {
            for (int j = 0; j < _entityCurrentStatus[i].entityStatus.Count; j++)
            {
                var status = _entityCurrentStatus[i].entityStatus[j];
                if (status == StatusType.Normal) return;
                if (status == StatusType.Mark) { }
                if (status == StatusType.Buff) { }
                if (status == StatusType.Debuff) { }
                if (status == StatusType.Guard) { }
                if (status == StatusType.Guardian) { }
                if (status == StatusType.PlayerReactAtk) { }
                if (status == StatusType.PlayerReactSupport) { }
            }

            // if () //버프 지속시간이 끝나면
            // {
            //     RemoveStatus(_entityCurrentStatus[i]);
            // }
            // else
            // {
            //     i++;
            // }
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

    
    //상태이상 효과들
    public void WeightCalculation()
    {
        for (int i = 0; i < _entityCurrentStatus.Count;)
        {
            for (int j = 0; j < _entityCurrentStatus[i].entityStatus.Count; j++)
            {
                var status = _entityCurrentStatus[i].entityStatus[j];
                if (status == StatusType.Normal) return;
                if (status == StatusType.Mark) { }
                if (status == StatusType.Buff) { }
                if (status == StatusType.Debuff) { }
                if (status == StatusType.Guard) { }
                if (status == StatusType.Guardian) { }
                if (status == StatusType.PlayerReactAtk) { }
                if (status == StatusType.PlayerReactSupport) { }
            }
        }
    }
}
