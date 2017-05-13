using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

    public Connection Socket { get; set; }
    public string Username { get; set; }
    public bool Connected { get; set; }
    public string Key { get; set; }
    public Color Coloration { get { return sprite.color; } set { sprite.color = value; } }
    public bool IsGrounded { get; private set; }

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;
    public  LayerMask groundMask;
    private Transform groundCheck;    // A position marking where to check if the player is grounded.
    const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    // Test things
    [Header("Debug")]
    public bool singleAction;
    public bool continousAction;
    private bool continousActionActive;
    public Buttons InputTest;
    public bool enableKeyboard = false;
    [Space(25)]
    // Test things


    [Header("Player tuning")]
    public int jumpCount = 2;
    public int remainingJump = 2;
    public float moveSpeed = 5;
    public float jumpForce = 1.2f;
    enum Direction { None, Left, Right, Down, Up }
    private Direction wantedDirection = Direction.None;

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

    // Jump input must treated here for some reasons ...
    private void Update()
    {
        if (enableKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
    }
    // Update is called once per frame
    void FixedUpdate ()
    {
        CheckGrounded();
        UpdateAnimation();

    //    body.isKinematic = IsGrounded && wantedDirection == Direction.None;

        // input test
        if (enableKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
            if (Input.GetKey(KeyCode.Q))
                wantedDirection = Direction.Left;
            else if (Input.GetKey(KeyCode.D))
                wantedDirection = Direction.Right;
            else if (Input.GetKey(KeyCode.S))
                wantedDirection = Direction.Down;
            else
                wantedDirection = Direction.None;
        }

        if (singleAction || continousAction)
        {
            if(continousAction)
                continousActionActive = true;

            ButtonPressedMessage m = new ButtonPressedMessage();
            m.ButtonId = InputTest;
            ReceiveMessage(m);

            if (singleAction)
            {
                singleAction = false;
                ButtonPressedMessage m2 = new ButtonPressedMessage();
                m2.ButtonId = InputTest;
                ReceiveMessage(m2);
            }
        }

        if (!continousAction && continousActionActive)
        {
            ButtonReleasedMessage m = new ButtonReleasedMessage();
            m.ButtonId = InputTest;
            ReceiveMessage(m);
        }
        // end test

        if(wantedDirection == Direction.Left || wantedDirection == Direction.Right)
        {
            Vector2 velocity = new Vector2((wantedDirection == Direction.Left ? -1 : 1) * moveSpeed, body.velocity.y);
            body.velocity = velocity;
        }
	}

    private void CheckGrounded()
    {
        IsGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                IsGrounded = true;
        }

        if (IsGrounded)
            remainingJump = jumpCount;
    }

    private void UpdateAnimation()
    {
        if (wantedDirection == Direction.Left && IsGrounded)
        {
            sprite.flipX = true;
            anim.Play(kWalkAnim);
        }
        else if(wantedDirection == Direction.Right && IsGrounded)
        {
            sprite.flipX = false;
            anim.Play(kWalkAnim);
        }
        else if (wantedDirection == Direction.None && IsGrounded)
        {
            anim.Play(kStandAnim);
        }
        else if (wantedDirection == Direction.Down && IsGrounded)
        {
            anim.Play(kCrouchAnim);
        }
        else if(!IsGrounded)
        {
            if (body.velocity.y > 0)
                anim.Play(kJumpAnim);
            else
                anim.Play(kStandAnim);

            if (wantedDirection == Direction.Left)
                sprite.flipX = true;
            else if (wantedDirection == Direction.Left)
                sprite.flipX = false;
        }
    }

    public void ReceiveMessage(Message mess)
    {
        if(mess is ButtonPressedMessage)
        {
            ButtonPressedMessage m = (ButtonPressedMessage)mess;

            switch (m.ButtonId)
            {
                case Buttons.A:
                    Jump();
                    break;
                case Buttons.DPadLeft:
                    wantedDirection = Direction.Left;
                    break;
                case Buttons.DPadRight:
                    wantedDirection = Direction.Right;
                    break;
                case Buttons.DPadDown:
                    wantedDirection = Direction.Down;
                    break;
                case Buttons.DPadUp:
                    wantedDirection = Direction.Up;
                    break;
            }
        }
        else if(mess is ButtonReleasedMessage)
        {
            ButtonReleasedMessage m = (ButtonReleasedMessage)mess;

            switch (m.ButtonId)
            {
                case Buttons.DPadLeft:
                case Buttons.DPadRight:
                case Buttons.DPadDown:
                case Buttons.DPadUp:
                    wantedDirection = Direction.None;
                    break;
            }

        }
    }

    public void Jump()
    {
        if (remainingJump <= 0)
            return;
        if (!IsGrounded)
            body.velocity = Vector2.zero;

        body.AddForce(new Vector2(0f, jumpForce));
        --remainingJump;
    }
}
