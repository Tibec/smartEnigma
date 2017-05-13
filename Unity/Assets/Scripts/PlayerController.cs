using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerDirection { None, Left, Right, Down, Up }

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;
    public LayerMask groundMask;
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .01f; // Radius of the overlap circle to determine if grounded

    // Movement data
    public int jumpCount = 2;
    public int remainingJump = 2;
    public float moveSpeed = 2.1f;
    public float jumpForce = 4f;
    private PlayerDirection wantedDirection = PlayerDirection.None;
    private bool jumpInProgress = false;
    public bool isGrounded;
    // Animation
    private const string kStandAnim = "Standing";
    private const string kWalkAnim = "Walking";
    private const string kJumpAnim = "Jumping";
    private const string kFallAnim = "Jumping";
    private const string kCrouchAnim = "Crouching";

    // Use this for initialization
    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        groundCheck = transform.Find("GroundCheck");
        //m_CeilingCheck = transform.Find("CeilingCheck");
    }

    // physics
    void FixedUpdate()
    {

        if (wantedDirection == PlayerDirection.Left || wantedDirection == PlayerDirection.Right)
        {
            Vector2 velocity = new Vector2((wantedDirection == PlayerDirection.Left ? -1 : 1) * moveSpeed, body.velocity.y);
            body.velocity = velocity;
        }

    }

    //input
    void Update ()
    {
        CheckGrounded();
        UpdateAnimation();
        
        if (isGrounded && wantedDirection == PlayerDirection.None && !jumpInProgress)
        {
            body.isKinematic = true;
            body.velocity = Vector2.zero;
        }
        else
            body.isKinematic = false;

    }

    private void UpdateAnimation()
    {
        if (wantedDirection == PlayerDirection.Left && isGrounded)
        {
            sprite.flipX = true;
            anim.Play(kWalkAnim);
        }
        else if (wantedDirection == PlayerDirection.Right && isGrounded)
        {
            sprite.flipX = false;
            anim.Play(kWalkAnim);
        }
        else if (wantedDirection == PlayerDirection.None && isGrounded)
        {
            anim.Play(kStandAnim);
        }
        else if (wantedDirection == PlayerDirection.Down && isGrounded)
        {
            anim.Play(kCrouchAnim);
        }
        else if (!isGrounded)
        {
            if (body.velocity.y > 0)
                anim.Play(kJumpAnim);
            else
                anim.Play(kStandAnim);

            if (wantedDirection == PlayerDirection.Left)
                sprite.flipX = true;
            else if (wantedDirection == PlayerDirection.Left)
                sprite.flipX = false;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !jumpInProgress)
                isGrounded = true;
        }

        if (!isGrounded && jumpInProgress)
            jumpInProgress = false;

        if (isGrounded)
        {
            remainingJump = jumpCount;
        }
    }

    public void Move(PlayerDirection dir)
    {
        wantedDirection = dir;
    }

    public void Jump()
    {
        if (remainingJump <= 0)
            return;
        if (!isGrounded)
            body.velocity = Vector2.zero;

        body.isKinematic = false;

        body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        --remainingJump;
        jumpInProgress = true;
    }

}
