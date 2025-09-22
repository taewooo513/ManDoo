using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeBuffSkill : SkillEffect
{
    public virtual void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity) // not used
    {
        StatEffectInfo statEffectInfo = new StatEffectInfo();

        targetEntity.AddEffect(statEffectInfo);
    }
}
