using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : SkillEffect
{
    public virtual void UseBuffSkill(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        BuffInfo statEffectInfo = new BuffInfo();
        statEffectInfo.Init(duration,constantValue,actionEntity);
        targetEntity.AddEffect(statEffectInfo);
    }
}
