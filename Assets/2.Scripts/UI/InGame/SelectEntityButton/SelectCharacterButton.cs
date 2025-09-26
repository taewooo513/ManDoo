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

    protected override void OnClickActionButton(Skill skill)
    {
        skill.UseSkill(player);
        BattleManager.Instance.NowTurnEntity.EndTurn();
    }

    public override void ActiveSkillButtonAction(Skill skill)
    {
        if (skill.GetSkillType() == EffectType.Debuff || skill.GetSkillType() == EffectType.Attack)
        {
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill));
    }
}