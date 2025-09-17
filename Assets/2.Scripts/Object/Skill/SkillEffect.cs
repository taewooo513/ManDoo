using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    private EffectData datas;
    protected EffectType effectType;
    protected float adRatio;
    protected int constantValue;
    protected int duration;
  
    public void Init(int id)
    {
        //DataManager.Instance.Effect.GetEnemyData;
    }
    public virtual void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {

    }
}
