using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part1 : InteractableElementBehaviour {

    private bool trigger1Ok = false;
    private bool trigger2Ok = false;

    public LockedWall targetWall; 

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if (ie.name == "Inter01")
                trigger1Ok = true;
            else if (ie.name == "Inter02")
                trigger2Ok = true;
        }
    }

    public override void ObjectExitTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if (ie.name == "Inter01")
                trigger1Ok = false;
            else if (ie.name == "Inter02")
                trigger2Ok = false;
        }
    }

    private void Update()
    {
        if(trigger1Ok && trigger2Ok)
        {
            if (targetWall != null)
                targetWall.Open();
        }
    }
}
