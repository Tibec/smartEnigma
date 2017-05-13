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

    private SpriteRenderer sprite;

    // Test things
    [Header("Debug")]
    public bool singleAction;
    public bool continousAction;
    private bool continousActionActive;
    public Buttons InputTest;
    public bool enableKeyboard = false;

    private PlayerController controller;

    // Use this for initialization
    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
    }

    // Jump input must treated here for some reasons ...
    private void Update()
    {
        if (enableKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                controller.Jump();
        }
    }
    // Update is called once per frame
    void FixedUpdate ()
    {
        // input test
        if (enableKeyboard)
        {
            if (Input.GetKey(KeyCode.Q))
                controller.Move(PlayerDirection.Left);
            else if (Input.GetKey(KeyCode.D))
                controller.Move(PlayerDirection.Right);
            else if (Input.GetKey(KeyCode.S))
                controller.Move(PlayerDirection.Down);
            else
                controller.Move(PlayerDirection.None);
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

	}



    public void ReceiveMessage(Message mess)
    {
        if(mess is ButtonPressedMessage)
        {
            ButtonPressedMessage m = (ButtonPressedMessage)mess;

            switch (m.ButtonId)
            {
                case Buttons.A:
                    controller.Jump();
                    break;
                case Buttons.DPadLeft:
                    controller.Move(PlayerDirection.Left);
                    break;
                case Buttons.DPadRight:
                    controller.Move(PlayerDirection.Right);
                    break;
                case Buttons.DPadDown:
                    controller.Move(PlayerDirection.Down);
                    break;
                case Buttons.DPadUp:
                    controller.Move(PlayerDirection.Up);
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
                    controller.Move(PlayerDirection.None);
                    break;
            }

        }
    }

}
