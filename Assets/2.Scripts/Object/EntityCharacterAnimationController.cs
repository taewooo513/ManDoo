using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EntityCharacterAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected Action action;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Attack(Action action, BaseEntity baseEntity)
    {

    }
    public virtual void Attack(Action action, List<BaseEntity> baseEntitys)
    {

    }
    public virtual void LayerUp( )
    {
    }
    public virtual void LayerDown( )
    {
    }

    public virtual void ActionEvent()
    {

    }
}
