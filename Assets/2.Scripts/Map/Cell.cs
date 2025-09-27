using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public BaseRoom Room;
    //public CellEvent cellEvent;
    //private Image icon

    public void Init(BaseRoom room = null, bool hasEvent = false)
    {
        if (room != null) Room = room;
        
        //if(hasEvent) cellEvent = GetRandomCellEvent();
    }

    public void EnterCell()
    {
        //if(cellEvent == null && Random.value > 0.5f){
        //    cellEvent = GetRandomCellEvent();
        //    Battle;
        //}
    }
    
    //private CellEvent GetRandomCellEvent(){
    //}

    public void EnterRoom()
    {
        //Room.EnterRoom();
    }
}
