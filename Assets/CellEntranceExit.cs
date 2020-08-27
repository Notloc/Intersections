using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class CellEntranceExit
{
    private const int EDGE_MARGIN = 2;

    public Direction Side { get; private set; }
    public uint Width { get; private set; }
    public CellEntranceExit Other { get; set; }

    public Vector2Int Position { get; private set; }

    private CellEntranceExit() {}

    private List<Vector2Int> coordinates = new List<Vector2Int>();

    public void Remove(Cell cell)
    {
        foreach (Vector2Int coord in coordinates)
            cell.Set(coord.x, coord.y, TileType.NONE);
    }















    public static CellEntranceExit Create(Cell cell, uint width, ErrorCollector errors)
    {
        CellEntranceExit entrance = new CellEntranceExit();

        entrance.Width = width;
        int tries = 0;
        entrance.Side = RandomDirection();
        while (!EntranceFits(cell, entrance))
        {
            if (tries > 10)
            {
                errors.Add(new System.Exception("Unable to fit the entrance."));
                return null;
            }
            entrance.Side = RandomDirection();
            tries++;
        }

        PositionRandomly(cell, entrance, errors);
        return entrance;
    }

    public static CellEntranceExit Create(Cell cell, CellEntranceExit otherSide, ErrorCollector errors)
    {
        CellEntranceExit entranceExit = new CellEntranceExit();

        entranceExit.Other = otherSide;
        otherSide.Other = entranceExit;

        entranceExit.Width = otherSide.Width;
        int tries = 0;
        entranceExit.Side = RandomDirection(otherSide.Side);
        while (!EntranceFits(cell, entranceExit))
        {
            if (tries > 10)
            {
                errors.Add(new System.Exception("Unable to fit the EntranceExit."));
                return null;
            }
            entranceExit.Side = RandomDirection();
            tries++;
        }

        PositionRandomly(cell, entranceExit, errors);
        return entranceExit;
    }

    private static Direction RandomDirection()
    {
        switch(Random.Range(0, 4))
        {
            case 0:
                return Direction.NORTH;
            case 1:
                return Direction.EAST;
            case 2:
                return Direction.SOUTH;
            case 3:
                return Direction.WEST;
            default:
                return Direction.NORTH;
        }
    }


    // Returns a random direction other than the given one
    private static Direction RandomDirection(Direction direction)
    {
        Direction dir = direction;
        while (dir == direction)
            dir = RandomDirection();

        return dir;
    }

    private static bool EntranceFits(Cell cell, CellEntranceExit entrance)
    {
        int x, y;
        bool searchX;
        switch(entrance.Side)
        {
            case Direction.NORTH:
                y = (int)cell.Size - 1;
                x = 0;
                searchX = true;
                break;

            case Direction.SOUTH:
                y = 0;
                x = 0;
                searchX = true;
                break;

            case Direction.EAST:
                y = 0;
                x = 0;
                searchX = false;
                break;
            case Direction.WEST:
                y = 0;
                x = (int)cell.Size - 1;
                searchX = false;
                break;

            default:
                return false;
        }

        for (int i = EDGE_MARGIN; i < cell.Size - entrance.Width - EDGE_MARGIN; i++)
        {
            int count = 0;
            for (int j = 0; j < entrance.Width; j++)
            {
                int x1 = x, y1 = y;

                if (searchX)
                    x1 += i + j; 
                else
                    y1 += i + j;

                if (cell.Get(x1, y1) == 0)
                    count++;
                else
                    break; 
            }
            if (count == entrance.Width)
                return true;
        }
        return false;
    }

    private static void PositionRandomly(Cell cell, CellEntranceExit entrance, ErrorCollector errors)
    {
        int x, y;
        bool searchX;
        switch (entrance.Side)
        {
            case Direction.NORTH:
                x = 0;
                y = (int)cell.Size - 1;
                searchX = true;
                break;

            case Direction.SOUTH:
                x = 0;
                y = 0;
                searchX = true;
                break;

            case Direction.WEST:
                x = 0;
                y = 0;
                searchX = false;
                break;
            case Direction.EAST:
                x = (int)cell.Size - 1;
                y = 0;
                searchX = false;
                break;

            default:
                return;
        }


        // Try to randomly place
        int tries = 0;
        while (tries < 10)
        {
            int count = 0;
            int index = Random.Range(EDGE_MARGIN, (int)(cell.Size - entrance.Width - EDGE_MARGIN));
            for (int i = 0; i < entrance.Width; i++)
            {
                int x1 = x, y1 = y;

                if (searchX)
                    x1 += i + index;
                else
                    y1 += i + index;

                if (cell.Get(x1, y1) == 0)
                    count++;
                else
                    break;
            }
            if (count == entrance.Width)
            {
                SetPosition(cell, entrance, x, y, index, searchX);
                return;
            }
            tries++;
        }

        // Give up and just place it in the first possible spot
        for (int i = EDGE_MARGIN; i < cell.Size - entrance.Width - EDGE_MARGIN; i++)
        {
            int count = 0;
            for (int j = 0; j < entrance.Width; j++)
            {
                int x1 = x, y1 = y;

                if (searchX)
                    x1 += i + j;
                else
                    y1 += i + j;

                if (cell.Get(x1, y1) == 0)
                    count++;
                else
                    break;
            }
            if (count == entrance.Width)
            {
                SetPosition(cell, entrance, x, y, i, searchX);
                return;
            }
        }
        errors.Add(new System.Exception("Unable to place the EntranceExit."));
    }

    private static void SetPosition(Cell cell, CellEntranceExit entrance, int x, int y, int offset, bool searchX)
    {
        Vector2Int position;
        if (searchX)
            position = new Vector2Int(x + offset, y);
        else
            position = new Vector2Int(x, y + offset);

        entrance.Position = position;


        for (int j = 0; j < entrance.Width; j++)
        {
            int x1 = x, y1 = y;
            if (searchX)
                x1 += offset + j;
            else
                y1 += offset + j;

            cell.Set(x1, y1, TileType.ENTRANCE);
            entrance.coordinates.Add(new Vector2Int(x1,y1));
        }
    }
}
