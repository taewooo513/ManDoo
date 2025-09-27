using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    private MapGenerator _mapGenerator;
    private MapUI _mapUI;
    [SerializeField] private int index;

    public List<BaseRoom> rooms = new();
    public INavigatable CurrentLocation;

    public Corridor CurrentCorridor;
    protected override void Awake()
    {
        base.Awake();
        _mapGenerator = GetComponent<MapGenerator>();
        if(_mapGenerator == null) _mapGenerator = gameObject.AddComponent<MapGenerator>();
            
    }

    private void Start()
    {
        
        
    }

    public void Test()
    {
        DataManager.Instance.Initialize();
        DataTable.MapData mapData = DataManager.Instance.Map.GetMapData(3);
        rooms = _mapGenerator.GenerateMap(mapData);
        _mapUI = UIManager.Instance.OpenUI<MapUI>();
        _mapUI.Init(rooms);
        _mapUI.GenerateMapUI();
        CurrentLocation = rooms[0];
        rooms[0].EnterRoom();
    }

    private BaseRoom FindDestinationRoom(INavigatable destinationRoom)
    {
        if (CurrentLocation is BaseRoom room && destinationRoom is Corridor corridor)
        {
            foreach (var item in room.corridors.Keys)
            {
                if (corridor == room.corridors[item])
                {
                    Debug.Log("Find " + room.connectedRooms[item]);
                    CurrentCorridor = corridor;
                    return room.connectedRooms[item];
                }
            }
        }
        return null;
    }
    public void ChangeCurrentLocation(INavigatable destination)
    {
        if (CurrentLocation is BaseRoom)
        {
            BaseRoom destinationRoom = FindDestinationRoom(destination);
            CurrentLocation = destination;
            Debug.Log(destination);
            DeActivateButtonUI();
            CurrentLocation.Enter(destinationRoom);
        }
        else if(CurrentLocation is Corridor)
        {
            CurrentLocation = destination;
            DeActivateButtonUI();
            CurrentLocation.Enter();
        }
    }

    public void DeActivateButtonUI()
    {
        foreach (var item in rooms)
        {
            item.RoomUI.DeactivateButton();
        }
    }
}
 