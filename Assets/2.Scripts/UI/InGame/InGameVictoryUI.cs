using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameVictoryUI : UIBase
{
    public Button yesButton;
    public Button noButton;
    private Dictionary<int, int> rewardItems = new (); // key: id, value: count
    private List<RectTransform> contents = new();
    private ScrollRect sr;

    private void Awake()
    {
        if (sr == null)
            sr = GetComponent<ScrollRect>();
        var rewards = sr.GetComponentsInChildren<RectTransform>(true);

        foreach (var reward in rewards)
        {
            //if (reward)
        }
    }
    public void Start()
    {
        Button yesBtn = yesButton.GetComponent<Button>();
        Button noBtn = noButton.GetComponent<Button>();
        yesBtn.onClick.AddListener(YesButtonOnClick);
        noBtn.onClick.AddListener(NoButtonOnClick);

        // if (rewardItemObjects == null)
        // {
        //     
        // }
    }

    public void YesButtonOnClick()
    {
        foreach (var item in rewardItems)
        {
            var id = item.Key;
            var remaining = item.Value;

            while (remaining > 0)
            {
                if (!InventoryManager.Instance.TryAddItem(id, remaining))
                    break;
                remaining--;
            }
        }
        UpdateContents();
    }
    
    public void NoButtonOnClick() => UIManager.Instance.CloseUI<InGameVictoryUI>();

    private void UpdateContents()
    {
        // foreach (var item in rewardItemObjects)
        // {
        //     if ()
        // }
    }
}
