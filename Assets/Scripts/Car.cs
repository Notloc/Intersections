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

    Road road;
    CarExit exit;

    float currentSpeed;
    float timer;
    Node currentNode;
    Vector2 lastPosition;

    public uint Score { get { return score; } }


    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Road road, CarExit targetExit)
    {
        this.road = road;
        exit = targetExit;

        rigid2D.rotation = GetRotation(road.Start.direction);
        currentSpeed = speed;

        currentNode = road.Start;
        NextNode();

        timer = 1f;
    }

    private float GetRotation(Direction direction)
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

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * currentSpeed;
        Move();
    }

    private void Move()
    {
        rigid2D.MovePosition(Vector2.Lerp(lastPosition, currentNode.position, timer));
        rigid2D.rotation = Mathf.Lerp(rigid2D.rotation, GetRotation(currentNode.direction), timer * 5f);

        if (timer > 1f)
        {
            timer -= 1f;
            NextNode();
            Move();
        }
    }

    private void NextNode()
    {
        if (currentNode == null)
            return;

        lastPosition = currentNode.position;
        currentNode = road.NextNode(currentNode);
    }

    public void SpeedUp()
    {
        currentSpeed = speedUpSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CarExit exit = collision.gameObject.GetComponent<CarExit>();
        if (exit && exit == this.exit)
        {
            exit.Exit(this);
        }

        Car otherCar = collision.gameObject.GetComponent<Car>();
        if (otherCar)
        {
            Destroy(this.gameObject);
            Destroy(otherCar.gameObject);
        }
    }
}
