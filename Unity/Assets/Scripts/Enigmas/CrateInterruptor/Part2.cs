using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part2 : InteractableElementBehaviour
{

    private bool trigger1Ok = false;
    private bool trigger2Ok = false;
    private bool trigger3Ok = false;
    private bool trigger4Ok = false;
    private bool trigger5Ok = false;

    public LockedWall targetWall;

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if ((ie as FootInteruptor).CurrentState() != FootInteruptor.eInteruptorState.Up)
                return;
            if (ie.name == "Inter10")
                trigger1Ok = true;
            else if (ie.name == "Inter11")
                trigger2Ok = true;
            else if (ie.name == "Inter12")
                trigger3Ok = true;
            else if (ie.name == "Inter13")
                trigger4Ok = true;
            else if (ie.name == "Inter14")
                trigger5Ok = true;
        }
    }

    public override void ObjectExitTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            if ((ie as FootInteruptor).CurrentState() != FootInteruptor.eInteruptorState.Down)
                return;
            if (ie.name == "Inter10")
                trigger1Ok = false;
            else if (ie.name == "Inter11")
                trigger2Ok = false;
            else if (ie.name == "Inter12")
                trigger3Ok = false;
            else if (ie.name == "Inter13")
                trigger4Ok = false;
            else if (ie.name == "Inter14")
                trigger5Ok = false;
        }
    }

    void Update()
    {
        if (trigger1Ok && trigger2Ok && trigger3Ok && trigger4Ok && trigger5Ok)
        {
            if (targetWall != null)
                targetWall.Open();
        }
    }
}
