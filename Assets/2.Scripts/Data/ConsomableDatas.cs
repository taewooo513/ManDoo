using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsomableDatas : ConsomableData
{
    public ConsomableData GetSkillData(int idx)
    {
        return ConsomableDataMap[idx];
    }
}
