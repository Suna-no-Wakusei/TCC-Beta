using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public class TreantM : Fighter
{
    //Savement
    public ScriptableEnemy enemy;

    //Experience
    public int xpValue = 10;

    //Logic
    public enum GameState
    {
        Idle,
        ChasingTarget,
        Attacking,
        GoBackToStart
    }

    public GameState state;

    public float attackRange;
    public float chasingRange;
    private Transform target;
    private Vector3 roamPosition;
    private Vector3 startingPosition;
    private bool stopMoving = false;
    public GameObject rootAttack;

    public float speed = 1200f;
    public float nextWaypointDistance = 0.5f;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;

    private Rigidbody2D rb;
    private Vector2 oldpos;
    bool attackReady = true;

    //Animation
    public Animator animator;

    //Hitbox
    public BoxCollider2D hitbox;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        target = GameManager.instance.hero.transform;

        //velocity
        startingPosition = transform.position;
        oldpos = transform.position;

        //Updating Pathfinding
        InvokeRepeating("UpdatePath", 0f, 0.5f);

        //health
        hp = 10;
        maxHP = 10;
    }

    void UpdatePath()
    {
        if (characterUnableToMove) return;

        Vector3 targetPathFinal = Vector3.zero;
        switch (state)
        {
            default:
            case GameState.Idle:
                

                FindTarget();
                break;
            case GameState.ChasingTarget:
                if (Vector3.Distance(transform.position, target.position) >= attackRange - 0.25f)
                {
                    if (seeker.IsDone())
                        seeker.StartPath(rb.position, target.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), OnPathComplete);
                }

                if (Vector3.Distance(transform.position, target.position) <= attackRange && attackReady)
                {
                    path = null;
                    stopMoving = true;
                    state = GameState.Attacking;
                    Coroutine lastAttack = StartCoroutine(EnemyAttack());
                    if (damaged)
                    {
                        StopCoroutine(lastAttack);

                        animator.SetBool("Attack", false);

                        rootAttack.SetActive(false);

                        stopMoving = false;
                        state = GameState.ChasingTarget;
                    }
                }

                float stopChasing = 10f;
                if (Vector3.Distance(transform.position, target.position) > stopChasing)
                {
                    state = GameState.Idle;
                }
                break;
            case GameState.Attacking:
                break;
            case GameState.GoBackToStart:
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, startingPosition, OnPathComplete);

                if (Vector3.Distance(transform.position, roamPosition) < 3f)
                {
                    state = GameState.Idle;
                }
                break;
        }

        if (path != null)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                targetPathFinal = target.position;
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + UtilsClass.GetRandomDir() * Random.Range(-4f, 4f);
    }

    private void FindTarget()
    {
        //Chasing target
        if (Vector2.Distance(transform.position, target.position) < chasingRange)
            state = GameState.ChasingTarget;
    }

    private void Update()
    {
        if (!enemy.alive)
            Destroy(this.gameObject);

        if (!enemy.canMove)
            characterUnableToMove = true;
        else
            characterUnableToMove = false;
    }

    private void FixedUpdate()
    {
        if (characterUnableToMove) return;

        if (path == null) return;

        if (!stopMoving)
        {
            try
            {
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

                Vector2 force = direction * speed * Time.deltaTime;

                rb.AddForce(force);

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }

                //velocity
                animator.SetFloat("Horizontal", rb.velocity.x);
                animator.SetFloat("Vertical", rb.velocity.y);
            }
            catch
            {
                //velocity
                animator.SetFloat("Horizontal", rb.velocity.x);
                animator.SetFloat("Vertical", rb.velocity.y);
            }
        }

        float lastMoveVertical = 0;
        float lastMoveHorizontal = 0;

        Vector2 newpos = transform.position;
        var media = (newpos - oldpos);
        Vector2 velocity = media / Time.deltaTime;

        if (media != Vector2.zero)
        {
            lastMoveHorizontal = velocity.x;
            lastMoveVertical = velocity.y;
        }

        oldpos = newpos;
        newpos = transform.position;

        animator.SetFloat("IdleHorizontal", lastMoveHorizontal);
        animator.SetFloat("IdleVertical", lastMoveVertical);

        animator.SetFloat("Speed", Mathf.Abs(velocity.x + velocity.y));
    }

    IEnumerator EnemyAttack()
    {
        animator.SetBool("Attack", true);

        GameManager.instance.sfxManager.PlayRootAttack2();

        animator.SetFloat("AttackHorizontal", target.position.x - rb.position.x);
        animator.SetFloat("AttackVertical", target.position.y - rb.position.y);

        Vector2 attackPos = GameManager.instance.hero.transform.position;

        stopMoving = true;

        yield return new WaitForSeconds(0.5f);

        rootAttack.SetActive(true);

        rootAttack.transform.position = attackPos;

        yield return null;

        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(1.5f);

        rootAttack.SetActive(false);

        state = GameState.ChasingTarget;

        stopMoving = false;
    }

    protected override void Death()
    {
        if (enemy.alive)
            GameManager.instance.sfxManager.PlayTreantHurt();

        StartCoroutine(DeathAnimation());
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+" + xpValue + " XP", 18, Color.green, transform.position, Vector3.up * 35, 0.5f);
    }

    IEnumerator DeathAnimation()
    {
        rootAttack.SetActive(false);
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("Death", false);
        enemy.alive = false;
        Destroy(gameObject);
    }
}
