using System.Collections.Generic;
using UnityEngine;
using System;

public static class RandomizeUtility
{
    public static int TryGetRandomPlayerIndexByWeight(List<float> weights)
    {
        float total = 0f;
        int playerIndex = -1;

        foreach (var weight in weights)
            total += weight;

        float rand = UnityEngine.Random.value * total;

        int i = 0;
        foreach (var weight in weights)
        {
            rand -= weight;

            if (rand <= 0f)
            {
                playerIndex = i;
                break;
            }
            i++;
        }
        return playerIndex;
    }
}
