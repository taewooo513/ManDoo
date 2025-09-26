using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameVictoryUI : UIBase
{
    public Button yesButton;
    public Button noButton;

    // [SerializeField] private RewardSlotUI slot;
    // private List<RewardSlotUI> activeSlots;
    // private Queue<RewardSlotUI> pool = new();
    
    private Dictionary<(eItemType, int id), int> rewards = new(); // key: type & id, value: count

    private ScrollRect sr;
    private RectTransform contentRt;


    private void Awake()
    {
        if (sr == null)
            sr = GetComponent<ScrollRect>();
        if (sr != null)
            contentRt = sr.content;
    }
    public void Start()
    {
        Button yesBtn = yesButton.GetComponent<Button>();
        Button noBtn = noButton.GetComponent<Button>();
        yesBtn.onClick.AddListener(YesButtonOnClick);
        noBtn.onClick.AddListener(NoButtonOnClick);
    }

    public void YesButtonOnClick()
    {
        foreach (var reward in rewards)
        {
            var key = reward.Key;
            var value = reward.Value;
            InventoryManager.Instance.TryAddItem(key.Item1, key.Item2, value);
        }
        
        UpdateContents();
    }
    
    public void NoButtonOnClick() => UIManager.Instance.CloseUI<InGameVictoryUI>();

    private void UpdateContents() 
    {
        // TODO: 보상 슬롯 초기화
    }
}
