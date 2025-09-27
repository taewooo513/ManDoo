using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    private void Awake()
    {
        if (icon == null)
            icon = GetComponentInChildren<Image>();
        if (countText == null)
            countText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSlot(eItemType type, int id, int count)
    {
        if (type == eItemType.Consumable)
        {
            var item = ItemManager.Instance.CreateItem(id);
            var itemIcon = ItemManager.Instance.GetItemIcon(item);
            if (icon != null)
            {
                icon.sprite = itemIcon;
                icon.enabled = (itemIcon != null);
            }
            if (countText != null)
                countText.text = count.ToString();
        }
        else if (type == eItemType.Weapon)
        {
            var weapon = ItemManager.Instance.CreateWeapon(id);
            var weaponIcon = ItemManager.Instance.GetWeaponIcon(weapon);
            if (icon != null)
            {
                icon.sprite = weaponIcon;
                icon.enabled = (weaponIcon != null);
            }
        }
        
    }
}