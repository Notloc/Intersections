using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ClickCar();
    }

    private void ClickCar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);
        if (hit)
        {
            Car car = hit.collider.GetComponentInParent<Car>();
            if (car)
                car.SpeedUp();
        }

    }
}
