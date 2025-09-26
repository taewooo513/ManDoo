using DataTable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    [Header("common Data")]
    public int id; // 아이템 ID (소비 아이템/무기 아이템 공통)
    //public string iconPathString; // 아이템 아이콘 경로

    [Header("consumable Data")]

    public ItemType itemType; // 아이템 타입 (소비 아이템/무기 아이템)
    public int consumableSkillId; // 소비 아이템 사용 시 발동되는 스킬 ID
    public string itemName; // 아이템 이름
    public string itemDescription; // 아이템 설명
    public int maxCount; // 최대 소지 개수 (소비 아이템에만 해당)
    public int price; // 아이템 가격
   
    [Header("weapon Data")]
    public int proficiencyLevel; // 무기 숙련도
    public WeaponType weaponType; // 무기 타입 (검/활/지팡이 등)
    public int attack; // 무기 공격력
    public int defense; // 무기 방어력
    public int speed; // 무기 속도
    public float evasion; // 무기 회피율
    public float critical; // 무기 치명타 확률
    public int skillId; // 무기 장착 시 발동되는 스킬 ID
    //public string gameObjectString; // 무기 게임 오브젝트 경로

    // 아이템 정보 (소비 아이템/무기 아이템 중 하나만 세팅됨)
    private ConsumableData consumableData; // 소비 아이템 데이터
    private WeaponData weaponData; // 무기 아이템 데이터

    // 예시: UI 오브젝트들
    public TextMeshProUGUI itemNameText;
    public Image iconImage;
    public Button buyButton;

    public void SetConsumableData(ConsumableData data)
    {
        //consumableData = data;
        id = data.id;
        itemType = data.itemType;
        consumableSkillId = data.consumableSkillId;
        itemName = data.itemName;
        itemDescription = data.itemDescription;
        maxCount = data.maxCount;
        price = data.price;
        //iconPathString = data.iconPathString;

        // UI 반영
        itemNameText.text = data.itemName;
        iconImage.sprite = Resources.Load<Sprite>(data.iconPathString);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyConsumable);
    }

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
        id = data.id;
        proficiencyLevel = data.proficiencyLevel;
        weaponType = data.weaponType;
        attack = data.attack;
        defense = data.defense;
        speed = data.speed;
        evasion = data.evasion;
        critical = data.critical;
        skillId = data.skillId;
        //gameObjectString = data.gameObjectString;
        //iconPathString = data.iconPathString;
        

        // UI 반영
        itemNameText.text = data.weaponType.ToString();
        iconImage.sprite = Resources.Load<Sprite>(data.iconPathString);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyWeapon);
    }

    private void BuyConsumable()
    {
        UIManager.Instance.OpenUI<InGamePMCUI>();
        if (consumableData != null)
        {
            Debug.Log($"{consumableData.itemName} 구매!");
            // 골드 차감 등 추가 필요

            // eItemType과 ItemType이 enum 이름이 같으면 캐스팅 가능
            bool success = InventoryManager.Instance.TryAddItem(eItemType.Consumable, consumableData.id, 1); // 1개 구매
            if (success)
                Debug.Log("소비 아이템 인벤토리에 추가됨!");
            else
                Debug.LogWarning("인벤토리 공간 부족 또는 저장 한도 초과!");
        }
    }

    private void BuyWeapon()
    {
        if (weaponData != null)
        {
            Debug.Log($"{weaponData.weaponType} 구매!");
            // 골드 차감 등 추가 필요

            bool success = InventoryManager.Instance.TryAddItem(eItemType.Weapon, weaponData.id, 1);
            if (success)
                Debug.Log("무기 아이템 인벤토리에 추가됨!");
            else
                Debug.LogWarning("인벤토리 공간 부족 또는 저장 한도 초과!");
        }
    }
}