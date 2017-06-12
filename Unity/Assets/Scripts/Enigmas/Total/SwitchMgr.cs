using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMgr : InteractableElementBehaviour
{
    public LockedWall wall;
    int switchCount = 0;

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            FootInteruptor fi = ie as FootInteruptor;
            if (fi.CurrentState() == FootInteruptor.eInteruptorState.Up) // if not already pressed
                switchCount++;
        }
    }

    public override void ObjectExitTrigger(InteractableElement ie, Object o)
    {
        if (ie is FootInteruptor)
        {
            FootInteruptor fi = ie as FootInteruptor;
            if (fi.CurrentState() == FootInteruptor.eInteruptorState.Down) // if not already released
                switchCount--;
        }
    }

    private void Update()
    {
        if (switchCount == 4)
            wall.Open();
    }
}
