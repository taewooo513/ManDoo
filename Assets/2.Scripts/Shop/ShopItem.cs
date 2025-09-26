using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // UI 컴포넌트 연결
    public Image iconImage;
    public Text nameText;
    public Text priceText;
    public Button buyButton;

    // 아이템 정보
    private ConsumableData consumableData;
    private (int id, string name, int attack, int defense, string prefabPath, string iconPath, int price)? weaponData;

    public void SetConsumableData(ConsumableData data)
    {
        consumableData = data;
        iconImage.sprite = Resources.Load<Sprite>(data.iconPathString);
        nameText.text = data.itemName;
        priceText.text = data.price.ToString();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyConsumable);
    }

    public void SetWeaponData((int id, string name, int attack, int defense, string prefabPath, string iconPath, int price) data)
    {
        weaponData = data;
        iconImage.sprite = Resources.Load<Sprite>(data.iconPath);
        nameText.text = data.name;
        priceText.text = data.price.ToString();
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyWeapon);
    }

    private void BuyConsumable()
    {
        if (consumableData != null)
        {
            Debug.Log($"{consumableData.itemName} 구매!");
            // 돈 차감, 인벤토리 추가 등 실제 구매 처리 필요
        }
    }

    private void BuyWeapon()
    {
        if (weaponData != null)
        {
            Debug.Log($"{weaponData.Value.name} 구매!");
            // 돈 차감, 인벤토리 추가 등 실제 구매 처리 필요
        }
    }
}
