using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private readonly List<BaseRoom> _rooms = new();
    private DataTable.MapData _mapData;
    private int _battleRoomCount;
    private int _itemRoomCount;
    private int _roomCount;
    private int _shopRoomCount;
    private int _pmcRoomCount;
    private int _emptyRoomCount;
    private int _additionalPathCount;
    private float _pathProb;
  
    public List<BaseRoom> GenerateMap(DataTable.MapData mapData)
    {
        //-----Initialize-----//
        
        _mapData = mapData;
        _roomCount = _mapData.totalCnt;
        _battleRoomCount = _mapData.battleCnt;
        _itemRoomCount = _mapData.itemCnt;
        _shopRoomCount = _mapData.shopCnt;
        _pmcRoomCount = _mapData.pmcCnt;
        _emptyRoomCount = _mapData.emptyCnt;
        _additionalPathCount = _mapData.additionalPathCnt;
        _pathProb = _mapData.pathProp;
        
        //---Initialize End---//
        
        var recentlyListedRooms = new List<BaseRoom>();
        
        var startRoom = new StartRoom();
        _rooms.Add(startRoom);
        _roomCount--;
        startRoom.RoomLocation = "";
        GenerateRoom(startRoom, ref recentlyListedRooms, true);
        //Debug.Log(startRoom + " " + startRoom.RoomLocation);

        while (_roomCount > 1)
        {
            if (recentlyListedRooms.Count == 0)
            {
                Debug.LogError("Room Count Error");
                break;
            }
            List<BaseRoom> tempList = new List<BaseRoom>();
            foreach (var item in recentlyListedRooms)
            {
                if (_roomCount <= 1) break;
                GenerateRoom(item, ref tempList);
            }
            recentlyListedRooms.Clear();
            recentlyListedRooms.AddRange(tempList);
            if (_roomCount <= 1) break;
        }
        
        var bossRoom = recentlyListedRooms[Random.Range(0, recentlyListedRooms.Count)];
        GenerateBossRoom(bossRoom);
        return _rooms;
    }

    private BaseRoom GetConnectableRoom(BaseRoom room)
    {
        BaseRoom connectableRoom = room;
        while (connectableRoom.RoomLocation != "")
        {
            List<RoomDirection> possibleDirection = GetPossibleDirection(connectableRoom);
            if (possibleDirection.Count != 0) return connectableRoom;
            char[] possibleLocationArray = connectableRoom.RoomLocation.ToCharArray();
            for (int i = 0; i < possibleLocationArray.Length; i++)
            {
                connectableRoom = FindRoomWithString(connectableRoom.RoomLocation.Remove(connectableRoom.RoomLocation.Length - 1, 1));
            }
        }

        List<BaseRoom> tempList = new List<BaseRoom>();
        foreach (var item in _rooms)
        {
            if (GetPossibleDirection(item).Count != 0) tempList.Add(item);
        }

        return tempList[Random.Range(0, tempList.Count)];
    }

    private BaseRoom FindRoomWithString(string roomLocation)
    {
        foreach (var item in _rooms)
        {
            if (item.RoomLocation == roomLocation) return item;
        }
        return null;
    }

    private BaseRoom FindRoomWithLocation((int, int) location)
    {
        foreach (var item in _rooms)
        {
            if (GetRoomLocation(item.RoomLocation) == location) return item;
        }
        return null;   
    }

    private void GenerateRoom(BaseRoom parentRoom, ref List<BaseRoom> recentlyListedRooms, bool isFirstRoom = false)
    {
        List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
        if (possibleDirection.Count == 0 && !isFirstRoom)
        {
            parentRoom = GetConnectableRoom(parentRoom);
            possibleDirection = GetPossibleDirection(parentRoom);
        }
        int random = isFirstRoom ? 1 : Random.Range(1, possibleDirection.Count + 1);

        for (int i = 0; i < random; i++)
        {
            if (_roomCount <= 1) break;
            RoomDirection direction = possibleDirection[Random.Range(0, possibleDirection.Count)];
            possibleDirection.Remove(direction);
            List<RoomType> possibleRoomType = GetPossibleRoomType();
            RoomType randomRoomType = possibleRoomType[Random.Range(0, possibleRoomType.Count)];
            switch (randomRoomType)
            {
                case RoomType.Start:
                    Debug.LogError("RoomType Error");
                    break;
                case RoomType.Empty:
                    var emptyRoom = new EmptyRoom();
                    _emptyRoomCount--;
                    recentlyListedRooms.Add(emptyRoom);
                    _rooms.Add(emptyRoom);
                    ConnectRoom(parentRoom, emptyRoom, direction);
                    //Debug.Log(emptyRoom + " " + emptyRoom.RoomLocation);
                    break;
                case RoomType.Battle:
                    var battleRoom = new BattleRoom();
                    _battleRoomCount--;
                    recentlyListedRooms.Add(battleRoom);
                    _rooms.Add(battleRoom);
                    ConnectRoom(parentRoom, battleRoom, direction);
                    //Debug.Log(battleRoom + " " + battleRoom.RoomLocation);
                    break;
                case RoomType.Item:
                    var itemRoom = new ItemRoom();
                    _itemRoomCount--;
                    recentlyListedRooms.Add(itemRoom);
                    _rooms.Add(itemRoom);
                    ConnectRoom(parentRoom, itemRoom, direction);
                    //Debug.Log(itemRoom + " " + itemRoom.RoomLocation);
                    break;
                case RoomType.Shop:
                    var shopRoom = new ShopRoom();
                    _shopRoomCount--;
                    recentlyListedRooms.Add(shopRoom);
                    _rooms.Add(shopRoom);
                    ConnectRoom(parentRoom, shopRoom, direction);
                    //Debug.Log(shopRoom + " " + shopRoom.RoomLocation);
                    break;
                case RoomType.PMC:
                    var pmcRoom = new PmcRoom();
                    _pmcRoomCount--;
                    recentlyListedRooms.Add(pmcRoom);
                    _rooms.Add(pmcRoom);
                    ConnectRoom(parentRoom, pmcRoom, direction);
                    //Debug.Log(pmcRoom + " " + pmcRoom.RoomLocation);
                    break;
                default:
                    Debug.LogError("RoomType Error");
                    break;
            }

            _roomCount--;
        }
    }

    private void GenerateBossRoom(BaseRoom parentRoom)
    {
        List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
        if (possibleDirection.Count == 0) return;
        RoomDirection direction = possibleDirection[Random.Range(0, possibleDirection.Count)];
        
        var villageRoom = new VillageRoom();
        _rooms.Add(villageRoom);
        ConnectRoom(parentRoom, villageRoom, direction);
        //Debug.Log(villageRoom + " " + villageRoom.RoomLocation);
    }
    private void ConnectRoom(BaseRoom parentRoom, BaseRoom childRoom, RoomDirection direction, bool isConnectCorridorOnly = false)
    {
        var corridor = parentRoom.MakeConnection(childRoom, direction);
        childRoom.ApplyConnection(parentRoom, GetOppositeDirection(direction), corridor);
        if (isConnectCorridorOnly)
        {
            //Debug.Log("Corridor between " + parentRoom + "(" + parentRoom.RoomLocation + ") and " + childRoom + "(" + childRoom.RoomLocation +") has been connected");
            _additionalPathCount--;
        }
        if(!isConnectCorridorOnly) childRoom.SetRoomLocation(parentRoom, direction);
    }

    private RoomDirection GetOppositeDirection(RoomDirection direction)
    {
        return (RoomDirection)(((int)direction + 2) % 4);
    }

    private List<RoomDirection> GetPossibleDirection(BaseRoom room)
    {
        //TODO: 훨씬 깊게 탐색할 수 있어야함. - 완료?
        List<RoomDirection> possibleDirection = new();
        
        (int, int) location = GetRoomLocation(room.RoomLocation);
        if (IsLocationPossible((location.Item1 + 1, location.Item2)))
        {
            possibleDirection.Add(RoomDirection.Right);
        }
        else
        {
            BaseRoom locatedRoom = FindRoomWithLocation((location.Item1 + 1, location.Item2));
            if(locatedRoom != null && _additionalPathCount > 0 && CalculateProbability() && !room.IsConnected(locatedRoom, RoomDirection.Right)) ConnectRoom(room, locatedRoom, RoomDirection.Right, true);
        }

        if (IsLocationPossible((location.Item1 - 1, location.Item2)))
        {
            possibleDirection.Add(RoomDirection.Left);
        }
        else
        {
            BaseRoom locatedRoom = FindRoomWithLocation((location.Item1 - 1, location.Item2));
            if(locatedRoom != null && _additionalPathCount > 0 && CalculateProbability() && !room.IsConnected(locatedRoom, RoomDirection.Left)) ConnectRoom(room, locatedRoom, RoomDirection.Left, true);
        }

        if (IsLocationPossible((location.Item1, location.Item2 + 1)))
        {
            possibleDirection.Add(RoomDirection.Up);
        }
        else
        {
            BaseRoom locatedRoom = FindRoomWithLocation((location.Item1, location.Item2 + 1));
            if(locatedRoom != null && _additionalPathCount > 0 && CalculateProbability() && !room.IsConnected(locatedRoom, RoomDirection.Up)) ConnectRoom(room, locatedRoom, RoomDirection.Up, true);
        }

        if (IsLocationPossible((location.Item1, location.Item2 - 1)))
        {
            possibleDirection.Add(RoomDirection.Down);
        }
        else
        {
            BaseRoom locatedRoom = FindRoomWithLocation((location.Item1, location.Item2 - 1));
            if(locatedRoom != null && _additionalPathCount > 0 && CalculateProbability() && !room.IsConnected(locatedRoom, RoomDirection.Down)) ConnectRoom(room, locatedRoom, RoomDirection.Down, true);
        }
        return possibleDirection;
    }

    private bool CalculateProbability()
    {
        if (_pathProb > Random.value)
        {
            _pathProb = _mapData.pathProp;
            return true;
        }

        _pathProb *= 1.5f;
        return false;
    }
    private List<RoomType> GetPossibleRoomType()
    {
        List<RoomType> possibleRoomType = new();
        if(_battleRoomCount > 0) possibleRoomType.Add(RoomType.Battle);
        if(_itemRoomCount > 0) possibleRoomType.Add(RoomType.Item);
        if(_shopRoomCount > 0) possibleRoomType.Add(RoomType.Shop);
        if(_pmcRoomCount > 0) possibleRoomType.Add(RoomType.PMC);
        if(_emptyRoomCount > 0) possibleRoomType.Add(RoomType.Empty);
        return possibleRoomType;
    }

    private (int, int) GetRoomLocation(string roomLocation)
    {
        (int, int) locationInt = new();
        char[] possibleLocationArray = roomLocation.ToCharArray();

        foreach (var item in possibleLocationArray)
        {
            switch (item)
            {
                case 'L':
                    locationInt.Item1--;
                    break;
                case 'R':
                    locationInt.Item1++;
                    break;
                case 'U':
                    locationInt.Item2++;
                    break;
                case 'D':
                    locationInt.Item2--;
                    break;
            }
        }
        return locationInt;
    }

    private bool IsLocationPossible((int, int) targetLocationInt)
    {
        foreach (var item in _rooms)
        {
            if(GetRoomLocation(item.RoomLocation) == targetLocationInt)
            {
                return false;
            }
        }
        return true;
    }
}