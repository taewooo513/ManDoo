using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public void PlayableCharacterSpawn()
    {
    }

    public void EnemySpawn(int id)
    {
        for (int i = 0; i < 4; i++)
        {
            //Instantiate(enemyPrefabs, pos, ind).GetComponent<Enemy>().Init(id);
        }
    }
}
