using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    [SerializeField] private int initID;
    private MercenaryData data;
    private Weapon equipWeapon;
   

    public override void Init(int id)
    {
        SetData(initID);
        buffIcons.UpdateIcon(entityInfo.statEffect);
    }

    public void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical
        );
        entityInfo.SetUpSkill(data.skillId, this);
    }

    public override void Attack(float dmg, BaseEntity baseEntity)
    {
        base.Attack(dmg, baseEntity);
        BattleManager.Instance.AttackEntity(
            Utillity.GetIndexInListToObject(BattleManager.Instance._enemyCharacters, baseEntity), dmg);
    }

    public override void UseSkill(BaseEntity baseEntity)
    {
        base.UseSkill(baseEntity);
    }
    public override void StartTurn()
    {
        base.StartTurn();
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills);
    }
    public override void EndTurn(bool hasExtraTurn = true)
    {

    }

    public void EquipWeapon(Weapon weapon)
    {
        if (IsEquipWeapon(weapon))
        {
            UnEquipWeapon();
            return;
        }
        equipWeapon = weapon;
        entityInfo.skills[3] = weapon.skill;
    }
    private void UnEquipWeapon()
    {
        equipWeapon = null;
        entityInfo.skills[3] = null;
    }
    private bool IsEquipWeapon(Weapon weapon)
    {
        if (equipWeapon == weapon)
        {
            return true;
        }
        return false;
    }
}