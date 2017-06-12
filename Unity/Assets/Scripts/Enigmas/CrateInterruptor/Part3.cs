using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part3 : InteractableElementBehaviour
{

    private bool triggerOk = false;

    public LockedWall targetWall;

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if (ie.name == "Inter20")
                triggerOk = true;
        }
    }

    public override void ObjectExitTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if (ie.name == "Inter20")
                triggerOk = false;
        }
    }

    private void Update()
    {
        if (triggerOk)
        {
            if (targetWall != null)
                targetWall.Open();
        }
    }
}