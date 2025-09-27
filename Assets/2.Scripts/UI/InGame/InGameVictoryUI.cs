using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameVictoryUI : UIBase
{
    public Button yesButton;
    public Button noButton;

    [SerializeField] private RewardSlotUI rewardSlot; // reward slot prefab
    [SerializeField] private RectTransform content;
    
    private Dictionary<(eItemType, int id), int> rewards = new(); // key: type & id, value: count

    // private ScrollRect sr;
    // private RectTransform contentRt;
    private void Awake()
    {
        if (rewardSlot == null)
            Debug.LogError("Reward Slot 프리팹 비어있음.");
        if (content == null)
            Debug.LogError("Content 오브젝트 비어있음.");
    }
    
    public void Start()
    {
        Button yesBtn = yesButton.GetComponent<Button>();
        Button noBtn = noButton.GetComponent<Button>();
        yesBtn.onClick.AddListener(YesButtonOnClick);
        noBtn.onClick.AddListener(NoButtonOnClick);
    }

    public void OnEnable()
    {
        rewards = ItemManager.Instance.GetRewards();
        UpdateContents();
    }
    
    public void YesButtonOnClick() 
    {
        foreach (var reward in rewards)
        {
            var key = reward.Key;
            var value = reward.Value;
            // TODO: 리워드 아이템을 인벤토리에 다 넣을 수 있는지 확인하고 넣는 설계 구현 (all or nothing)
            if (!InventoryManager.Instance.TryAddItem(key.Item1, key.Item2, value)) return;
        }
        
        ItemManager.Instance.ClearRewards(); 
        rewards.Clear();
        UpdateContents();
        UIManager.Instance.CloseUI<InGameVictoryUI>();
    }
    
    public void NoButtonOnClick() => UIManager.Instance.CloseUI<InGameVictoryUI>();

    public void ObtainRewardItem(eItemType type, int id, int amount)
    {
        var key = (type, id);
        if (!rewards.ContainsKey(key)) return;
        rewards[key] += amount;
        if (rewards[key] <= 0)
            rewards.Remove(key);
        
        UpdateContents();
    }

    private void UpdateContents() 
    {
        // TODO: 보상 슬롯 초기화
        if (rewards == null) return;
        if (content == null || rewardSlot == null) return;

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        foreach (var reward in rewards)
        {
            var key = reward.Key; // item1: type, item2: id
            var value = reward.Value; // value: amount
            
            var newSlot = Instantiate(rewardSlot, content);
            newSlot.gameObject.SetActive(true);
            newSlot.SetSlot(key.Item1, key.Item2, value);
        }
    }
}
