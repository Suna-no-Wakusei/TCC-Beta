using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    private Vector2 moveDelta;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;

    private bool isMoving;

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
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    void Collect()
    {
        var facingDir = new Vector3(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, Interactable);
        if (collider != null)
            collider.GetComponent<Collectable>()?.Collect();
    }
}
