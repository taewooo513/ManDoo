using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class EffectDatas : EffectData
{
    public EffectData GetEnemyData(int idx)
    {
        return EffectDataMap[idx];
    }
}
