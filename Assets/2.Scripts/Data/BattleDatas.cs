using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDatas : BattleData
{
    public BattleData GetBattleData(int idx)
    {
        return BattleDataMap[idx];
    }
}
