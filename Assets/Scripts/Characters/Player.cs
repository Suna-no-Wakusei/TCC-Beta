using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : Fighter
{
    public bool timeRunning = true;

    private BoxCollider2D boxCollider;

    public event Action OnAttack;
    public event Action OnEndAttack;

    public PolygonCollider2D colliderAttack;

    private Rigidbody2D rb;
    private float colliderY;
    private Vector2 moveDelta;
    public float fireballSpeed = 6f;

    public float moveSpeed = 5f;
    public float attackCooldown;
    private bool attackReady = true;
    private bool dodgeUp = false;

    private float LastMoveVertical;
    private float LastMoveHorizontal;
    private bool MoveX, MoveY;

    public LayerMask Interactable;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        colliderY = boxCollider.size.y;

        hp = GameManager.instance.health;
        maxHP = GameManager.instance.maxHP;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (timeRunning)
        {
            moveDelta.x = Input.GetAxisRaw("Horizontal");
            moveDelta.y = Input.GetAxisRaw("Vertical");

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

            animator.SetFloat("Speed", moveDelta.sqrMagnitude);

            //idle
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

            //check for diagonal movement
            if (moveDelta.x != 0 && moveDelta.y != 0)
            {
                moveDelta *= 0.7f;
            }
            rb.MovePosition(rb.position + moveDelta * Time.fixedDeltaTime * moveSpeed);

            //Combat
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackReady)
            {
                StartCoroutine(Attaque());
            }

            //Collect/Interact
            if (Input.GetKeyDown(KeyCode.E))
            {
                Collect();
            }

            //Dodge
            if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeUp == false && moveDelta != Vector2.zero)
                StartCoroutine(PlayerDodge());
        }
    }

    IEnumerator PlayerDodge()
    {
        dodgeUp = true;
        boxCollider.size = new Vector2(boxCollider.size.x, 0.3f);
        animator.SetTrigger("Dodge");
        moveSpeed *= 1.5f;

        yield return new WaitForSeconds(0.25f);

        moveSpeed /= 1.5f;
        animator.SetTrigger("Dodge");
        boxCollider.size = new Vector2(boxCollider.size.x, colliderY);

        yield return new WaitForSeconds(1f);

        dodgeUp = false;
    }

    void Collect()
    {
        var facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, Interactable);
        if (collider != null)
            collider.GetComponent<ICollectable>()?.Collect();
    }

    //ARRUMAR QUARTENIAL DO ATAQUE

    IEnumerator Attaque()
    {
        OnAttack?.Invoke();

        attackReady = false;

        animator.SetBool("Attack", true);

        yield return null;

        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(attackCooldown);

        attackReady = true;

        OnEndAttack?.Invoke();
    }

    protected override void Death()
    {
        SceneManager.LoadScene("StartMenu");
    }

}