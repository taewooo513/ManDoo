using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    //TODO : 상위 cs에서 레이캐스트+좌클릭 시 이쪽의 OnTrapRelease 실행하도록 하기
    //함정이 지나가다가 자동으로 발동하는 거라서, 클릭 시 수행되는 거는 해제만 시켜주는 것.
    
    public void OnTrapRelease() //클릭해서 함정 해제하기
    {
        //if 아이템의 '함정 해제 도구'를 사용중이라면
        //100%로 함정 해제
        
        //else 아무것도 없다면
        //50%로 함정 해제
    }

    public void TrapActive(BaseEntity suffered) //TODO : 통로쪽에서 확률적으로 호출하는 부분 넣어줘야 함
    {
        int sufferedCurrentHp = suffered.entityInfo.currentHp;
        double percentage = suffered.entityInfo.maxHp * 0.1;

        //
        sufferedCurrentHp -= (int)percentage; //함정 발동
        
        if (sufferedCurrentHp <= 0)
        {
            BattleManager.Instance.EntityDead(suffered);
        }
    }
}
