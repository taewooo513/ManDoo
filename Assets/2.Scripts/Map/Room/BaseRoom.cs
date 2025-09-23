using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    //데이터테이블에 있는 id 값 넣고, 리소스에 있는 프리팹을 게임오브젝트에 넣어주는 작업 필요함
    
    public virtual void EnterRoom()
    {
        // int[] id = { 0 }; //현재 용병 리스트가 계속 바뀔 수 있으니, 방에 들어갈 때마다
        // for (int i = 0; i < BattleManager.Instance.PlayableCharacters.Count; i++) //현재 리스트가 가진 id 값들 체크 
        // {
        //     id[i]  = BattleManager.Instance.PlayableCharacters[i].id;
        // }
        
        //id[i]가 프리팹과 같은 id를 가졌으면
        //특정 위치에 플레이어 소환하기
    }

    public virtual void ExitRoom()
    {
    }
}
