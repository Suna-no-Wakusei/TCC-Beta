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
    private Vector2 facingDirIdle;

    public LayerMask Interactable;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        colliderY = boxCollider.size.y;

        dodgeUp = true;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (characterUnableToMove)
            return;

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
                rb.velocity = rb.velocity * 0.9f;
            }

            facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
            facingDirIdle = new Vector3(animator.GetFloat("IdleHorizontal"), animator.GetFloat("IdleVertical"));
        }

        if(rb.velocity == Vector2.zero)
        {
            GameManager.instance.sfxManager.StopShortGrass();
            GameManager.instance.sfxManager.StopFootstepWood();
            GameManager.instance.sfxManager.StopLongGrass();
            GameManager.instance.sfxManager.StopEarthStep();
        }
            
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        var inputValue = ctx.ReadValue<Vector2>();

        if (timeRunning)
        {
            if (GameManager.instance.hero.gameObject.activeSelf)
            {
                switch (GameManager.instance.floorType)
                {
                    case GameManager.FloorType.Grass:
                        if (!GameManager.instance.sfxManager.shortGrass.isPlaying)
                            StartCoroutine(playShort());

                        GameManager.instance.sfxManager.StopFootstepWood();
                        GameManager.instance.sfxManager.StopLongGrass();
                        GameManager.instance.sfxManager.StopEarthStep();
                        break;
                    case GameManager.FloorType.Wood:
                        if (!GameManager.instance.sfxManager.footstepWood.isPlaying)
                            StartCoroutine(playWood());

                        GameManager.instance.sfxManager.StopShortGrass();
                        GameManager.instance.sfxManager.StopLongGrass();
                        GameManager.instance.sfxManager.StopEarthStep();
                        break;
                    case GameManager.FloorType.TallGrass:
                        if (!GameManager.instance.sfxManager.longGrass.isPlaying)
                            StartCoroutine(playTall());

                        GameManager.instance.sfxManager.StopShortGrass();
                        GameManager.instance.sfxManager.StopFootstepWood();
                        GameManager.instance.sfxManager.StopEarthStep();
                        break;
                    case GameManager.FloorType.Earth:
                        if (!GameManager.instance.sfxManager.earthStep.isPlaying)
                            StartCoroutine(playEarth());

                        GameManager.instance.sfxManager.StopShortGrass();
                        GameManager.instance.sfxManager.StopFootstepWood();
                        GameManager.instance.sfxManager.StopLongGrass();
                        break;
                }
            }
            
            
            inputMovement = inputValue;
        }
        else
            inputMovement = Vector2.zero;
    }

    IEnumerator playShort()
    {
        GameManager.instance.sfxManager.PlayShortGrass();
        yield return new WaitForSeconds(GameManager.instance.sfxManager.shortGrass.clip.length);
    }

    IEnumerator playTall()
    {
        GameManager.instance.sfxManager.PlayLongGrass();
        yield return new WaitForSeconds(GameManager.instance.sfxManager.longGrass.clip.length);
    }

    IEnumerator playWood()
    {
        GameManager.instance.sfxManager.PlayFootstepWood();
        yield return new WaitForSeconds(GameManager.instance.sfxManager.footstepWood.clip.length);
    }

    IEnumerator playEarth()
    {
        GameManager.instance.sfxManager.PlayEarthStep();
        yield return new WaitForSeconds(GameManager.instance.sfxManager.earthStep.clip.length);
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }

        if(characterUnableToMove) { return; }

        if(Time.deltaTime == 0) { return; }

        if (timeRunning)
            StartCoroutine(Attaque());
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
        characterUnableToMove = true;

        dodgeUp = false;
        GameManager.instance.sfxManager.PlayDash();

        if (facingDir == Vector2.zero)
            rb.AddForce(facingDirIdle * 5f, ForceMode2D.Impulse);
        else
            rb.AddForce(facingDir * 5f, ForceMode2D.Impulse);
        animator.SetBool("Dodge", true);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Dodge", false);

        characterUnableToMove = false;

        yield return new WaitForSeconds(1f);

        dodgeUp = true;
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

            GameManager.instance.sfxManager.PlaySwordSwing();

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