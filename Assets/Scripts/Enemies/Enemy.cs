using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 10;

    //Logic
    public float attackRange;
    public float chaseLenght;
    public bool chasing;
    public bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Rigidbody2D rb;
    private Vector2 moveDelta;
    public float moveSpeed = 2f;

    public CircleCollider2D circleEnemyAttack;

    //Animation
    public Animator animator;

    //Hitbox
    private BoxCollider2D hitbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameManager.instance.hero.transform;
        startingPosition = transform.position;
        hitbox = GetComponent<BoxCollider2D>();

        hp = 10;
        maxHP = 10;
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
            chasing = true;

        if(chasing){
            MoveEnemy(playerTransform.position - transform.position);
            if (Vector3.Distance(playerTransform.position, transform.position) <= attackRange)
            {
                StartCoroutine(EnemyAttack());
            }
        }
    }

    IEnumerator EnemyAttack()
    {
        Vector3 diferencia = playerTransform.position - circleEnemyAttack.gameObject.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        circleEnemyAttack.gameObject.transform.rotation = Quaternion.Euler(0, 0, angulo);
        circleEnemyAttack.enabled = true;

        yield return new WaitForSeconds(.1f);

        circleEnemyAttack.enabled = false;

        yield return new WaitForSeconds(5f);
    }

    private void MoveEnemy(Vector3 toMove)
    {
        moveDelta = toMove.normalized;

        //Animation
        animator.SetFloat("Horizontal", moveDelta.x);
        animator.SetFloat("Vertical", moveDelta.y);

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
