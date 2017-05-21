﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElementBehaviour : MonoBehaviour {
    public virtual void OnInteraction(InteractableElement ie, Player p)
    {

    }

    public virtual bool CanInteract(InteractableElement ie, Player p)
    {
        return true;
    }

    public virtual bool PlayerEnterTrigger(InteractableElement ie, Player p)
    {
        return true;
    }

    public virtual bool PlayerExitTrigger(InteractableElement ie, Player p)
    {
        return true;
    }
}