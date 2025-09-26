using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterAnimationController : EntityCharacterAnimationController
{
    int layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    SpriteRenderer sprites;
    BaseEntity nowEntity;
    public void Awake()
    {
        sprites = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        nowEntity = GetComponentInParent<Enemy>();
    }

    public override void Attack(Action action, BaseEntity targetEntity)
    {
        animator.SetTrigger("Attack");
        this.targetEntity = targetEntity;
        targetEntity.characterAnimationController.LayerUp();
        LayerUp();
        this.action = action;
    }
    public override void Attack(Action action, List<BaseEntity> baseEntitys)
    {
        animator.SetTrigger("Attack");
        this.baseEntitys = baseEntitys;
        LayerUp();
        baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerUp(); });
        this.action = action;
    }
    public override void LayerUp()
    {
        layers = sprites.sortingOrder;
        sprites.sortingOrder += 50;
        BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public override void LayerDown()
    {
        sprites.sortingOrder = layers;
        BattleManager.Instance.blackOutImage.SetActive(false);
    }

    public override void ActionEvent()
    {
        action.Invoke();
        LayerDown();

        if (targetEntity != null)
        {
            targetEntity.characterAnimationController.LayerDown();
            targetEntity = null;
        }
        else
        {
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
            baseEntitys = null;
        }
        nowEntity.EndTurn();
    }
}
