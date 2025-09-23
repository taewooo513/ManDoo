using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor
{
    public List<Cell> CorridorCells;

    public void MakeCells()
    {
        for (int i = 0; i < 4; i++)
        {
            Cell cell = new Cell();
            CorridorCells.Add(cell);
        }
    }
}
