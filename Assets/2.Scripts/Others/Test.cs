using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BattleRoom battleRoom;
    public StartRoom startRoom;
    
    void Start()
    {
        DataManager.Instance.Initialize();
        battleRoom = new BattleRoom();
        battleRoom.EnterRoom();
        
        startRoom = new StartRoom();
        startRoom.EnterRoom();
        
        //UIManager.Instance.OpenUI<InGameBattleStartUI>(); //ui 테스트
        GameManager.Instance.StartBattle();
    }
}
