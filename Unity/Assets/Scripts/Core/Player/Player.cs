using CreativeSpore.SmartColliders;
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
    private GameElement nearestInteraction;

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

    private void Update()
    {
        // input test
        if (enableKeyboard)
        {
            controller.SetActionState(eControllerActions.Jump, Input.GetKeyDown(KeyCode.Space));
            controller.SetActionState(eControllerActions.Left, Input.GetKey(KeyCode.Q));
            controller.SetActionState(eControllerActions.Right, Input.GetKey(KeyCode.D));
            controller.SetActionState(eControllerActions.Down, Input.GetKey(KeyCode.S));
            controller.SetActionState(eControllerActions.Up, Input.GetKey(KeyCode.Z));
            if(Input.GetKeyDown(KeyCode.A))
                Interact();

        }

        if (singleAction || continousAction)
        {
            if (continousAction)
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
                case Buttons.B:
                    controller.SetActionState(eControllerActions.Jump, true);
                    break;
                case Buttons.A:
                    Interact();
                    break;
                case Buttons.Joystick:
                    controller.SetActionState(eControllerActions.Left, false);
                    controller.SetActionState(eControllerActions.Right, false);
                    controller.SetActionState(eControllerActions.Down, false);
                    controller.SetActionState(eControllerActions.Up, false);
                    controller.SetActionState(TranslateJoystickInput(m.Extra1, m.Extra2), true);
                    break;
                case Buttons.DPadLeft:
                    controller.SetActionState(eControllerActions.Left, true);
                    break;
                case Buttons.DPadRight:
                    controller.SetActionState(eControllerActions.Right, true);
                    break;
                case Buttons.DPadDown:
                    controller.SetActionState(eControllerActions.Down, true);
                    break;
                case Buttons.DPadUp:
                    controller.SetActionState(eControllerActions.Up, true);
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
                case Buttons.Joystick:
                    controller.SetActionState(eControllerActions.Left, false);
                    controller.SetActionState(eControllerActions.Right, false);
                    controller.SetActionState(eControllerActions.Down, false);
                    controller.SetActionState(eControllerActions.Up, false);
                    break;
                case Buttons.B:
                    controller.SetActionState(eControllerActions.Jump, false);
                    break;
                case Buttons.A:
                    break;
            }

        }
    }

    public void SendGameTip(string origin, string content)
    {
        Socket.SendMessage(new GameTipSentMessage(origin, content));
    }

    private void Interact()
    {
        if(nearestInteraction != null)
        {
            nearestInteraction.Interact(this);
        }
    }

    private eControllerActions TranslateJoystickInput(int extra1, int extra2)
    {
        const int left = 180;
        const int right = 0;
        const int up =  90;
        const int down = 270;

        if (extra1 < 30 || extra1 > 330)
            return eControllerActions.Right;
        else if (extra1 < left + 30 && extra1 > left - 30)
            return eControllerActions.Left;
        else if (extra1 <= down + 30 && extra1 >= down - 30)
            return eControllerActions.Down;
        else if (extra1 <= up + 30 && extra1 >= up - 30)
            return eControllerActions.Up;
        else
            return eControllerActions.None;
    }

    public void SetInteraction(GameElement e)
    {
        Debug.Log("Player notified for interaction");
        if (nearestInteraction == null)
            nearestInteraction = e;
        else
        {
            if (Vector3.Distance(e.transform.position, transform.position) < Vector3.Distance(e.transform.position, transform.position))
                nearestInteraction = e;

        }
    }

    public void RemoveInteraction(GameElement e)
    {
        if (nearestInteraction == e)
            nearestInteraction = null;
    }
}
