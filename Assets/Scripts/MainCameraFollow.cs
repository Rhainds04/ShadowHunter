using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    private float verticalOffset = 1.5f;
    private float verticalMovementSpeed = 0.05f;

    private bool _isLookingDown;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        PlayerController.OnLookDown += HandleLookDown;
    }

    private void OnDestroy()
    {
        PlayerController.OnLookDown -= HandleLookDown;
    }

    private void Update()
    {
        if(player != null)
        {
            Vector3 newPosition = player.transform.position;
            newPosition.z = -10;
            newPosition.y = player.transform.position.y + verticalOffset;
            transform.position = Vector3.Lerp(transform.position, newPosition, verticalMovementSpeed);
        }
    }

    private void HandleLookDown(bool isLookingDown)
    {
        _isLookingDown = isLookingDown;

        Vector3 newPosition = transform.position;

        if (_isLookingDown)
        {
            newPosition.y -= 0.2f;
            transform.position = newPosition;
        }
        else
        {
            newPosition.y += 0.2f;
            transform.position = newPosition;
        }
    }
}
