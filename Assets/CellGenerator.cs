using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    private List<Path> drawPaths = new List<Path>();

    public Cell GenerateCell(Vector2Int coordinates, uint size)
    {
        GameObject folder = new GameObject(coordinates.ToString());
        Cell newCell = new Cell(coordinates, folder.transform, size);

        GenerateIntersection(newCell);
        GenerateGrass(newCell);

        return newCell;
    }

    private void GenerateIntersection(Cell cell)
    {
        int roadCount = Random.Range(3, 4);

        int generateRoads = 0;
        uint width = 2;

        ErrorCollector errors = new ErrorCollector();

        int tries = 0;
        // Create new paths
        while (generateRoads < roadCount && tries < 25)
        {
            errors.Clear();
            tries++;

            // Place entrance
            CellEntranceExit entrance = CellEntranceExit.Create(cell, width, errors);
            errors.GoBoom();

            // Place exit
            CellEntranceExit exit = null;
            int exitTries = 0;
            while (exitTries < 5)
            {
                exitTries++;
                exit = CellEntranceExit.Create(cell, entrance, errors);
                errors.GoBoom();
                break;
                //if (errors.Count > 0)
                //{
                //  entrance.Remove(cell);
                //break;
                //}
            }
            errors.GoBoom();

            int roadTries = 0;
            while (roadTries < 20)
            {
                errors.Clear();
                roadTries++;
                CreateRoad(cell, entrance, exit, errors);
                errors.GoBoom();

                break;
            }

            generateRoads++;
        }
    }

    private void CreateRoad(Cell cell, CellEntranceExit entrance, CellEntranceExit exit, ErrorCollector errors)
    {

        Path path = PathGenerator.GeneratePath(cell, entrance, exit, errors);
        errors.GoBoom();

        Road road = new Road(path);
        cell.AddRoad(road);
    }

    private void GenerateGrass(Cell cell)
    {
        for (int x = 0; x < cell.Size; x++)
        {
            for (int y = 0; y < cell.Size; y++)
            {
                TileType block = cell.Get(x, y);
                if (block == TileType.NONE)
                    cell.Set(x,y, TileType.GRASS);
            }
        }
    }
}
