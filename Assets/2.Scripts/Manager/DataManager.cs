using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public SkillDatas Skill;
    public MercenaryDatas Mercenary;//Player로 변경해도 됨.
    public EnemyDatas Enemy;
    public EffectDatas Effect;
    public WeaponDatas Weapon;
    //Item 데이터테이블 만들고 생성; 원본 데이터에는 아이템 id.
    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Skill = new SkillDatas();
        Mercenary = new MercenaryDatas();
        Enemy = new EnemyDatas();
        Effect = new EffectDatas();
        Weapon = new WeaponDatas();
    }
}