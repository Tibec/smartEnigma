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

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animation anim;


    // Test things
    [Header("Debug")]
    public bool singleAction;
    public bool continousAction;
    private bool continousActionActive;
    public Buttons InputTest;
    [Space(25)]
    // Test things


    [Header("Player tuning")]
    public int jumpCount = 2;
    private int remainingJump = 2;
    public float moveSpeed = 5;
    public float jumpForce = 1.2f;
    enum Direction { None, Left, Right }
    private Direction dir = Direction.None;

	// Use this for initialization
	void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animation>();
       
    }

    public bool IsGrounded()
    {
        return Physics2D.Raycast(body.position, -Vector2.up, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // input test
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

        if(dir != Direction.None)
        {
            Vector2 velocity = new Vector2((dir == Direction.Right ? -1 : 1) * moveSpeed, body.velocity.y);
            body.velocity = velocity;
        }

        if(IsGrounded())
        {
            remainingJump = jumpCount;
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
                    sprite.flipX = false;
                    dir = Direction.Left;
                    break;
                case Buttons.DPadRight:
                    sprite.flipX = true;
                    dir = Direction.Right;
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
                    dir = Direction.None;
                    break;
            }

        }
    }

    public void Jump()
    {
        if (remainingJump <= 0)
            return;
        if(!IsGrounded())
            body.velocity = new Vector2();
        body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        --remainingJump;
    }
}
