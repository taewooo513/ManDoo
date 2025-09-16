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
}
