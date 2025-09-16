using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillButton : MonoBehaviour
{
    private Skill skill;
    private Button skillButton;
    private InGameUIManager inGameUIManager;

    private void Awake()
    {
        skillButton.onClick.AddListener(OnClickSkillButton);
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
    }

    private void OnClickSkillButton()
    {
        inGameUIManager.OnClickSkillButton(skill);
    }

}
