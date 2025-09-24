using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BattleRoom battleRoom;
    
    void Start()
    {
        DataManager.Instance.Initialize();
        battleRoom = new BattleRoom();
        battleRoom.EnterRoom();
        
        //UIManager.Instance.OpenUI<InGameBattleStartUI>(); //ui 테스트
    }
}
