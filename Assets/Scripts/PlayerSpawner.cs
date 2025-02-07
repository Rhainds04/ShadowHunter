using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public static string sceneToLoad;
    public static Vector3 newPlayerPosition;
    public static bool doorUnlocked;
    public void UseDoor()
    {
        GameObject player = GameObject.FindWithTag("Player");
        doorUnlocked = true;

        if (player != null)
        {
            if (GameManager.Instance != null)
            {
                switch (GameManager.Instance.LastUsedDoor)
                {
                    case "DungeonToSafeZone":
                        Debug.Log("DungeonToSafeZone");
                        sceneToLoad = "SafeZone";
                        newPlayerPosition = new Vector3(5, -0.5f, 0);//position de la DungeonToSafeZone Door.
                        break;
                    case "SafeZoneToDungeon":
                        Debug.Log("SafeZoneToDungeon");
                        sceneToLoad = "Dungeon";
                        newPlayerPosition = new Vector3(0, -0.5f, 0);//position de SafeZoneToDungeon Door.
                        break;
                    case "DungeonToStorageRoom":
                        Debug.Log("DungeonToStorageRoom");
                        sceneToLoad = "StorageRoom";
                        newPlayerPosition = new Vector3(0, -0.5f, 0);//position de la DungeonToStorageRoom Door.
                        break;
                    case "StorageRoomToDungeon":
                        Debug.Log("StorageRoomToDungeon");
                        sceneToLoad = "Dungeon";
                        newPlayerPosition = new Vector3(-2, 10.5f, 0);//position de la StorageRoomToDungeon Door.
                        break;
                    case "StorageRoomToShortcutA":
                        if (GameManager.Instance.hasShortcutAKey)
                        {
                            Debug.Log("StorageRoomToShortcutA");
                            sceneToLoad = "ShortcutA";
                            newPlayerPosition = new Vector3(-4, -0.5f, 0);//position de la StorageRoomToShortcutA Door.
                        }
                        else
                        {
                            doorUnlocked = false;
                        }
                        break;
                    case "ShortcutAToStorageRoom":
                        Debug.Log("ShortcutAToStorageRoom");
                        sceneToLoad = "StorageRoom";
                        newPlayerPosition = new Vector3(-14, 10.5f, 0);//position de la ShortcutAToStorageRoom Door.
                        break;
                    case "ShortcutAToDungeon":
                        Debug.Log("ShortcutAToDungeon");
                        sceneToLoad = "Dungeon";
                        newPlayerPosition = new Vector3(53, 23.5f, 0);//position de la ShortcutAToDungeon Door.
                        break;
                    case "DungeonToShortcutA":
                        if (GameManager.Instance.hasShortcutAKey)
                        {
                            Debug.Log("DungeonToShortcutA");
                            sceneToLoad = "ShortcutA";
                            newPlayerPosition = new Vector3(40, -0.5f, 0);//position de la DungeonToShortcutA Door.
                        }
                        else
                        {
                            doorUnlocked = false;
                        }
                        break;
                    case "PathwayAToDungeon1":
                        Debug.Log("PathwayAToDungeon1");
                        sceneToLoad = "Dungeon";
                        newPlayerPosition = new Vector3(-19, -6.5f, 0);//position de la PathwayAToDungeon1 Door.
                        break;
                    case "DungeonToPathwayA1":
                        if (GameManager.Instance.hasPathwayAKey)
                        {
                            Debug.Log("DungeonToPathwayA1");
                            sceneToLoad = "PathwayA";
                            newPlayerPosition = new Vector3(1, -0.5f, 0);//position de la DungeonToPathwayA1 Door.
                        }
                        else
                        {
                            doorUnlocked = false;
                        }
                        break;
                    case "PathwayAToDungeon2":
                        Debug.Log("PathwayAToDungeon2");
                        sceneToLoad = "Dungeon";
                        newPlayerPosition = new Vector3(-24, -6.5f, 0);//position de la PathwayAToDungeon2 Door.
                        break;
                    case "DungeonToPathwayA2":
                        if (GameManager.Instance.hasPathwayAKey)
                        {
                            Debug.Log("DungeonToPathwayA2");
                            sceneToLoad = "PathwayA";
                            newPlayerPosition = new Vector3(-37, -0.5f, 0);//position de la DungeonToPathwayA2 Door.
                        }
                        else
                        {
                            doorUnlocked = false;
                        }
                        break;
                    case "DungeonToExit":
                        Debug.Log("DungeonToExit");
                        sceneToLoad = "Exit";
                        newPlayerPosition = new Vector3(1, 3.5f, 0);//position de Exit.
                        break;
                }
            }
        }
    }
}
