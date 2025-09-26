using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    private BaseRoom _room;
    [SerializeField] private Button button;
    public void Init(BaseRoom room)
    {
        _room = room;
        icon.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        SetIcon();
        _room.SetRoomUI(this);
    }

    private void SetIcon()
    {
        switch (_room)
        {
            case BattleRoom:
                
                break;
            case EmptyRoom:
                
                break;
            case TreasureRoom:
                
                break;
            case PmcRoom:
                
                break;
            case ShopRoom:
                
                break;
            case StartRoom:

                icon.gameObject.SetActive(true);
                break;
            case VillageRoom:

                icon.gameObject.SetActive(true);
                break;
        }
    }

    public void ActivateIcon()
    {
        icon.gameObject.SetActive(true);
    }

    public void ActivateButton()
    {
        button.gameObject.SetActive(true);
    }

    public void EnterCorridor()
    {
        //_room.EnterCorridor(_room);
    }
}
