using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayableCharacterAnimationController : MonoBehaviour
{
    private Animator animator;
    Action action;
    int[] layers;
    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack(Action action)
    {
        animator.SetTrigger("Attack");
        this.action = action;
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        layers = new int[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            layers[i] = sprites[i].sortingOrder;
            sprites[i].sortingOrder += 50;
        }
        BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public void ActionEvent()
    {
        action.Invoke();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = layers[i];
        }
        BattleManager.Instance.blackOutImage.SetActive(false);
    }
}
