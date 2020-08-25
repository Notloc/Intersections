using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Car : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float speedUpSpeed = 5f;
    [SerializeField] uint score = 100;
    Rigidbody2D rigid2D;
    CarExit exit;

    public uint Score { get { return score; } }

    public void Initialize(Vector2 direction, float rotation, CarExit targetExit)
    {
        rigid2D = GetComponent<Rigidbody2D>();
        rigid2D.velocity = direction * speed;
        rigid2D.rotation = rotation;
        exit = targetExit;
    }

    public void SpeedUp()
    {
        rigid2D.velocity = rigid2D.velocity.normalized * speedUpSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Car otherCar = collision.gameObject.GetComponent<Car>();
        if (otherCar)
        {
            Destroy(this.gameObject);
            Destroy(otherCar.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CarExit exit = collision.gameObject.GetComponent<CarExit>();
        if (exit && exit == this.exit)
        {
            exit.Exit(this);
        }
    }
}
