using System.Collections;
using System.Collections.Generic;
using DataTable;
using DefaultTable;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    [SerializeField] private int initID;
    private MercenaryData data;
    public Weapon equipWeapon;

    private void Start()
    {
    }

    public override void Init(int id)
    {
        SetData(id);
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
        List<BuffType> buffTypes = new List<BuffType>();
        List<DeBuffType> deBuffTypes = new List<DeBuffType>();
        buffTypes.Add(BuffType.AttackUp);
        buffTypes.Add(BuffType.DefenseUp);
        buffTypes.Add(BuffType.SpeedUp);
        buffTypes.Add(BuffType.EvasionUp);
        buffTypes.Add(BuffType.CriticalUp);
        buffTypes.Add(BuffType.AllStatUp);

        deBuffTypes.Add(DeBuffType.AttackDown);
        deBuffTypes.Add(DeBuffType.DefenseDown);
        deBuffTypes.Add(DeBuffType.SpeedDown);
        deBuffTypes.Add(DeBuffType.EvasionDown);
        deBuffTypes.Add(DeBuffType.CriticalDown);
        deBuffTypes.Add(DeBuffType.AllStatDown);
        deBuffTypes.Add(DeBuffType.Damaged);
        entityInfo.statEffect.AttackWeight(entityInfo);
        if (hasExtraTurn)
        {
            buffIcons.UpdateIcon(entityInfo.statEffect);
            Damaged(entityInfo.statEffect.totalStat.damagedValue);
            entityInfo.statEffect.ReduceTurn(buffTypes, deBuffTypes);
        }
        BattleManager.Instance.EndTurn(hasExtraTurn);
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
    
    // TODO: 장착/획득 아이템 전분 인벤토리매니저에 넘겨주기.
}