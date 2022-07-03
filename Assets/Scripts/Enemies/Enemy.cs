using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 10;

    //Logic
    public float attackRange;
    public float chaseLenght;
    private bool chasing;
    private bool attackBool;
    public bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    private Rigidbody2D rb;
    private Vector2 moveDelta;
    private bool MoveX, MoveY;
    private float LastMoveVertical;
    private float LastMoveHorizontal;
    private Vector2 oldpos;
    bool attackReady;

    public CircleCollider2D circleEnemyAttack;

    //Animation
    public Animator animator;

    //Hitbox
    public BoxCollider2D hitbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameManager.instance.hero.transform;
        startingPosition = transform.position;

        //velocity
        oldpos = transform.position;

        //health
        hp = 10;
        maxHP = 10;
    }

    private void FixedUpdate()
    {
        //velocity
        float lastMoveVertical = 0;
        float lastMoveHorizontal = 0;

        Vector2 newpos = transform.position;
        var media = (newpos - oldpos);
        Vector2 velocity = media / Time.deltaTime;

        if(media != Vector2.zero)
        {
            lastMoveHorizontal = velocity.x;
            lastMoveVertical = velocity.y;
        }

        oldpos = newpos;
        newpos = transform.position;

        //Animation
        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Vertical", velocity.y);

        animator.SetFloat("IdleHorizontal", lastMoveHorizontal);
        animator.SetFloat("IdleVertical", lastMoveVertical);

        animator.SetFloat("Speed", Mathf.Abs(velocity.x + velocity.y));

        //Stating Machine
        if (Vector3.Distance(playerTransform.position, transform.position) > attackRange)
        {
            attackBool = true;
            attackReady = true;
        }
        else
            attackBool = false;

        if (!attackBool)
        {
            GetComponent<IAstarAI>().canMove = false;
            if (attackReady)
                StartCoroutine(EnemyAttack());
        }
    }

    IEnumerator EnemyAttack()
    {
        Vector3 diferencia = playerTransform.position - circleEnemyAttack.gameObject.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        circleEnemyAttack.gameObject.transform.rotation = Quaternion.Euler(0, 0, angulo);
        circleEnemyAttack.enabled = true;
        attackReady = false;

        yield return new WaitForSeconds(.1f);

        circleEnemyAttack.enabled = false;

        yield return new WaitForSeconds(.5f);
        attackReady = true;
        GetComponent<IAstarAI>().canMove = true;
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+"+xpValue+" XP", 8, Color.green, transform.position, Vector3.up * 35, 0.5f);
    }
}
