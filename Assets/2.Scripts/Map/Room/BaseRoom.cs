using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom
{
    public Dictionary<RoomDirection, BaseRoom> connectedRooms;
    public Dictionary<RoomDirection, Corridor> corridors;
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    public string RoomLocation;//시작 지점으로부터의 방향을 뜻함
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

    public void SetRoomLocation(BaseRoom parentRoom, RoomDirection direction)
    {
        //???이런 것도 된다고?
        RoomLocation = parentRoom.RoomLocation + direction switch
        {
            RoomDirection.Up => "U",
            RoomDirection.Down => "D",
            RoomDirection.Left => "L",
            RoomDirection.Right => "R"
        };
    }
    public virtual void ExitRoom()
    {
    }
    public Corridor MakeConnection(BaseRoom room, RoomDirection direction)
    {
        connectedRooms.Add(direction, room);
        var corridor = new Corridor();
        corridors.Add(direction, corridor);
        corridor.MakeCells();

        return corridor;
    }

    public void ApplyConnection(BaseRoom room, RoomDirection direction, Corridor corridor)
    {
        connectedRooms.Add(direction, room);
        corridors.Add(direction, corridor);
    }
}
