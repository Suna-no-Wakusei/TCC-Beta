using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;

public class Player : Fighter
{
    public bool timeRunning = true;

    private BoxCollider2D boxCollider;

    public event Action OnAttack;
    public event Action OnEndAttack;

    public PolygonCollider2D colliderAttack;

    public PlayerInput playerInput;

    private Vector2 inputMovement;
    public PlayerInput PlayerInput => playerInput;

    private Rigidbody2D rb;
    private float colliderY;
    private Vector2 moveDelta;
    public float fireballSpeed = 6f;

    public float moveSpeed = 5f;
    public float attackCooldown;
    public int attackDamage;
    private bool dodgeUp = false;

    private float LastMoveVertical;
    private float LastMoveHorizontal;
    private bool MoveX, MoveY;
    private Vector2 facingDir;

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
            rb.velocity = inputMovement * moveSpeed;

            //Animation
            animator.SetFloat("Horizontal", rb.velocity.x);
            if (animator.GetFloat("Horizontal") != 0)
            {
                LastMoveHorizontal = animator.GetFloat("Horizontal");
                MoveX = true;
            }

            animator.SetFloat("Vertical", rb.velocity.y);
            if (animator.GetFloat("Vertical") != 0)
            {
                LastMoveVertical = animator.GetFloat("Vertical");
                MoveY = true;
            }

            animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

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
            if (rb.velocity.x != 0 && rb.velocity.y != 0)
            {
                rb.velocity = rb.velocity/new Vector3((float)1.4, (float)1.4);
            }

            facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        var inputValue = ctx.ReadValue<Vector2>();

        if(timeRunning)
            inputMovement = inputValue;
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }

        if (timeRunning)
            StartCoroutine(Attaque());

        inputMovement = Vector2.zero;
    }

    public void Collect(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        if (timeRunning)
            Collect();
    }

    public void Dodge(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        if (timeRunning)
            if(dodgeUp)
                StartCoroutine(PlayerDodge());
    }

    IEnumerator PlayerDodge()
    {
        dodgeUp = true;
        boxCollider.size = new Vector2(boxCollider.size.x, 0.3f);
        animator.SetTrigger("Dodge");
        rb.AddForce(facingDir * 10, ForceMode2D.Force);

        yield return new WaitForSeconds(0.25f);

        animator.SetTrigger("Dodge");
        boxCollider.size = new Vector2(boxCollider.size.x, colliderY);

        yield return new WaitForSeconds(1f);

        Debug.Log("Desvia");

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
        if(GameManager.instance.weaponLevel == 1)
        {
            OnAttack?.Invoke();

            animator.SetBool("Attack", true);

            yield return null;

            animator.SetBool("Attack", false);

            yield return new WaitForSeconds(attackCooldown);

            OnEndAttack?.Invoke();
        }
    }

    protected override void Death()
    {
        Time.timeScale = 0f;
        timeRunning = false;
        GameManager.instance.deathScreen.SetActive(true);
    }


    public void Respawn()
    {
        if (File.Exists(Application.persistentDataPath + "/JSONData.sus"))
        {
            SaveSystem.LoadState();
            SaveSystem.LoadSavedScene();
            Time.timeScale = 1f;
        }
    }
}