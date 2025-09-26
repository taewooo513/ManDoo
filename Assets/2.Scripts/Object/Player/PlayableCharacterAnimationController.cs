using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayableCharacterAnimationController : EntityCharacterAnimationController
{
    int[] layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    SpriteRenderer[] sprites;
    BaseEntity nowEntity;
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        nowEntity = GetComponentInParent<PlayableCharacter>();
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
        layers = new int[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            layers[i] = sprites[i].sortingOrder;
            sprites[i].sortingOrder += 50;
        }
    }

    public override void LayerDown()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = layers[i];
        }
    }

    public override void ActionEvent()
    {
        action.Invoke();
        LayerDown();
        if (targetEntity != null)
        {
            targetEntity.characterAnimationController.LayerDown();
        }
        else
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
        BattleManager.Instance.EndTurn(false);
    }
}
