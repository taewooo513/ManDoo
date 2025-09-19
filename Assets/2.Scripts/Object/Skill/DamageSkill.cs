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
        Debug.Log(attackEntity.entityInfo.name + "가 " + damagedEntity.entityInfo.name + "를 공격함");
        Debug.Log("대미지: " + (attackEntity.entityInfo.attackDamage) * adRatio);
        attackEntity.Attack(((float)attackEntity.entityInfo.attackDamage) * adRatio, damagedEntity);
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

    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        HitDamageSkill(actionEntity, targetEntity);
    }
}
