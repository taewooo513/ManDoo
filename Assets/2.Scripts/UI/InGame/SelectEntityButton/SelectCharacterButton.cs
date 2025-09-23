using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class SelectCharacterButton : SelectEntityButton
{
    PlayableCharacter player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayableCharacter>();
    }

}