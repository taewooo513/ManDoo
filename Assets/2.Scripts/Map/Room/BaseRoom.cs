using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom
{
    public Dictionary<RoomDirection, BaseRoom> connectedRooms;
    public Dictionary<RoomDirection, Corridor> corridors;
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    public string RoomLocation;//시작 지점으로부터의 방향을 뜻함
    public Spawn spawn;
    
    public virtual void EnterRoom(int id)
    {
        if (spawn == null) //스폰이 null이면
        {
            GameObject spawnObject = new GameObject("Spawn"); //스폰 생성
            spawn = spawnObject.AddComponent<Spawn>(); //스폰 컴포넌트 챙겨오기
        }
        spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
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
