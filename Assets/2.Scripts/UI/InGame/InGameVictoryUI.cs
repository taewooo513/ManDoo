using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameVictoryUI : UIBase
{
    public Button yesButton;
    public Button noButton;

    public void Start()
    {
        Button yesBtn = yesButton.GetComponent<Button>();
        Button noBtn = noButton.GetComponent<Button>();
        yesBtn.onClick.AddListener(YesButtonOnClick);
        noBtn.onClick.AddListener(NoButtonOnClick);
    }

    public void YesButtonOnClick()
    {
        Debug.Log("You have clicked yes button!");
    }
    
    public void NoButtonOnClick()
    {
        Debug.Log("You have clicked no button!");
        UIManager.Instance.CloseUI<InGameVictoryUI>();
    }
}
