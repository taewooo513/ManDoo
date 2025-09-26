using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterAnimationController : EntityCharacterAnimationController
{
    int layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    public void Awake()
    {
        animator = GetComponent<Animator>();
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
        SpriteRenderer sprites = GetComponent<SpriteRenderer>();
        sprites.sortingOrder += 50;

        layers = sprites.sortingOrder;
        BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public override void LayerDown()
    {
        SpriteRenderer sprites = GetComponent<SpriteRenderer>();

        sprites.sortingOrder = layers;
        BattleManager.Instance.blackOutImage.SetActive(false);
    }

    public override void ActionEvent()
    {
        action.Invoke();
        LayerDown();
        if (targetEntity != null)
            targetEntity.characterAnimationController.LayerDown();
        else
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
    }
}
