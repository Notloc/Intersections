using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarEntrance : MonoBehaviour
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    [SerializeField] Car[] carPrefabs = null;

    [SerializeField] Transform spawnPoint = null;
    [SerializeField] Direction direction = Direction.EAST;
    [SerializeField] CarExit oppositeExit = null;

    [SerializeField] float spawnRate = 6f;
    [SerializeField] float spawnRandomness = 2f;

    private float spawnTimer = 0f;

    private void Start()
    {
        ResetSpawnTimer();
    }

    private void ResetSpawnTimer()
    {
        spawnTimer = Time.time + spawnRate + Random.Range(0f, spawnRandomness);
    }

    private void FixedUpdate()
    {
        if (Time.time > spawnTimer)
            SpawnCar();
    }

    private void SpawnCar()
    {
        Car carPrefab = GetRandomCar();
        Car car = Instantiate(carPrefab, spawnPoint.transform.position, Quaternion.identity);

        Vector2 dir = GetDirection();
        float rotation = GetRotation();

        car.Initialize(dir, rotation, oppositeExit);

        ResetSpawnTimer();
    }

    private Car GetRandomCar()
    {
        int index = Random.Range(0, carPrefabs.Length);
        return carPrefabs[index];
    }

    private Vector2 GetDirection()
    {
        switch(direction)
        {
            case Direction.NORTH :
                return Vector2.up;
            case Direction.EAST:
                return Vector2.right;
            case Direction.SOUTH:
                return Vector2.down;
            case Direction.WEST:
                return Vector2.left;
            default:
                return Vector2.right;
        }
    }

    private float GetRotation()
    {
        switch (direction)
        {
            case Direction.NORTH:
                return 0f;
            case Direction.EAST:
                return 270f;
            case Direction.SOUTH:
                return 180f;
            case Direction.WEST:
                return 90;
            default:
                return 0f;
        }
    }
}
