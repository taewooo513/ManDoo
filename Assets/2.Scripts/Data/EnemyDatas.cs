using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class EnemyDatas : EnemyData
{
    public EnemyData GetEnemyData(int idx)
    {
        return EnemyDataMap[idx];
    }
}
