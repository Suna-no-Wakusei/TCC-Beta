using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy : Fighter
{
    //Experience
    public int xpValue = 10;

    //Logic
    public float attackRange;
    public float chasingRange;
    private bool attackBool;
    private Transform target;

    public float speed = 1f;

    private Vector3 startingPosition;
    private Vector3 roamPosition;

    private Rigidbody2D rb;
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
        target = GameManager.instance.hero.transform;
        startingPosition = transform.position;

        //velocity
        oldpos = transform.position;

        //health
        hp = 10;
        maxHP = 10;
    }

    private void Update()
    {
        CheckDistance();

        //velocity
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

    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chasingRange && Vector3.Distance(target.position, transform.position) >= attackRange)
        {
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            changeAnim(transform.position - temp);

            transform.position = temp;

            if (Vector3.Distance(target.position, transform.position) == attackRange && attackReady)
                StartCoroutine(EnemyAttack());
        }
    }

    private void SetAnimFloat(Vector2 setVector)
    {
        animator.SetFloat("Horizontal", setVector.x);
        animator.SetFloat("Vertical", setVector.y);
    }

    private void changeAnim(Vector2 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimFloat(Vector2.left);
            }else if(direction.x < 0)
            {
                SetAnimFloat(Vector2.right);
            }
        }else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.down);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.up);
            }
        }
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

        yield return new WaitForSeconds(.5f);
        attackReady = true;
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.experience += xpValue;
        GameManager.instance.ShowText("+"+xpValue+" XP", 8, Color.green, transform.position, Vector3.up * 35, 0.5f);
    }
}
