using System.Collections;
using System.Collections.Generic;
using GoogleSheet.Core.Type;
using UnityEngine;


[UGS(typeof(RoleType))]
public enum RoleType
{
    Nuker,
    Initiator,
    Lurker,
    Supporter
}
[UGS(typeof(EffectType))]
public enum EffectType
{
    Attack,
    Protect,
    Support,
    StatusEffect
}
[UGS(typeof(TargetType))]
public enum TargetType
{
    Single,
    Range
}