using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public void PlayableCharacterSpawn(int id)
    {
        Vector3 pos = new Vector3(1, 0, 0);
        Vector3 add = new Vector3(-2, 0, 0);

        pos += add; //TODO : 위치 설정 제대로 해야됨 
        GameObject playableCharacter = Instantiate(Resources.Load<GameObject>(Constants.Player + "playableCharacter"), pos, Quaternion.identity);
        GameManager.Instance.AddPlayer(playableCharacter.GetComponent<BaseEntity>()); //게임매니저 리스트에 플레이어 추가
        playableCharacter.GetComponent<PlayableCharacter>().Init(id); //새로 만들어지는 프리팹에 플레이어 넣어줌
    }

    public void EnemySpawn(List<int> id)
    {
        Vector3 pos = new Vector3(-1, 0, 0);
        Vector3 add = new Vector3(2, 0, 0);

        for (int i = 0; i < id.Count; i++) //적 특정 위치에 생성
        {
            pos += add;
            GameObject enemy = Instantiate(Resources.Load<GameObject>(Constants.Enemy + "Enemy"), pos, Quaternion.identity);
            GameManager.Instance.AddEnemy(enemy.GetComponent<BaseEntity>()); //게임매니저에서 적 생성
            enemy.GetComponent<Enemy>().Init(id[i]); //소환하려는 적을 새로 만들어지는 프리팹에 넣어줌
        }
    }
}
