using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float range;
    public float speed;
    public bool movementIsHorizontal;
    public float waitTime = 0.5f;
    private bool movingUp = true;

    private GameObject player;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if(player != null)
        {

        }

        if (!movementIsHorizontal)
        {
            if (movingUp && transform.position.y <= initialPosition.y + range)
            {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y >= initialPosition.y + range)
                {
                    movingUp = false;
                    Invoke("ToggleDirection", waitTime);
                    enabled = false;
                }
            }
            else if (!movingUp && transform.position.y >= initialPosition.y)
            {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y <= initialPosition.y)
                {
                    movingUp = true;
                    Invoke("ToggleDirection", waitTime);
                    enabled = false;
                }
            }
        }
        else
        {
            if(movingUp)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (transform.position.x >= initialPosition.x + range)
                {
                    movingUp = false;
                    enabled = false;
                    Invoke("ToggleDirection", waitTime);
                }
            }
            else if(!movingUp)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (transform.position.x <= initialPosition.x)
                {
                    movingUp = true;
                    enabled = false;
                    Invoke("ToggleDirection", waitTime);
                }
            }
        }
    }

    private void ToggleDirection()
    {
        enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }
}
