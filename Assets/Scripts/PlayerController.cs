using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IPlayerInput
{
    float HorizontalInput { get; }
    bool SpaceInput { get; }

    bool EInput { get; }

    bool SInput { get; }

}

public class PlayerInput : IPlayerInput
{
    public float HorizontalInput
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public bool SpaceInput
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    public bool EInput
    {
        get
        {
            return Input.GetKeyDown(KeyCode.E);
        }
    }

    public bool SInput
    {
        get
        {
            return Input.GetKey(KeyCode.S);
        }
    }
}
public class PlayerController : MonoBehaviour
{
    private float _speed = 5f;
    private float _jumpSpeed = 600f;
    private float SKeyRequiredHoldTime = .5f;
    private float SKeyHoldTimer;
    private float enemyAndPlayerDistance;

    public static bool _playerIsDead = false;
    private bool _isOnGround;
    private bool _isOnDoor;
    private bool _isOnSpawnPoint;
    private bool _isLookingDown;
    private bool _MainDoorIsOpen = false;
    private bool _isOnLock;
    private bool _OnEnemyRange;

    private PlayerInput _playerInput;
    private Rigidbody2D _playerRb;
    private Animator myAnim;
    private BoxCollider2D stairsCollider;
    private SpriteRenderer spriteRenderer;

    private GameObject playerSpawner;
    private string currentDoor;
    GameObject currentLock;
    GameObject currentEnemy;

    Vector2 initialOffset;

    public static event Action<bool> OnLookDown;
    public static event Action<GameObject, float> OnEnemyHit;

    private void Start()
    {
        _playerInput = new PlayerInput();
        _playerRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        stairsCollider = transform.Find("StairsCollider").GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialOffset = stairsCollider.offset;
        GameManager.Instance._health = 10f;
        playerSpawner = GameObject.Find("PlayerSpawner");
        GameManager.OnMainDoorUnlocked += MainDoorUnlocked;
    }

    private void Update()
    {
        if (!_playerIsDead)
        {
            PlayerControllerInputs();
            ManageDoors();
            ManageSpawnPoints();
            ManageLocks();
        }
        else
        {
            StartCoroutine(RespawnPlayer(1f));
        }
    }

    private void PlayerControllerInputs()
    {
        if (_playerInput.HorizontalInput != 0)
        {
            if (_isOnGround)
            {
                myAnim.Play("PlayerRunning");
                myAnim.SetBool("IsRunning", true);
            }
            else
            {
                myAnim.Play("PlayerFalling");
            }

            transform.position += Vector3.right * _playerInput.HorizontalInput * _speed * Time.deltaTime;

            if (_playerInput.HorizontalInput > 0)
            {
                spriteRenderer.flipX = false;
                stairsCollider.offset = initialOffset;
            }
            else if (_playerInput.HorizontalInput < 0)
            {
                spriteRenderer.flipX = true;
                stairsCollider.offset = -initialOffset;
            }
        }
        else if (_playerInput.HorizontalInput == 0)
        {
            myAnim.SetBool("IsRunning", false);
        }
        if (_playerInput.SpaceInput && _isOnGround)
        {
            _playerRb.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            myAnim.Play("PlayerJump");
        }
        AttackEnemy();
        CameraLookDown();
    }

    private void AttackEnemy()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAnim.Play("PlayerAttack", 0, 0f);
            if (_OnEnemyRange)
            {
                OnEnemyHit?.Invoke(currentEnemy, 2f);
            }
        }
    }

    private void CameraLookDown()
    {
        if (_playerInput.SInput && _isOnGround)
        {
            SKeyHoldTimer += Time.deltaTime;

            if (SKeyHoldTimer >= SKeyRequiredHoldTime)
            {
                _isLookingDown = true;  
                OnLookDown?.Invoke(true);
            }
        }
        else
        {
            if(_isLookingDown)
            {
                _isLookingDown = false;
                OnLookDown?.Invoke(false);
                SKeyHoldTimer = 0f;
            }
        }
    }

    private void ManageDoors()
    {
        if (_isOnDoor)
        {
            if (_playerInput.EInput)
            {
                GameManager.Instance.LastUsedDoor = currentDoor;
                playerSpawner.GetComponent<PlayerSpawner>().UseDoor();
                if (PlayerSpawner.doorUnlocked)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.doorClosing);
                    StartCoroutine(ChangeScene(1f));
                }
            }
        }
    }

    IEnumerator ChangeScene(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.sceneLoaded += OnSceneLoaded;//est call quand la scene est load à 100%
        SceneManager.LoadScene(PlayerSpawner.sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            player.transform.position = PlayerSpawner.newPlayerPosition;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ManageSpawnPoints()
    {
        if (_isOnSpawnPoint)
        {
            if (_playerInput.EInput)
            {
                GameManager.Instance.LastSpawnPoint = transform.position;
                GameManager.Instance.LastSpawnPointScene = SceneManager.GetActiveScene().name;
                Debug.Log("spawn point set");
            }
        }
    }

    IEnumerator RespawnPlayer(float duration)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string lastSpawnPointScene = GameManager.Instance.LastSpawnPointScene;

        spriteRenderer.enabled = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.deathSound);

        yield return new WaitForSeconds(duration);

        if (currentScene.name != lastSpawnPointScene)
        {
            SceneManager.LoadScene(lastSpawnPointScene);
            transform.position = GameManager.Instance.LastSpawnPoint;
        }
        else
        {
            transform.position = GameManager.Instance.LastSpawnPoint;
        }
        _playerIsDead = false;
        spriteRenderer.enabled = true;
    }

    private void MainDoorUnlocked()
    {
        _MainDoorIsOpen = true;
    }

    private void ManageLocks()
    {
        if (_isOnLock)
        {
            if (_playerInput.EInput)
            {
                Debug.Log("E pressed");
                GameManager.Instance.interactWithLock(currentLock);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            myAnim.SetBool("IsOnGround", true);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = false;
            myAnim.SetBool("IsOnGround", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if(other.gameObject.name == "DungeonToExit")
            {
                if (_MainDoorIsOpen)
                {
                    _isOnDoor = true;
                    currentDoor = other.gameObject.name;
                }
            }
            else
            {
                _isOnDoor = true;
                currentDoor = other.gameObject.name;
            }
        }
        else if (other.gameObject.CompareTag("SpawnPoint"))
        {
            _isOnSpawnPoint = true;
        }
        else if (other.gameObject.CompareTag("Spikes"))
        {
            _playerIsDead = true;
        }
        else if (other.gameObject.CompareTag("Key"))
        {
            GameManager.Instance.manageKeys(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Lock"))
        {
            currentLock = other.gameObject;
            _isOnLock = true;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            enemyAndPlayerDistance = Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position);
            if(enemyAndPlayerDistance <= 2)
            {
                _OnEnemyRange = true;
                currentEnemy = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            _isOnDoor = false;
        }
        else if (other.gameObject.CompareTag("SpawnPoint"))
        {
            _isOnSpawnPoint = false;
        }
        else if (other.gameObject.CompareTag("Lock"))
        {
            _isOnLock = false;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            _OnEnemyRange = false;
        }
    }
}