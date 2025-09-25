using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    private MapGenerator _mapGenerator;
    private MapUI _mapUI;
    [SerializeField] private int index;

    private void Awake()
    {
        _mapUI = gameObject.AddComponent<MapUI>();
    }

    void Start()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        DataManager.Instance.Initialize();
        DataTable.MapData mapData = DataManager.Instance.Map.GetMapData(index);
        List<BaseRoom> rooms = _mapGenerator.GenerateMap(mapData);
        // Debug.Log(rooms.Count);
        // foreach (var item in rooms)
        // {
        //     Debug.Log(item+" "+ item.RoomLocation);
        // }
        UIManager.Instance.OpenUI<MapUI>();
        _mapUI.Init(rooms);
        _mapUI.GenerateMapUI();
    }
}
