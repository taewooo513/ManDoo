using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillButton : MonoBehaviour
{
    
    private Button skillButton;
    private InGameUIManager inGameUIManager;

    private void Awake()
    {
        
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
    }
    public void SetButton(Skill skill)
    {
        skillButton.onClick.AddListener(() => OnClickSkillButton(skill));
    }

    private void OnClickSkillButton(Skill skill)
    {
        inGameUIManager.OnClickSkillButton(skill);
    }

}
