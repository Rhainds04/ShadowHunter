using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using TMPro;
public class GameManager : MonoBehaviour
{
    //get et set necessaire pour utiliser l'instance dans les tests
    public static GameManager Instance { get; set; }

    public string LastUsedDoor;//pour nous aider à placer le Player lorsqu'il change de scene.

    public Vector3 LastSpawnPoint = new Vector3(1.5f, 0.5f, 0);//pour savoir ou déplacer le Player lorsqu'il meurt.
    public string LastSpawnPointScene = "SafeZone";//pour sauvegarder la scene

    public float _health;

    public TextMeshProUGUI keyCountText;
    private float keyCount = 0;

    //MainDoor Keys
    public bool hasMainDoorKey1 = false;
    public bool hasMainDoorKey2 = false;
    public bool hasMainDoorKey3 = false;

    //PathwayA key
    public bool hasPathwayAKey = false;

    //ShortcutA key
    public bool hasShortcutAKey = false;

    //Locks
    public bool lock1Locked = true;
    public bool lock2Locked = true;
    public bool lock3Locked = true;

    public static event Action OnMainDoorUnlocked;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//détruit pas le gameobject onLoad.
        }
        else
        {
            Destroy(gameObject);//Si Instance n'est pas définit.
        }
    }

    public void manageKeys(GameObject key)
    {
        string keyName = key.name;
        switch (keyName)
        {
            case "MainDoorKey1":
                hasMainDoorKey1 = true;
                keyCount++;
                keyCountText.text = $"Keys: {keyCount}/5";
                break;
            case "MainDoorKey2":
                hasMainDoorKey2 = true;
                keyCount++;
                keyCountText.text = $"Keys: {keyCount}/5";
                break;
            case "MainDoorKey3":
                hasMainDoorKey3 = true;
                keyCount++;
                keyCountText.text = $"Keys: {keyCount}/5";
                break;
            case "PathwayAKey":
                hasPathwayAKey = true;
                keyCount++;
                keyCountText.text = $"Keys: {keyCount}/5";
                break;
            case "ShortcutAKey":
                hasShortcutAKey = true;
                keyCount++;
                keyCountText.text = $"Keys: {keyCount}/5";
                break;
        }
        Destroy(key);
    }
    public void interactWithLock(GameObject doorLock)
    {
        Debug.Log("interactWithLock called");

        switch (doorLock.name)
        {
            case "Lock1":
                if (lock1Locked && hasMainDoorKey1)
                {
                    lock1Locked = false;
                    var lockedState = doorLock.transform.Find("Locked");
                    var unlockedState = doorLock.transform.Find("Unlocked");
                    lockedState.gameObject.SetActive(false);
                    unlockedState.gameObject.SetActive(true);
                }
                break;
            case "Lock2":
                if (lock2Locked && hasMainDoorKey2)
                {
                    lock2Locked = false;
                    var lockedState = doorLock.transform.Find("Locked");
                    var unlockedState = doorLock.transform.Find("Unlocked");
                    lockedState.gameObject.SetActive(false);
                    unlockedState.gameObject.SetActive(true);
                }
                break;
            case "Lock3":
                if (lock3Locked && hasMainDoorKey3)
                {
                    lock3Locked = false;
                    var lockedState = doorLock.transform.Find("Locked");
                    var unlockedState = doorLock.transform.Find("Unlocked");
                    lockedState.gameObject.SetActive(false);
                    unlockedState.gameObject.SetActive(true);
                }
                break;
        }

        if(!lock1Locked && !lock2Locked && !lock3Locked)
        {
            OnMainDoorUnlocked?.Invoke();
            var mainDoor = GameObject.Find("ExitDoor");
            if(mainDoor != null)
            {
                mainDoor.transform.GetChild(1).gameObject.SetActive(false);
                mainDoor.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
}
