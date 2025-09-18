using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenerateWeightListUtility //타겟 가중치 리스트
{
    public static List<float> weightsList = new List<float>();

    public static void CombineWeights(float weight)
    {
        weightsList.Add(weight);
    }

    public static List<float> GetWeights()
    {
        return weightsList;
    }

    public static void Clear()
    {
        weightsList.Clear();
    }
}