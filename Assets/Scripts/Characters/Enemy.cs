using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 1;

    //Logic
    public float attackRange;
    public float chaseLenght;
    public bool chasing;
    public bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Rigidbody2D rb;
    private Vector2 moveDelta;
    public float moveSpeed = 5f;
    private float startingSpeed;

    //Animation
    public Animator animator;

    //Fireball
    public Fireball fireball;

    //Hitbox
    private BoxCollider2D hitbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameManager.instance.hero.transform;
        startingPosition = transform.position;
        hitbox = GetComponent<BoxCollider2D>();
        startingSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
            chasing = true;

        if(chasing){
            MoveEnemy(playerTransform.position - transform.position);
            if (Vector3.Distance(playerTransform.position, transform.position) <= attackRange)
            {
                moveSpeed = 0;
                StartCoroutine(EnemyAttack());
            }
            else
            {
                moveSpeed = startingSpeed;
            }
        }
    }

    IEnumerator EnemyAttack()
    {
        if (!fireball.fireballRunning)
            fireball.EnemyFireball();

        yield return new WaitForSeconds(3f);
    }

    private void MoveEnemy(Vector3 toMove)
    {
        moveDelta = (toMove).normalized;

        //Animation
        animator.SetFloat("Horizontal", moveDelta.x);
        animator.SetFloat("Vertical", moveDelta.y);
        
        if (playerTransform.position.y > transform.position.y)
        {
            if (playerTransform.position.x > transform.position.x && (2 > playerTransform.position.y - transform.position.y && playerTransform.position.y - transform.position.y > -2))
            {
                animator.SetFloat("IdleHorizontal", 1);
                animator.SetFloat("IdleVertical", 0);
                return;
            }
            animator.SetFloat("IdleVertical", 1);
        }
        else if (playerTransform.position.y < transform.position.y)
        {
            if (playerTransform.position.x < transform.position.x && (2 > playerTransform.position.y - transform.position.y || playerTransform.position.y - transform.position.y > -2))
            {
                animator.SetFloat("IdleHorizontal", -1);
                animator.SetFloat("IdleVertical", 0);
                return;
            }
            animator.SetFloat("IdleVertical", -1);
        }

        animator.SetFloat("Speed", moveSpeed);

        //check for diagonal movement
        if (moveDelta.x != 0 && moveDelta.y != 0)
        {
            moveDelta *= 0.7f;
        }

        //move the enemy
        rb.MovePosition(rb.position + moveDelta * Time.fixedDeltaTime * moveSpeed);
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+"+xpValue+" XP", 8, Color.green, transform.position, Vector3.up * 35, 0.5f);
    }
}
