using UnityEngine;
using DataTable;

/// <summary>
/// 아이템의 기본 정보를 담고 있는 클래스
/// </summary>
public class ItemInfo
{
    public ItemType itemType;        // 아이템 타입 (BattleSupport, ExplorerSupport, Gold)
    public int consumableSkillId;    // 소비 아이템 사용 시 발동되는 스킬 ID
    public string itemName;          // 아이템 이름
    public string itemDescription;   // 아이템 설명
    public int maxCount;            // 최대 소지 가능 개수
    public int price;               // 아이템 가격
    public string iconPathString;   // 아이템 아이콘 리소스 경로
    private ConsumableData itemData; // 아이템 데이터 테이블 정보
    
    /// <summary>
    /// 아이템 정보를 초기화하는 생성자
    /// </summary>
    /// <param name="id">아이템 ID</param>
    public ItemInfo(int id)
    {
        this.itemData = DataManager.Instance.Consumable.GetConsumableData(id);
        this.itemName = itemData.itemName;
        this.itemDescription = itemData.itemDescription;
        this.maxCount = itemData.maxCount;
        this.price = itemData.price;
        this.iconPathString = itemData.iconPathString;
    }
}

/// <summary>
/// 소비 아이템을 나타내는 클래스
/// </summary>
//[System.Serializable]
public class Item
{
    public ItemInfo ItemInfo { get; private set; }  // 아이템 기본 정보
    public int ItemId { get; private set; }         // 아이템 고유 ID
    public Sprite icon;                             // 아이템 아이콘 스프라이트
    private EnemyData _data;                        // TODO: EnemyData 사용 여부 확인 필요

    /// <summary>
    /// 아이템을 생성하는 생성자
    /// </summary>
    /// <param name="id">아이템 ID</param>
    public Item(int id)
    {
        ItemId = id;
        Init(id);
    }

    /// <summary>
    /// 아이템 정보를 초기화하는 메서드
    /// </summary>
    /// <param name="id">아이템 ID</param>
    public void Init(int id) => ItemInfo = new ItemInfo(id);
}