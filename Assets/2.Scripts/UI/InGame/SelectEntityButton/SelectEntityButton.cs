using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SelectEntityButton : MonoBehaviour
{
    protected Button button;
    protected InGamePlayerUI inGamePlayerUI;
    private InGameUIManager inGameUIManager;
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
        inGameUIManager.AddSkillButtonAction(SwapButtonAction);
        
    }

    public virtual void OnClickButton()
    {
    }

    protected virtual void OnClickActionButton(Skill skill)
    {
    }

    public virtual void SwapButtonAction(Skill skill)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill));
    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
