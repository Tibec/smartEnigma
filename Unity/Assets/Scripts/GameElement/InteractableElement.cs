using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElement : GameElement
{
    public GameObject notifyTarget;
    // Use this for initialization
    void Start()
    {
    }

// Update is called once per frame
void Update()
    {

    }

    public override void Interact(Player p)
    {
        if (notifyTarget == null)
            return;
        InteractableElementBehaviour[] callbacks = notifyTarget.GetComponents<InteractableElementBehaviour>();
        foreach(var callback in callbacks)
        {
            callback.OnInteraction(p);
        }
    }

}
