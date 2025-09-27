using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    private RoomBackground _background;
    private SpriteRenderer _spriteRenderer;
    private CorridorBackground _corridorBackground;
    private void Start()
    {
        
    }
    public void ChangeBackground(INavigatable location)
    {
        if (location is BaseRoom room)
        {
            if(_background == null) InstantiateBackgrounds();
            _background.gameObject.SetActive(true);
            _corridorBackground.gameObject.SetActive(false);
            _spriteRenderer.sprite = Resources.Load<Sprite>(room.GetBackgroundPath());
        }
        else
        {
            if(_corridorBackground == null) InstantiateBackgrounds();
            _corridorBackground.gameObject.SetActive(true);
            _background.gameObject.SetActive(false);
            _corridorBackground.gameObject.transform.position = new Vector3(10, 0, 0);
        }
    }

    private void InstantiateBackgrounds()
    {
        var go = Instantiate(Resources.Load<GameObject>("Prefabs/Map/RoomBackground"));
        _background = go.GetComponent<RoomBackground>();
        _spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
        var go2 = Instantiate(Resources.Load<GameObject>("Prefabs/Map/CorridorBackground"));
        _corridorBackground = go2.GetComponent<CorridorBackground>();
        _background.gameObject.SetActive(false);
        _corridorBackground.gameObject.SetActive(false);
    }
}
