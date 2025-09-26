using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    private MapGenerator _mapGenerator;
    private MapUI _mapUI;
    [SerializeField] private int index;
    
    public List<BaseRoom> rooms;
    public INavigatable CurrentLocation;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        DataManager.Instance.Initialize();
        DataTable.MapData mapData = DataManager.Instance.Map.GetMapData(index);
        List<BaseRoom> rooms = _mapGenerator.GenerateMap(mapData);
        _mapUI = UIManager.Instance.OpenUI<MapUI>();
        _mapUI.Init(rooms);
        _mapUI.GenerateMapUI();
        CurrentLocation = rooms[0];
    }
    
    public void ChangeCurrentLocation(INavigatable currentLocation)
    {
        CurrentLocation = currentLocation;
    }
}
 