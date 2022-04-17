using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 1;

    //Logic
    public float triggerLenght;
    public float chaseLenght;
    public bool chasing;
    public bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Rigidbody2D rb;
    private Vector2 moveDelta;
    public float moveSpeed = 3f;

    //Animation
    private float LastMoveVertical;
    private float LastMoveHorizontal;
    private bool MoveX, MoveY;
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
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            chasing = true;
        }

        if(chasing){
            StartCoroutine(EnemyAttack());
            MoveEnemy(playerTransform.position + new Vector3(3,0) - transform.position);
        }
    }

    IEnumerator EnemyAttack()
    {
        if (!fireball.fireballRunning)
            fireball.EnemyFireball();

        yield return new WaitForSeconds(1f);
    }

    private void MoveEnemy(Vector3 toMove)
    {
        moveDelta = (toMove).normalized;

        //Animation
        animator.SetFloat("Horizontal", moveDelta.x);
        if (animator.GetFloat("Horizontal") != 0)
        {
            LastMoveHorizontal = animator.GetFloat("Horizontal");
            MoveX = true;
        }

        animator.SetFloat("Vertical", moveDelta.y);
        if (animator.GetFloat("Vertical") != 0)
        {
            LastMoveVertical = animator.GetFloat("Vertical");
            MoveY = true;
        }
        //idle animation
        if (MoveX == true)
        {
            animator.SetFloat("IdleVertical", 0);
            LastMoveVertical = 0;
            MoveX = false;
        }
        else
        {
            animator.SetFloat("IdleVertical", LastMoveVertical);
        }
        if (MoveY == true)
        {
            animator.SetFloat("IdleHorizontal", 0);
            LastMoveHorizontal = 0;
            MoveY = false;
        }
        else
        {
            animator.SetFloat("IdleHorizontal", LastMoveHorizontal);
        }

        animator.SetFloat("Speed", moveDelta.sqrMagnitude);

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
