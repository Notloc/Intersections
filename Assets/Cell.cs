using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual cell, 
/// </summary>
public class Cell
{
    private Transform transform;
    private int[,] blockGrid;
    private Vector2Int coordinates;


    public uint Size { get; private set; }

    public Cell(Vector2Int coordinates, Transform transform, uint size)
    {
        this.coordinates = coordinates;
        this.transform = transform;
        Size = size;
        CreateGrid(size);
    }

    private void CreateGrid(uint size)
    {
        blockGrid = new int[size, size];
    }

    public int Get(int x, int y)
    {
        return blockGrid[x, y];
    }

    public void Set(int x, int y, BlockType block) { Set(x, y, (int)block); }
    private void Set(int x, int y, int blockId)
    {
        blockGrid[x, y] = blockId;
    }

    public void Clear()
    {
        blockGrid = new int[Size,Size];
    }
}
