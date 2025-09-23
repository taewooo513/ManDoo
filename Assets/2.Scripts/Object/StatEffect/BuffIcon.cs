using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconFactory
{
    public static string GetBuffIconPath(BuffInfo buff)
    {
        string path = "";
        switch (buff.buffType)
        {
            case BuffType.AttackUp:
                path = "AttackUp";
                break;
            case BuffType.AllStatUp:
                path = "AllStatUp";
                break;
            case BuffType.EvasionUp:
                path = "EvasionUp";
                break;
            case BuffType.CriticalUp:
                path = "CriticalUp";
                break;
            case BuffType.SpeedUp:
                path = "SpeedUp";
                break;
        }
        switch (buff.deBuffType)
        {
            case DeBuffType.AttackDown:
                path = "AttackDown";
                break;
            case DeBuffType.AllStatDown:
                path = "AllStatDown";
                break;
            case DeBuffType.EvasionDown:
                path = "EvasionDown";
                break;
            case DeBuffType.CriticalDown:
                path = "CriticalDown";
                break;
            case DeBuffType.SpeedDown:
                path = "SpeedDown";
                break;
            case DeBuffType.Damaged:
                path = "BurnIcon";
                break;
        }
        Debug.Log(buff.buffType);
        Debug.Log(buff.deBuffType);
        return path;
    }
}

public class BuffIcon : MonoBehaviour
{
    private BuffInfo buff;
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdateUI(BuffInfo buff)
    {
        this.buff = buff;
        image.sprite = SpriteManager.Instance.FindSprite(Constants.BuffSpriteIcon + BuffIconFactory.GetBuffIconPath(buff));

        if (buff.duration <= 1)
        {

        }
    }
}
