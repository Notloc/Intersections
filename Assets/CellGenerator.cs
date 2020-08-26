using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour
{
    public Cell GenerateCell(Vector2Int coordinates, uint size)
    {
        GameObject folder = new GameObject(coordinates.ToString());
        Cell newCell = new Cell(coordinates, folder.transform, size);

        GenerateIntersection(newCell);

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
            if (errors.Count > 0)
                continue;

            // Place exit
            CellEntranceExit exit = null;
            int exitTries = 0;
            while (exitTries < 5)
            {
                exitTries++;
                exit = CellEntranceExit.Create(cell, entrance, errors);
                if (errors.Count > 0)
                {
                    entrance.Remove(cell);
                    break;
                }
            }
            if (errors.Count > 0)
                continue;

            int roadTries = 0;
            while (roadTries < 20)
            {
                errors.Clear();
                roadTries++;
                ConnectRoad(entrance, exit, errors);
                if (errors.Count > 0)
                    continue;

                break;
            }

            generateRoads++;
        }
    }

    private void ConnectRoad(CellEntranceExit entrance, CellEntranceExit exit, ErrorCollector errors)
    {

        // A*

        // Start build roads from each end
            // Place pseudo corner
                // branch to the lowest cost direction, prefering straight
                    // Path is complete when roads meet
                        // Do not optimize
                        // Trim excess from road that was "hit"


        // limit allowed intersection of roads
            // no "2 way" lanes
            // intersections must not intersect existing intersections or corners
                // unless the corner can be completely replaced by the intersection






    }
}
