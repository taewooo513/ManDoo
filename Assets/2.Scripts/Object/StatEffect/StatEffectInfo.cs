using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Normal, Mark, Buff, Debuff, Guard, Guardian, PlayerReactAtk, PlayerReactSupport
}

public class StatEffectInfo //상태이상 효과 정보 리스트
{
    public List<StatusType> entityStatus = new();
    public int duration;
    public int constValue;
    public BaseEntity actionEntity;


    public void Init(int duration, int constValue, BaseEntity baseEntity) //스킬 사용할 때 호출
    {
        this.duration = duration;
        this.constValue = constValue;
        actionEntity = baseEntity;
    }
}
