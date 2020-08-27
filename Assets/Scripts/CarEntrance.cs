using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarEntrance : MonoBehaviour
{    
    float spawnRate = 2.5f;
    float spawnRandomness = 4f;

    Car[] carPrefabs;
    CarExit exit;
    Road road;

    private float spawnTimer = 0f;


    public void Initialize(Road road, CarExit exit, Car[] carPrefabs)
    {
        this.exit = exit;
        this.road = road;
        this.carPrefabs = carPrefabs;
    }

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
        Car car = Instantiate(carPrefab, transform.position, Quaternion.identity);
        car.Initialize(road, exit);

        ResetSpawnTimer();
    }

    private Car GetRandomCar()
    {
        int index = Random.Range(0, carPrefabs.Length);
        return carPrefabs[index];
    }
}
