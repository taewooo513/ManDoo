using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmcRoom : BaseRoom
{
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
    }
    public override void OnEventEnded()
    {
        base.OnEventEnded();
    }
}
