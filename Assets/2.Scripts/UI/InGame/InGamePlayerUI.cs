using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGamePlayerUI : UIBase
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI jobText;

    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI currentHpUI;

    public void SettingUI(string name, int hp, int currentHp, Skill[] skills)
    {
        nameText.text = name;
        hpUI.text = hp.ToString();
        currentHpUI.text = currentHp.ToString();
    }

    public void UpdateHpUI(int hp)
    {
        
    }
}