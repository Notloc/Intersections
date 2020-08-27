using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileToTexture
{
    public TileType tile;
    public Sprite sprite;
}

public class GameWorld : MonoBehaviour
{
    [SerializeField] uint cellSize = 10;
    [SerializeField] Grid grid = null;
    [SerializeField] CellGenerator generator = null;
    [SerializeField] TileToTexture[] tileTextures = null;
    [SerializeField] Car[] carPrefabs = null;

    CellGrid cellGrid;

    private void Start()
    {
        Dictionary<TileType, Sprite> tileDictionary = CreateTileDictionary();

        cellGrid = new CellGrid(grid, cellSize);
        Cell cell = generator.GenerateCell(Vector2Int.zero, cellSize);

        cell.Spawn(tileDictionary, carPrefabs);
    }


    private Dictionary<TileType, Sprite> CreateTileDictionary()
    {
        Dictionary<TileType, Sprite> dictionary = new Dictionary<TileType, Sprite>();

        foreach(TileToTexture tToTex in tileTextures)
        {
            dictionary.Add(tToTex.tile, tToTex.sprite);
        }
        return dictionary;
    }
}
