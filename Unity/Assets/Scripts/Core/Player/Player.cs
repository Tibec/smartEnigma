using CreativeSpore.SmartColliders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

    public Connection Socket { get; set; }
    public string SocketID { get; set; }
    public string Username { get; set; }
    public bool Connected { get; set; }
    public string Key { get; set; }

    public Color Coloration { get { return sprite.color; } set { sprite.color = value; } }
    private SpriteRenderer sprite;
    private List<GameElement> availableInteraction;
    private GameElement nearestInteraction;
    private GrabbableElement grabbedElement;
    private CollectableElement heldElement;

    public CollectableElement HeldElement { get { return heldElement; } }

    public bool CarrySomething { get { return grabbedElement != null; } }

    // Test things
    [Header("Debug")]
    public bool singleAction;
    public bool continousAction;
    private bool continousActionActive;
    public Buttons InputTest;
    public bool enableKeyboard = false;

    private PlayerController controller;
    private UILabel interactionLabel;
    private UIPanel interactionPanel;
    private UI2DSprite climbSprite;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        availableInteraction = new List<GameElement>();
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
            if(Input.GetKeyDown(KeyCode.T))
            {
                heldElement.Throw();
                heldElement = null;
            }
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

        if(nearestInteraction == null)
        {
            RemoveInteraction(null);
        }

        PlayerController ctl = GetComponent<PlayerController>();

        if (ctl.CanClimb && !CarrySomething)
            ShowIndicator(false);
        else
            HideIndicator(false);

        UpdateNearestInteraction();
    }

    private void UpdateNearestInteraction()
    {
        if (availableInteraction.Count == 0)
            return;

        GameElement nearest = availableInteraction[0];

        for(int i=1; i<availableInteraction.Count; ++i)
        {
            if (Vector3.Distance(nearest.transform.position, transform.position) > Vector3.Distance(availableInteraction[i].transform.position, transform.position))
                nearest = availableInteraction[i];
        }
        if(!CarrySomething)
            SetNearestInteraction(nearest);
    }

    private void ShowIndicator(bool interaction, string text = "")
    {
        UI2DSprite climbSprite = Array.Find(GetComponentsInChildren<UI2DSprite>(), (s) => s.name == "ActionClimb");
        UI2DSprite actionSprite = Array.Find(GetComponentsInChildren<UI2DSprite>(), (s) => s.name == "ActionBackground");
        UILabel actiontText = GetComponentInChildren<UILabel>();

        if(interaction)
        {
            actionSprite.enabled = true;
            actiontText.enabled = true;
            actiontText.text = text;
        }
        else
        {
            climbSprite.enabled = true;
        }
    }

    private void HideIndicator(bool interaction)
    {
        UIPanel panel = GetComponentInChildren<UIPanel>();
        UI2DSprite climbSprite = Array.Find(GetComponentsInChildren<UI2DSprite>(), (s) => s.name == "ActionClimb");
        UI2DSprite actionSprite = Array.Find(GetComponentsInChildren<UI2DSprite>(), (s) => s.name == "ActionBackground");
        UILabel actiontText = GetComponentInChildren<UILabel>();

        if(interaction)
        {
            actionSprite.enabled = false;
            actiontText.enabled = false;
        }
        else
        {
            climbSprite.enabled = false;
        }
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
        else if(mess is ItemThrowMessage)
        {
            if(heldElement!=null)
            {
                heldElement.Throw();
                heldElement = null;
                Socket.SendMessage(new UpdateItemMessage(0, "", ""));
            }
        }
    }

    public void SendGameTip(string origin, string content)
    {
        Socket.SendMessage(new GameTipSentMessage(origin, content));
        PlayerInfoManager pim = FindObjectOfType<PlayerInfoManager>();
        pim.HighlightPlayer(this);
    }

    public void ReleaseGrabbedElement()
	{
		if(grabbedElement != null)
		{
			grabbedElement.Release();
			grabbedElement = null;
			return;
		}
	}

    public void DeleteHeldItem()
    {
        if (grabbedElement != null)
        {
            grabbedElement.Release();
            Destroy(grabbedElement.gameObject);
            grabbedElement = null;
            return;
        }
    }

    private void Interact()
    {
        // first check if something is held
        if(grabbedElement != null)
        {
            grabbedElement.Throw();
            grabbedElement = null;
            return;
        }

        // Next check if an interaction is available
        if (nearestInteraction != null)
        {
            GameElement elem = nearestInteraction;
            try
            {
                elem.Interact(this);
            }
            catch(Exception e)
            {

            }

            if (elem is GrabbableElement)
            {
                grabbedElement = elem as GrabbableElement;
                HideIndicator(true);
            }
            if (elem is CollectableElement)
            {
                if (heldElement == null)
                {
                    heldElement = elem as CollectableElement;
                    Socket.SendMessage(new UpdateItemMessage(heldElement.ItemID, heldElement.ItemDescription, heldElement.ItemName));
                }
                else
                    (elem as CollectableElement).Throw();
            }
            nearestInteraction = null;
            return;
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
        Debug.Log("Player "+Username+" got notified for interaction");
        availableInteraction.Add(e);
    }

    public void Disconnect()
    {
        Socket.SendMessage(new KickMessage());
        Socket.Disconnect();
    }

    private void SetNearestInteraction(GameElement e)
    {
        nearestInteraction = e;
        if(!string.IsNullOrEmpty(e.InteractText))
        {
            ShowIndicator(true, e.InteractText);
        }
    }

    public void RemoveInteraction(GameElement e)
    {
        availableInteraction.Remove(e);

        HideIndicator(true);
    }
}
