using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillageRoom : BaseRoom //마을, 보스방. 인데 사실상 battleRoom에 있는것도 전부 포함돼서, BattleRoom 상속받는게 더 나을듯
{
    public override void EnterRoom()
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
    }

    public override void Init(int id)
    {
        base.Init(id); //적 생성, 드랍템 초기화 등
    }

    public override void ExitRoom()
    {
        base.ExitRoom(); //떨어지는 보상 아이템
        List<BaseEntity> playableCharacter;
        for (int i = 0; i < GameManager.Instance.PlayableCharacter.Count; i++) //살아있는 캐릭터 리스트 가져오기
        {
            playableCharacter = GameManager.Instance.PlayableCharacter;
        }
        //탐색한 방의 수. 방 관리하는 쪽에서 count로 +1씩 해줘야 됨.
        //파티 초상화.
    }
}
