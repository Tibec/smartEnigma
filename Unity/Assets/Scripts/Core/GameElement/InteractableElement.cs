using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElement : GameElement
{
    public InteractableElementBehaviour controller;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {

    }

    protected void NotifyController(Player p)
    {
        if (controller == null)
            return;

        controller.OnInteraction(this, p);
    }

    protected bool InteractionAllowed(Player p)
    {
        if (controller == null)
            return true;

        return controller.CanInteract(this, p);
    }

    protected override void PlayerTriggerEnter(Player p)
    {
        if (controller == null)
            return;

        controller.PlayerEnterTrigger(this, p);
    }

    protected override void PlayerTriggerExit(Player p)
    {
        if (controller == null)
            return;

        controller.PlayerExitTrigger(this, p);
    }

}
