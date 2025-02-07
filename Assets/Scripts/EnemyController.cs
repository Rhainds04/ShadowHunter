using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float range;
    private float playerRange = 8f;
    private float speed = 2f;
    public float _health = 5f;
    private float bounceForce = 1f;
    private float minPlayerDistance = 1f;

    public bool _IsDead = false;

    public SpriteRenderer spriteRenderer;
    private Animator myAnim;
    private Vector3 initialPosition;
    private GameObject player;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        PlayerController.OnEnemyHit += OnEnemyHit;
    }

    private void Update()
    {
        if (!_IsDead)
        {
            myAnim.Play("EnemyMoving");

            float distanceWithPlayer = Vector3.Distance(transform.position, player.transform.position);
            Vector3 direction = (player.transform.position - transform.position).normalized;

            if (player != null)
            {
                if(transform.position.x <= initialPosition.x + range && transform.position.x >= initialPosition.x - range)
                {
                    if (distanceWithPlayer <= playerRange && distanceWithPlayer >= -playerRange)
                    {
                        if (distanceWithPlayer >= minPlayerDistance || distanceWithPlayer <= -minPlayerDistance)
                        {
                            transform.position += direction * speed * Time.deltaTime;
                        }
                    }
                }
            }
            FlipSprite(direction);
        }
        else
        {
            //die animation
            Destroy(gameObject);
        }
    }

    private void FlipSprite(Vector3 direction)
    {
        if(direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void OnEnemyHit(GameObject enemy, float damage)
    {
        if (enemy != gameObject) return;//the event is only called on currentEnemy.
        StartCoroutine(TakeDamage(enemy, damage));
    }

    private IEnumerator TakeDamage(GameObject enemy, float damage)
    {
        if(player != null)
        {
            Vector3 bounceDirection = (transform.position - player.transform.position).normalized;

            Vector3 startPosition = transform.position;

            Vector3 targetPosition = startPosition + bounceDirection * bounceForce;

            float bounceDuration = 0.3f;
            float clock = 0f;

            while (clock < bounceDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, clock / bounceDuration);
                clock += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }

        _health -= damage;

        if (_health <= 0)
        {
            _IsDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance._health -= 1;
            Debug.Log(GameManager.Instance._health + " health");
            if (GameManager.Instance._health == 0)
            {
                PlayerController.OnEnemyHit -= OnEnemyHit;
                PlayerController._playerIsDead = true;
            }
        }
    }
}
