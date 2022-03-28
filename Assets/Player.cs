using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    private Vector3 moveDelta;

    public float moveSpeed = 5f;

    private float LastMoveVertical;
    private float LastMoveHorizontal;

    private bool MoveX, MoveY;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Reset moveDelta
        moveDelta = new Vector3(x, y, 0);

        //Movement
        transform.Translate(moveDelta * Time.fixedDeltaTime * moveSpeed);

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
        if(MoveX == true)
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

    }
}
