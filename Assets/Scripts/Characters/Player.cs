using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{

    private BoxCollider2D boxCollider;

    public CircleCollider2D circleColliderAttack;

    private Rigidbody2D rb;
    private float colliderY;
    private Vector2 moveDelta;
    public float fireballSpeed = 6f;

    public float moveSpeed = 5f;
    public float attackRange = 0.5f;
    bool dodgeUp = false;

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

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void HandleUpdate()
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attaque());
        }

        //Collect/Interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }

        //Dodge
        if(Input.GetKeyDown(KeyCode.LeftShift) && dodgeUp == false && moveDelta != Vector2.zero)
            StartCoroutine(PlayerDodge());
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
            collider.GetComponent<Collectable>()?.Collect();
    }

    IEnumerator Attaque()
    {
        //animator.SetTrigger("Attack");
        //talvez mudar o collider de um circulo pra alguma outra forma / botar animação para arma e variações de armas e magias
        Vector3 diferencia = Camera.main.ScreenToWorldPoint(Input.mousePosition) - circleColliderAttack.gameObject.transform.position;
        float angulo = Mathf.Atan2(diferencia.y, diferencia.x) * Mathf.Rad2Deg;
        circleColliderAttack.gameObject.transform.rotation = Quaternion.Euler(0, 0, angulo);
        circleColliderAttack.enabled = true;
        yield return new WaitForSeconds(.1f);
        circleColliderAttack.enabled = false;
    }

}