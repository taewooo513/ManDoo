using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public void PlayableCharacterSpawn()
    {
    }

    public void EnemySpawn(List<int> id)
    {
        foreach (var item in id)
        {
            Debug.Log(item);
        }
        Vector3 pos = new Vector3(1, -1, 0);
        Vector3 add = new Vector3(0, 2, 0);
        
        for (int i = 0; i < id.Count; i++) //적 특정 위치에 생성
        {
            pos += add;
            GameObject enemy = Instantiate(Resources.Load<GameObject>(Constants.Enemy + "Enemy"), pos, Quaternion.identity);
            GameManager.Instance.AddEnemy(enemy.GetComponent<BaseEntity>()); //게임매니저에서 적 생성
            enemy.GetComponent<Enemy>().Init(id[i]); //소환하려는 적을 새로 만들어지는 프리팹에 넣어줌
        }
    }
}
