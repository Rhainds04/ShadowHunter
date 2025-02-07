using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbStair : MonoBehaviour
{
    private float stairsHeight = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            Vector3 playerPosition = transform.parent.position;
            playerPosition.y += stairsHeight;
            transform.parent.position = playerPosition;
        }
    }
}