using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableElementBehaviour : MonoBehaviour {
    public virtual void OnInteraction(InteractableElement ie, Player p)
    {

    }

    public virtual void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
		
    }

    public virtual void ObjectExitTrigger(InteractableElement ie, Object o)
    {

    }
}
