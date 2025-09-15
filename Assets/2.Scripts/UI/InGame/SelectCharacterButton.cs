using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    private Button button;
    public int index;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setting(int index)
    {
        button.onClick.AddListener(() => OnClickSkillButton(index));
    }

    public void OnClickSkillButton(int index)
    {
        BattleManager.Instance.SelectPlayer(index);
    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}