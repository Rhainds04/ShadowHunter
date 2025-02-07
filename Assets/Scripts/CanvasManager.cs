using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//d�truit pas le Player lorsqu'il change de scene.
        }
        else
        {
            Destroy(gameObject);//Si Instance n'est pas d�finit.
        }
    }
}
