using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class EnemyDatas : EnemyData
{
    public void Test()
    {
        Debug.Log(EnemyDataList.Count);
    }

    public EnemyData GetEnemyData(int idx)
    {
        return EnemyDataMap[idx];
    }
}
