using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private InGameInventoryUI inventoryUI; // 씬의 인벤토리 UI 참조
    [SerializeField] private InGameVictoryUI victoryUI;     // 리워드 UI(승리 화면) 참조 (있다면)

    [Header("테스트 데이터")]
    // 실제 프로젝트의 유효한 ID로 바꾸세요
    // item ids: {1001, 2001, 3001, 2002, 2003, 3002};
    [SerializeField]
    private int[] consumableId = new int[10];
    [SerializeField] private int weaponId = 40011;

    private void Start()
    {
        // 1) 인벤토리 아이템 채우기
        var im = InventoryManager.Instance;

        foreach (var item in consumableId)
        {
            im.TryAddItem(eItemType.Consumable, item, 2);
        }
        
        im.TryAddItem(eItemType.Weapon, weaponId, 1);

        // 2) 장비창(무기) 장착 테스트
        // 빈 슬롯에서 무기 하나를 장착해두고 시작
        for (int i = 0; i < im.GetSlotCount(); i++)
        {
            var item = im.GetItemInSlot(i);
            if (item != null && im.GetItemType(i) == eItemType.Weapon)
            {
                im.TryEquipFromInventory(i, EquipmentSlotType.Weapon);
                break;
            }
        }

        // 3) 리워드(승리 UI) 테스트 데이터 추가

        foreach (var item in consumableId)
        {
            ItemManager.Instance.AddReward(eItemType.Consumable, item, 2);
        }
        ItemManager.Instance.AddReward(eItemType.Weapon, weaponId, 1);

        // 4) UI 갱신
        if (inventoryUI != null)
            inventoryUI.RefreshSlots();

        // 승리/리워드 UI가 별도 초기화 루틴이 있으면 호출
        if (victoryUI != null)
            victoryUI.RefreshRewards(); 
    }
}