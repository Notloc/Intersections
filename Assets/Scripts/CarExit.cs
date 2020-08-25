using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExit : MonoBehaviour
{
    public void Exit(Car car)
    {
        Destroy(car.gameObject);
        Score.Instance.Add(car.Score);
    }
}
