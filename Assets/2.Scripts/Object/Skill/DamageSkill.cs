using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillEffect
{
    public void DeBuffDamageSkill(int duration, BaseEntity targetEntity)
    {

    }
    public void HitDamageSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        attackEntity.Attack(1, damagedEntity);
    }

    public void UseBuffSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        if (duration != 0)
        {
            DeBuffDamageSkill(duration, damagedEntity);
        }
        else
        {
            HitDamageSkill(attackEntity, damagedEntity);
        }
    }
}
