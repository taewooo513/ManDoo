using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : SkillEffect
{
    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        BuffInfo statEffectInfo = new BuffInfo();
        statEffectInfo.Init(duration, constantValue, actionEntity, buffType, debuffType);
        targetEntity.AddEffect(statEffectInfo);
    }
}
