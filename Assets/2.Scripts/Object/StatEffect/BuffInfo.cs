using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo //상태이상 효과 정보 리스트
{
    public BuffType buffType;
    public DeBuffType deBuffType;
    public int duration;
    public int constantValue;
    public void Init(int duration, int constantValue, BaseEntity baseEntity) //스킬 사용할 때 호출
    {
        this.duration = duration;
        this.constantValue = constantValue;
    }
}