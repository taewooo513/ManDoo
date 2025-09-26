using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDatas : RewardData
{
    public RewardData GetRewardData(int idx)
    {
        return RewardDataMap[idx];
    }
}