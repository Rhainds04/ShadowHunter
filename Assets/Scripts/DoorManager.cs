using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public void setLastUsedDoor(string currentDoor)
    {
        switch (currentDoor)
        {
            case "SafeZoneToDungeon":
                GameManager.Instance.LastUsedDoor = currentDoor;
                break;
                /*ajouter des portes*/
        }
    }
}
