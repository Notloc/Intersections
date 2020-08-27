using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExit : MonoBehaviour
{
    private void Awake()
    {
        Rigidbody2D r = gameObject.AddComponent<Rigidbody2D>();
        r.gravityScale = 0f;
        r.constraints = RigidbodyConstraints2D.FreezeAll;

        Collider2D c = gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger = true;
    }

    public void Exit(Car car)
    {
        Destroy(car.gameObject);
        Score.Instance.Add(car.Score);
    }
}
