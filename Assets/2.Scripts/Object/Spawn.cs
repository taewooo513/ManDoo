using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private List<BaseEntity> _playerPositionList = new List<BaseEntity>(); //바뀐 플레이어 위치 받아오는 리스트
    
    public void PlayableCharacterCreate(int id) //캐릭터 생성
    {
        GameObject playableCharacter = Instantiate(Resources.Load<GameObject>(Constants.Player + "playableCharacter"));
        GameManager.Instance.AddPlayer(playableCharacter.GetComponent<BaseEntity>()); //게임매니저 리스트에 플레이어 추가
        playableCharacter.GetComponent<PlayableCharacter>().Init(id); //새로 만들어지는 프리팹에 플레이어 넣어줌
    }

    public void PlayableCharacterSpawn() //캐릭터 스폰(위치 지정)
    {
        Vector3 pos = new Vector3(1, 0, 0);
        Vector3 add = new Vector3(2, 0, 0);
        
        for (int i = 0; i < GameManager.Instance.PlayableCharacter.Count; i++) //현재 데리고 있는 플레이어 리스트만큼 카운트
        {
            pos -= add;
            if(_playerPositionList == null) //배틀매니저에서 받아온 플레이어 위치 정보가 없다면
            {
                GameManager.Instance.PlayableCharacter[i].transform.position = pos; //게임매니저에 있는 플레이어 호출
            }
            else if (_playerPositionList != null)
            {
                BaseEntity playerPostion = _playerPositionList[i]; //전투하면서 바뀐 플레이어들 위치
                playerPostion.transform.position = pos; //프리팹 위치별로 화면에 띄우기
                GameManager.Instance.PlayableCharacter[i] = _playerPositionList[i]; //현재 플레이어 정보(hp, 위치 등) 게임매니저에 반영
            }
        }
    }
    
    public void PlayableCharacterPosition(List<BaseEntity> playerPositionList) //캐릭터 스폰(위치 지정)
    {
        _playerPositionList = playerPositionList;
    }

    public void EnemySpawn(List<int> id) //적 생성
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
