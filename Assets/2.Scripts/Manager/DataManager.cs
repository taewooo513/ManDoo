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
    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Skill = new SkillDatas();
        Mercenary = new MercenaryDatas();
        Enemy = new EnemyDatas();
    }

    public void Test()
    {
        Skill.Test();
        Mercenary.Test();
        Enemy.Test();
    }
}