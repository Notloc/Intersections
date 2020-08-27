﻿using System.Collections;
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
                ConnectRoad(cell, entrance, exit, errors);
                errors.GoBoom();

                break;
            }

            generateRoads++;
        }
    }

    private void ConnectRoad(Cell cell, CellEntranceExit entrance, CellEntranceExit exit, ErrorCollector errors)
    {

        Path path = PathGenerator.GeneratePath(cell, entrance, exit, errors);

        if (errors.Count > 0)
        {
            Debug.LogError(errors.GetErrorMessages());
            errors.Clear();
            drawPaths.Clear();
        }
        DisplayPath(path);
                
    }

    private void DisplayPath(Path path)
    {
        drawPaths.Add(path);
    }

    private void OnDrawGizmos()
    {
        foreach (Path drawPath in drawPaths)
        {
            for (int i = 0; i < drawPath.nodes.Count; i++)
            {
                Node node = drawPath.nodes[i];
                if (i == 0)
                    Gizmos.color = Color.green;
                else if (i == drawPath.nodes.Count - 1)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;

                Gizmos.DrawCube(new Vector3(node.position.x, node.position.y, 0f), Vector3.one);
            }
        }
    }
}
