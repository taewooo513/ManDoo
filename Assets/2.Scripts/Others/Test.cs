using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BattleRoom battleRoom;
    
    void Awake()
    {
        DataManager.Instance.Initialize();
        battleRoom = new BattleRoom();
        battleRoom.EnterRoom();
    }
}
