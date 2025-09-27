using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCollider : MonoBehaviour
{
    [SerializeField] private int index;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        MapManager.Instance.CurrentCorridor.CorridorCells[index].StartEvent();
    }
}
