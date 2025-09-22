using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillEffect
{
    public void HitDamageSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        attackEntity.Attack(((float)attackEntity.entityInfo.attackDamage) * adRatio + constantValue, damagedEntity);
    }

    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        HitDamageSkill(actionEntity, targetEntity);
    }
}
