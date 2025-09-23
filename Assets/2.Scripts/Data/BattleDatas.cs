using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDatas : BattleData
{
    public BattleData GetSkillData(int idx)
    {
        return BattleDataMap[idx];
    }
}
