using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float speed = 1.0f; // Speed of movement
    public float distance = 2.0f; // Distance to move up and down from the starting position

    private Vector3 startPosition;
    private bool movingUp = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (movingUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);

            if (transform.position.y >= startPosition.y + distance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

            if (transform.position.y <= startPosition.y)
            {
                movingUp = true;
            }
        }
    }
}
