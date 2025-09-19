using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    [SerializeField] private List<BaseEntity> player;
    [SerializeField] private List<BaseEntity> enemies;
    void Start()
    {
        BattleManager.Instance.BattleStartTrigger(player,enemies);
    }
    
}
