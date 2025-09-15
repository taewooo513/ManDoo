using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGamePlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI jobText;

    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI currentHpUI;

    public List<Button> playerButtons; //플레이어 버튼

    public Image[] skillIcon;
    public Button[] selectedSkillButtons; // 스킬선택 버튼

    public void SettingUI(string name, string job, int hp, int currentHp, Skill[] skills)
    {
        nameText.text = name;
        jobText.text = job;
        hpUI.text = hp.ToString();
        currentHpUI.text = currentHp.ToString();
        
    }
}