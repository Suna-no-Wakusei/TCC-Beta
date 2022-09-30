using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 10;

    //Logic
    public enum GameState
    {
        Roaming,
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
    public bool enemyAlive = true;

    public float speed = 1200f;
    public float nextWaypointDistance = 0.5f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = true;

    Seeker seeker;

    private Rigidbody2D rb;
    private Vector2 oldpos;
    bool attackReady = true;

    public CircleCollider2D circleEnemyAttack;

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
        InvokeRepeating("UpdatePath", 0f, 1f);

        //health
        hp = 10;
        maxHP = 10;
    }

    void UpdatePath()
    {
        Vector3 targetPathFinal = Vector3.zero;
        switch (state)
        {
            default:
            case GameState.Roaming:
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, GetRoamingPosition(), OnPathComplete);

                float reachedPositionDistance = 5f;
                if(Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    roamPosition = GetRoamingPosition();
                }

                FindTarget();
                break;
            case GameState.ChasingTarget:
                if (Vector3.Distance(transform.position, target.position) >= attackRange - 0.25f)
                {
                    if (seeker.IsDone())
                        seeker.StartPath(rb.position, target.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), OnPathComplete);
                }

                if(Vector3.Distance(transform.position, target.position) <= attackRange && attackReady)
                {
                    path = null;
                    stopMoving = true;
                    state = GameState.Attacking;
                    StartCoroutine(EnemyAttack());
                }

                float stopChasing = 10f;
                if(Vector3.Distance(transform.position, target.position) > stopChasing)
                {
                    state = GameState.Roaming;
                }
                break;
            case GameState.Attacking:
                break;
            case GameState.GoBackToStart:
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, startingPosition, OnPathComplete);

                reachedPositionDistance = 5f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    state = GameState.Roaming;
                }
                break;
        }

        if(path != null)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                targetPathFinal = target.position;
            }
            else
            {
                reachedEndOfPath = false;
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

    private void FixedUpdate()
    {
        if(path == null) return;

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
                animator.SetFloat("Horizontal", force.x);
                animator.SetFloat("Vertical", force.y);
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
        yield return new WaitForSeconds(.4f);
        Vector3 diferencia = target.position - circleEnemyAttack.gameObject.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        circleEnemyAttack.gameObject.transform.rotation = Quaternion.Euler(0, 0, angulo);
        circleEnemyAttack.enabled = true;
        attackReady = false;

        yield return new WaitForSeconds(.1f);

        circleEnemyAttack.enabled = false;

        yield return new WaitForSeconds(0.5f);

        rb.AddForce(diferencia * -500, ForceMode2D.Force);
        attackReady = true;

        state = GameState.ChasingTarget;

        stopMoving = false;
    }

    protected override void Death()
    {
        Destroy(gameObject);
        enemyAlive = false;
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+"+xpValue+" XP", 8, Color.green, transform.position, Vector3.up * 35, 0.5f);
    }
}
