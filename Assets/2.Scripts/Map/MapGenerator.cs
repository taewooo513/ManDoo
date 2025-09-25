using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private List<BaseRoom> rooms = new();
    //private MapData _mapData;
    //private int _battleRoomCount;
    //private int _itemRoomCount;
  
    public List<BaseRoom> GenerateMap() //, MapData mapData
    {
        //-----Initialize-----//
        
        //_mapData = mapData;
        //int RoomCount = _mapData.roomCount;
        //_battleRoomCount = _mapData.battleRoomCount;
        //_itemRoomCount = _mapData.itemRoomCount;
        
        int roomCount = 10;
        //---Initialize End---//
        
        var recentlyListedRooms = new List<BaseRoom>();
        
        var startRoom = new StartRoom();
        rooms.Add(startRoom);
        GenerateRoom(startRoom, ref recentlyListedRooms, true);
        roomCount--;

        while (roomCount > 1)
        {
            List<BaseRoom> tempList = new List<BaseRoom>();
            foreach (var item in recentlyListedRooms)
            {
                if (roomCount <= 1) break;
                GenerateRoom(item, ref tempList);
                roomCount--;
            }
            if (roomCount <= 1) break;
            recentlyListedRooms.Clear();
            recentlyListedRooms.AddRange(tempList);
        }
        
        var bossRoom = recentlyListedRooms[UnityEngine.Random.Range(0, recentlyListedRooms.Count)];
        GenerateBossRoom(bossRoom);
        
        return rooms;
    }
    
    private void GenerateRoom(BaseRoom parentRoom, ref List<BaseRoom> recentlyListedRooms, bool isFirstRoom = false)
    {
        if (isFirstRoom)
        {
            //TODO: 타입 체크 가능한 리스트로 만들기
            int Random = UnityEngine.Random.Range(0, 4);
            RoomDirection direction = (RoomDirection)Random;
            //TODO: 방향 체크 가능한 리스트로 만들기
            Random = UnityEngine.Random.Range(1, 4);
            RoomType roomType = (RoomType)Random;
            
            switch (roomType)
            {
                case RoomType.Start:
                    break;
                case RoomType.Empty:
                    var EmptyRoom = new EmptyRoom();
                    recentlyListedRooms.Add(EmptyRoom);
                    rooms.Add(EmptyRoom);
                    ConnectRoom(parentRoom, EmptyRoom, direction);
                    break;
                case RoomType.Battle:
                    var BattleRoom = new BattleRoom();
                    //_battleRoomCount--;
                    recentlyListedRooms.Add(BattleRoom);
                    rooms.Add(BattleRoom);
                    ConnectRoom(parentRoom, BattleRoom, direction);
                    break;
                case RoomType.Item:
                    var itemRoom = new TreasureRoom();
                    //_itemRoomCount--;
                    recentlyListedRooms.Add(itemRoom);
                    rooms.Add(itemRoom);
                    ConnectRoom(parentRoom, itemRoom, direction);
                    break;
                default:
                    Debug.LogError("RoomType Error");
                    break;
            }
        }
        else
        {
            List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
            if (possibleDirection.Count == 0) return;
            int random = UnityEngine.Random.Range(0, possibleDirection.Count);

            for (int i = 0; i < random; i++)
            {
                RoomDirection direction = possibleDirection[UnityEngine.Random.Range(0, possibleDirection.Count)];
                possibleDirection.Remove(direction);
                List<RoomType> possibleRoomType = GetPossibleRoomType();
                RoomType randomRoomType = possibleRoomType[UnityEngine.Random.Range(0, possibleRoomType.Count)];
                switch (randomRoomType)
                {
                    case RoomType.Start:
                        Debug.LogError("RoomType Error");
                        break;
                    case RoomType.Empty:
                        var EmptyRoom = new EmptyRoom();
                        recentlyListedRooms.Add(EmptyRoom);
                        rooms.Add(EmptyRoom);
                        ConnectRoom(parentRoom, EmptyRoom, direction);
                        break;
                    case RoomType.Battle:
                        var BattleRoom = new BattleRoom();
                        //_battleRoomCount--;
                        recentlyListedRooms.Add(BattleRoom);
                        rooms.Add(BattleRoom);
                        ConnectRoom(parentRoom, BattleRoom, direction);
                        break;
                    case RoomType.Item:
                        var itemRoom = new TreasureRoom();
                        //_itemRoomCount--;
                        recentlyListedRooms.Add(itemRoom);
                        rooms.Add(itemRoom);
                        ConnectRoom(parentRoom, itemRoom, direction);
                        break;
                    default:
                        Debug.LogError("RoomType Error");
                        break;
                }
            }
        }
    }

    private void GenerateBossRoom(BaseRoom parentRoom)
    {
        List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
        if (possibleDirection.Count == 0) return;
        RoomDirection direction = possibleDirection[UnityEngine.Random.Range(0, possibleDirection.Count)];
        
        var BattleRoom = new BattleRoom();
        //_battleRoomCount--;
        rooms.Add(BattleRoom);
        ConnectRoom(parentRoom, BattleRoom, direction);
    }
    private void ConnectRoom(BaseRoom parentRoom, BaseRoom childRoom, RoomDirection direction)
    {
        var corridor = parentRoom.MakeConnection(childRoom, direction);
        childRoom.ApplyConnection(parentRoom, GetOppositeDirection(direction), corridor);
    }

    private RoomDirection GetOppositeDirection(RoomDirection direction)
    {
        return (RoomDirection)(((int)direction + 2) % 4);
    }

    private List<RoomDirection> GetPossibleDirection(BaseRoom room)
    {
        //TODO: 훨씬 깊게 탐색할 수 있어야함.
        List<RoomDirection> possibleDirection = new();
        if (room == null && room.connectedRooms.Count == 0) return null;
        for (int i = 0; i < 4; i++)
        {
            if (room.connectedRooms.ContainsKey((RoomDirection)i))
            {
                possibleDirection.Add((RoomDirection)i);
            }
        }
        return possibleDirection;
    }

    private List<RoomType> GetPossibleRoomType()
    {
        List<RoomType> possibleRoomType = new();
        //if(BattleRoomCount > 1)
        //possibleRoomType.Add(RoomType.Battle);
        possibleRoomType.Add(RoomType.Battle);
        //if(ItemRoomCount > 0)
        //possibleRoomType.Add(RoomType.Item);
        possibleRoomType.Add(RoomType.Item);
        possibleRoomType.Add(RoomType.Empty);
        return possibleRoomType;
    }
}