using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour, ISavable
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;

    [SerializeField] FacingDir defaultDirection = FacingDir.Down;

    //Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    //States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;

    SpriteAnimator currentAnim;
    bool wasMoving;
    //References
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnim = new SpriteAnimator(spriteRenderer, walkDownSprites);
        walkUpAnim = new SpriteAnimator(spriteRenderer, walkUpSprites);
        walkLeftAnim = new SpriteAnimator(spriteRenderer, walkLeftSprites);
        walkRightAnim = new SpriteAnimator(spriteRenderer, walkRightSprites);
        SetFacingDirection(defaultDirection);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;
        if(MoveX == 1)
        {
            currentAnim = walkRightAnim;
        }
        else if(MoveX == -1)
        {
            currentAnim = walkLeftAnim;
        }
        else if(MoveY == 1)
        {
            currentAnim = walkUpAnim;
        }
        else if(MoveY == -1)
        {
            currentAnim = walkDownAnim;
        }

        if(currentAnim != prevAnim || IsMoving != wasMoving)
        {
            currentAnim.Start();
        }

        if(IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            spriteRenderer.sprite = currentAnim.Frames[0];
        }
        wasMoving = IsMoving;
    }
    public void SetFacingDirection(FacingDir dir)
    {
        if(dir == FacingDir.Right)
        {
            MoveX = 1;
        }
        else if(dir == FacingDir.Left)
        {
            MoveX = -1;
        }
        else if (dir == FacingDir.Down)
        {
            MoveY = -1;
        }
        else if (dir == FacingDir.Up)
        {
            MoveY = 1;
        }
    }

    public object CaptureState()
    {
        return defaultDirection;
    }

    public void RestoreState(object state)
    {
        defaultDirection = (FacingDir)state;
        SetFacingDirection(defaultDirection);
    }

    public FacingDir DefaultDirection
    {
        get { return defaultDirection; }
    }
}
public enum FacingDir { Up, Down, Left, Right}
