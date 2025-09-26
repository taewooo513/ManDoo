using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIContentGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject corridorPrefab;
    private Transform _content;
    private List<BaseRoom> _rooms;
    private List<(BaseRoom, int, int)> _roomPositions = new();
    private int _xPos = 300;
    private int _yPos = 300;
    public void Init(Transform content, List<BaseRoom> rooms)
    {
        _content = content;
        _rooms = rooms;
    }

    public void GenerateMapUI()
    {
        FillRoomPositionList();
        foreach (var item in _roomPositions)
        {
            var go = Instantiate(roomPrefab, _content);
            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(_xPos * item.Item2, _yPos*item.Item3);
            }
        }
    }

    private void FillRoomPositionList()
    {
        (int, int) locationInt = new();
        foreach (var item in _rooms)
        {
            locationInt = GetRoomLocation(item);
            _roomPositions.Add((item, locationInt.Item1, locationInt.Item2));
        }
    }
    
    private (int, int) GetRoomLocation(BaseRoom room)
    {
        (int, int) locationInt = new();
        char[] possibleLocationArray = room.RoomLocation.ToCharArray();

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
}
