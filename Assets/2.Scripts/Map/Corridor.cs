using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : INavigatable
{
    public List<Cell> CorridorCells = new();
    public BaseRoom RoomA;
    public BaseRoom RoomB;
    public bool IsAlreadyMade;
    public RoomDirection Direction;
    public BaseRoom DestinationRoom;
    public bool IsAlreadyVisited;
    public void Enter(BaseRoom room = null)
    {
        BackgroundManager.Instance.ChangeBackground(this);
        EnterCorridor(room);
    }
    public void EnterCorridor(BaseRoom room)
    {
        if (!IsAlreadyVisited)
        {
            MapManager.Instance.CorridorVisitedCount++;
            IsAlreadyVisited = true;
        }
        if (room == RoomA)
        {
            DestinationRoom = RoomA;
        }
        else
        {
            DestinationRoom = RoomB;
        }
        //Test();
    }

    private void Test()
    {
        ExitCorridor();
    }
    public void ExitCorridor()
    {
        Debug.Log("Exit Corridor");
        Travel(DestinationRoom);
    }
    public void MakeCells()
    {
        for (int i = 0; i < 4; i++)
        {
            Cell cell = new Cell();
            CorridorCells.Add(cell);
        }
        CellInit();
    }

    private void CellInit()
    {
        int random = Random.Range(0, 3);
        List<int> randomEvent = new List<int>();
        
        if (random != 0)
        { 
            List<int> tempList = new List<int>();
            for (int i = 0; i < CorridorCells.Count; i++)
            {
                tempList.Add(i);
            }
            
            for (int i = 0; i < random; i++)
            {
                int temp = Random.Range(0, tempList.Count);
                randomEvent.Add(tempList[temp]);
                tempList.RemoveAt(temp);
            }
        }

        for (int i = 0; i < CorridorCells.Count; i++)
        {
            if (i == 0)
            {
                CorridorCells[i].Init(RoomA, randomEvent.Contains(i));
            }
            else if (i == 3)
            {
                CorridorCells[i].Init(RoomB, randomEvent.Contains(i));
            }
            else
            {
                CorridorCells[i].Init(null, randomEvent.Contains(i));
            }
        }
    }
    public void Init(BaseRoom roomA, BaseRoom roomB, RoomDirection direction)
    {
        RoomA = roomA;
        RoomB = roomB;
        Direction = direction;
    }

    public void Travel(INavigatable location)
    {
        MapManager.Instance.ChangeCurrentLocation(location);
    }
}
