using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;

    [SerializeField] List<Sprite> idleDownSprites;
    [SerializeField] List<Sprite> idleUpSprites;
    [SerializeField] List<Sprite> idleRightSprites;
    [SerializeField] List<Sprite> idleLeftSprites;

    //Parameters
    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public float IdleHorizontal { get; set; }
    public float IdleVertical { get; set; }

    public float Speed { get; set; }

    //States

    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator idleDown;
    SpriteAnimator idleUp;
    SpriteAnimator idleRight;
    SpriteAnimator idleLeft;

    SpriteAnimator currentAnim;
    SpriteAnimator currentIdle;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        idleDown = new SpriteAnimator(idleDownSprites, spriteRenderer);
        idleUp = new SpriteAnimator(idleUpSprites, spriteRenderer);
        idleRight = new SpriteAnimator(idleRightSprites, spriteRenderer);
        idleLeft = new SpriteAnimator(idleLeftSprites, spriteRenderer);

        currentAnim = walkDownAnim;
        currentIdle = idleDown;
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        var prevIdle = currentIdle;

        if (IdleHorizontal == 1)
            currentIdle = idleRight;
        else if (IdleHorizontal == -1)
            currentIdle = idleLeft;
        else if (IdleVertical == 1)
            currentIdle = idleUp;
        else if (IdleVertical == -1)
            currentIdle = idleDown;

        if (Horizontal == 1)
            currentAnim = walkRightAnim;
        else if (Horizontal == -1)
            currentAnim = walkLeftAnim;
        else if (Vertical == 1)
            currentAnim = walkUpAnim;
        else if (Vertical == -1)
            currentAnim = walkDownAnim;

        if (currentAnim != prevAnim)
            currentAnim.Start();
        if(currentIdle != prevIdle)
            currentIdle.Start();

        if (Speed == 0)
            currentIdle.HandleUpdate();
        else
            currentAnim.HandleUpdate();

    }
}
