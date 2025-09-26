using System.Collections.Generic;
using UnityEngine;

public class ConsumableData
{
    public int id;
    public string itemType;
    public int consumableSkillId;
    public string itemName;
    public string itemDescription;
    public int maxCount;
    public int price;
    public string iconPathString;

    public ConsumableData(int id, string itemType, int consumableSkillId, string itemName, string itemDescription, int maxCount, int price, string iconPathString)
    {
        this.id = id;
        this.itemType = itemType;
        this.consumableSkillId = consumableSkillId;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.maxCount = maxCount;
        this.price = price;
        this.iconPathString = iconPathString;
    }
}

public class ShopItemManager : MonoBehaviour
{
    public List<ConsumableData> consumableShopList = new List<ConsumableData>();
    public List<(int id, string name, int attack, int defense, string prefabPath, string iconPath, int price)> weaponShopList = new List<(int, string, int, int, string, string, int)>();

    public GameObject shopItemPrefab; // ShopItem 프리팹 (버튼, 아이콘, 텍스트 등)
    public Transform itemButtonParent; // 상점 UI에서 버튼들이 들어갈 부모

    void Start()
    {
        // Consumable 아이템 데이터 세팅
        var allConsumables = new List<ConsumableData>
        {
            new ConsumableData(2001, "BattleSupport", 29, "회복약", "은은한 빛을 띠는 회복약.\n사용 즉시 현재 캐릭터의 체력을 30 회복한다.", 5, 200, "Sprites/Consumable/2"),
            new ConsumableData(3001, "ExplorerSupport", -1, "함정 해제 도구", "정교하게 세공된 함정 해제 도구.\n사용하면 함정을 확실하게 무력화하여 안전하게 길을 지나갈 수 있다.", 3, 200, "Sprites/Consumable/3"),
            new ConsumableData(2002, "BattleSupport", 30, "폭탄", "거대한 폭발을 일으키는 전투 지원용 폭탄.\n사용 즉시 적 전체에게 강력한 충격을 가해 20의 피해를 준다.", 3, 600, "Sprites/Consumable/4"),
            new ConsumableData(2003, "BattleSupport", 31, "강화의 물약", "강화의 물약.\n쓰고 화학적인 맛이 난다. 더이상 순수해지지 않은 것 같다…\n사용 즉시 행동 1번 동안  현재 캐릭터의 공격력 50% 올려준다.", 1, 400, "Sprites/Consumable/5"),
            new ConsumableData(3002, "ExplorerSupport", -1, "지도", "현재 지역의 지도\n지역의 구조와 개척지들을 보여준다.", 1, 500, "Sprites/Consumable/6")
        };
        consumableShopList.AddRange(allConsumables);

        // 무기 아이템 데이터 세팅
        var allWeapons = new List<(int id, string name, int attack, int defense, string prefabPath, string iconPath, int price)>
        {
            (40011, "TwoHandedSword", 3, 3, "Prefabs/Weapon/1", "Sprites/Weapon/1", 500),
            (40021, "ShortSword", 1, 0, "Prefabs/Weapon/1", "Sprites/Weapon/1", 500),
            (40031, "Shield", 1, 5, "Prefabs/Weapon/1", "Sprites/Weapon/1", 500),
            (40041, "Bow", 3, 0, "Prefabs/Weapon/1", "Sprites/Weapon/1", 500),
            (40051, "Staff", 6, 0, "Prefabs/Weapon/1", "Sprites/Weapon/1", 500)
        };
        foreach (var w in allWeapons)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand < 0.33f)
            {
                weaponShopList.Add(w);
            }
        }

        // UI에 ShopItem 프리팹 동적으로 생성
        CreateShopItems();
    }

    void CreateShopItems()
    {
        foreach (var c in consumableShopList)
        {
            GameObject obj = Instantiate(shopItemPrefab, itemButtonParent);
            var shopItem = obj.GetComponent<ShopItem>();
            shopItem.SetConsumableData(c); // 데이터 전달
        }
        foreach (var w in weaponShopList)
        {
            GameObject obj = Instantiate(shopItemPrefab, itemButtonParent);
            var shopItem = obj.GetComponent<ShopItem>();
            shopItem.SetWeaponData(w); // 데이터 전달
        }
    }
}