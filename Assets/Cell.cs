using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual cell, 
/// </summary>
public class Cell
{
    private Transform transform;
    private int[,] tileGrid;
    private GameObject[,] realGrid;
    private Vector2Int coordinates;
    private List<Road> roads = new List<Road>();

    public uint Size { get; private set; }

    public Cell(Vector2Int coordinates, Transform transform, uint size)
    {
        this.coordinates = coordinates;
        this.transform = transform;
        Size = size;
        CreateGrid(size);
    }

    public void Spawn(Dictionary<TileType, Sprite> tileDictionary, Car[] carPrefabs)
    {
        realGrid = new GameObject[Size,Size];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                TileType type = (TileType)tileGrid[x, y];
                Sprite sprite = tileDictionary[type];
                GameObject tile = new GameObject(x+","+y);

                float z = 0f;
                if (type == TileType.ENTRANCE || type == TileType.EXIT)
                {
                    z = -2f;
                }

                tile.transform.SetParent(transform);
                tile.transform.localPosition = new Vector3(x, y, z);

                SpriteRenderer r = tile.AddComponent<SpriteRenderer>();
                r.sprite = sprite;

                realGrid[x, y] = tile;
            }
        }

        SpawnRoads(carPrefabs);
    }

    private void SpawnRoads(Car[] carPrefabs)
    {
        foreach (Road road in roads)
        {
            Vector2Int start = road.Start.position;
            Vector2Int end = road.End.position;

            CarExit exit = realGrid[end.x, end.y].AddComponent<CarExit>();
            CarEntrance entrance = realGrid[start.x, start.y].AddComponent<CarEntrance>();

            entrance.Initialize(road, exit, carPrefabs);
        }
    }

    public void AddRoad(Road road)
    {
        roads.Add(road);
        for (int i = 0; i < road.path.nodes.Count; i++)
        {
            Node node = road.path.nodes[i];
            if (i == 0)
                Set(node.position, TileType.ENTRANCE);
            else if (i == road.path.nodes.Count - 1)
                Set(node.position, TileType.EXIT);
            else
                Set(node.position, TileType.ROAD);
        }
    }

    private void CreateGrid(uint size)
    {
        tileGrid = new int[size, size];
    }

    public TileType Get(int x, int y)
    {
        return (TileType)tileGrid[x, y];
    }

    public void Set(Vector2Int coords, TileType block)
    {
        Set(coords.x, coords.y, (int)block);
    }
    public void Set(int x, int y, TileType block)
    {
        Set(x, y, (int)block);
    }
    private void Set(int x, int y, int blockId)
    {
        tileGrid[x, y] = blockId;
    }

    public void Clear()
    {
        tileGrid = new int[Size,Size];
    }
}
