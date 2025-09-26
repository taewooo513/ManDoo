using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class BaseRoom : INavigatable
{
    public Dictionary<RoomDirection, BaseRoom> connectedRooms = new();
    public Dictionary<RoomDirection, Corridor> corridors = new();
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    public string RoomLocation;//시작 지점으로부터의 방향을 뜻함
    public Spawn spawn;
    private RoomUI _roomUI;
    protected int roomId;
    public bool isInteract = false; //이거 베이스룸한테 주면 접근 어떻게하지..?
    
    public virtual void EnterRoom()
    {
        if (spawn == null) //스폰이 null이면
        {
            GameObject spawnObject = new GameObject("Spawn"); //스폰 생성
            spawn = spawnObject.AddComponent<Spawn>(); //스폰 컴포넌트 챙겨오기
        }
        spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
        _roomUI.ActivateIcon();
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

    public virtual void Init(int id) //전투 보상 등등 기본값 세팅 todo : 통로/방쪽에서 호출해줘야 됨
    {
        roomId = id;
    }

    public Corridor MakeConnection(BaseRoom room, RoomDirection direction)
    {
        connectedRooms.Add(direction, room);
        var corridor = new Corridor();
        corridor.Init(this, room, direction);
        corridors.Add(direction, corridor);
        corridor.MakeCells();

        return corridor;
    }

    public void ApplyConnection(BaseRoom room, RoomDirection direction, Corridor corridor)
    {
        connectedRooms.Add(direction, room);
        corridors.Add(direction, corridor);
    }

    public bool IsConnected(BaseRoom room, RoomDirection direction)
    {
        if (connectedRooms.TryGetValue(direction, out BaseRoom connectedRoom))
        {
            return connectedRoom == room;
        }

        return false;
    }
    
    public void SetRoomUI(RoomUI roomUI)
    {
        _roomUI = roomUI;
    }

    public void Travel(INavigatable room)
    {
        foreach (var item in connectedRooms.Keys)
        {
            if (connectedRooms[item] == room)
            {
                MapManager.Instance.ChangeCurrentLocation(corridors[item]);
                break;
            }
        }
    }
}
