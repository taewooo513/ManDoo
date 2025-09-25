using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private int proficiencyLevel;
    private WeaponType weaponType;
    public int attackDmg;
    public int defense;
    public int speed;
    public float evasion;
    public float critical;
    private string gameObjectPath;
    private string iconPath;
    private float _weaponExp;
    private float _weaponMaxExp;
    private int id;
    private bool isLovedWeapon = false;

    public Skill skill { get; private set; }
    WeaponData weaponData;
    public Weapon(int id)
    {
        Setting(id);
        this.id = id;
        _weaponExp = 0;
    }

    private void Setting(int id)
    {
        weaponData = DataManager.Instance.Weapon.GetWeaponData(id);
        attackDmg = weaponData.attack;
        defense = weaponData.defense;
        speed = weaponData.speed;
        evasion = weaponData.evasion;
        critical = weaponData.critical;
        gameObjectPath = weaponData.gameObjectString;
        iconPath = weaponData.iconPathString;
        this.id = id;

        weaponType = weaponData.weaponType;
    }

    public void InitWeaponSkill(BaseEntity baseEntity)
    {
        skill = new Skill();
        skill.Init(weaponData.skillId, baseEntity);
    }
    
    public void AddWeaponExp(int exp)
    {
        _weaponExp += exp;
        IsLevelUpWeapon();
    }
    
    private bool IsLevelUpWeapon()
    {
        if (_weaponMaxExp <= _weaponExp && isLovedWeapon == false)
        {
            Setting(id + 1);
            return true;
        }
        return false;
    }
    
    // private 으로 되어있어서 getter 생성
    public WeaponType GetWeaponType() => weaponType;
    public int GetId => id;
    public string GetIconPath() => iconPath;
}