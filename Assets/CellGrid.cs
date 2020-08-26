using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// A grid of cells, representing the entire play space
/// </summary>
public class CellGrid
{
    Grid grid;
    Dictionary<Vector2Int, Cell> cellGrid = new Dictionary<Vector2Int, Cell>();
    uint size;

    public CellGrid(Grid grid, uint size)
    {
        this.grid = grid;
        this.size = size;
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void AddCell(Cell cell, Vector2Int coordinates)
    {
        if (cellGrid.ContainsKey(coordinates))
            throw new System.Exception("The cell already exists.");

        cellGrid.Add(coordinates, cell);
    }
}
