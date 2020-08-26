using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    [SerializeField] uint cellSize = 10;
    [SerializeField] Grid grid = null;
    CellGrid cellGrid;

    private void Start()
    {
        cellGrid = new CellGrid(grid, cellSize);
    }

}
