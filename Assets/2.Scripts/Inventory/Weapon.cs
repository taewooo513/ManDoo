using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    private int proficiencyLevel;
    private WeaponType weaponType;
    private int attackDmg;
    private int defense;
    private int speed;
    private float evasion;
    private float critical;
    private string gameObjectPath;
    private string iconPath;
    private float exp;
    private float maxExp;
    private int id;
    private bool isLovedWeapon = false;

    public Skill skill { get; private set; }
    WeaponData weaponData;
    public Weapon(int id)
    {
        Setting(id);
        this.id = id;
        exp = 0;
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
    }

    public void InitWeaponSkill(BaseEntity baseEntity)
    {
        skill = new Skill();
        skill.Init(weaponData.skillId, baseEntity);
    }
    public void AddUseExp()
    {
        exp += 5;
        IsLevelUpWeapon();
    }
    public void AddKillExp()
    {
        exp += 15;
        IsLevelUpWeapon();
    }
    public void AddExploringunexploredfrontiersExp()
    {
        exp += 15;
        IsLevelUpWeapon();
    }
    public void AddVictoryExp()
    {
        exp += 30;
        IsLevelUpWeapon();
    }

    private bool IsLevelUpWeapon()
    {
        if (maxExp <= exp && isLovedWeapon == false)
        {
            Setting(id + 1);
            return true;
        }
        return false;
    }
}
