using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableDatas : ConsumableData
{
    public ConsumableData GetSkillData(int idx)
    {
        return ConsumableDataMap[idx];
    }
}
