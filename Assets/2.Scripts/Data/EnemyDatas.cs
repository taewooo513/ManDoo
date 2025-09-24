using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class EnemyDatas : EnemyData
{
    public EnemyData GetEnemyData(int idx)
    {
        foreach (var ad in EnemyDataMap)
        {
            Debug.Log(ad.Key);
        }
        return EnemyDataMap[idx];
    }
}
