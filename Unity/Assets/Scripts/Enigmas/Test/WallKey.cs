using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKey : InteractableElementBehaviour {

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        if(ie is LockedWall)
        {
            LockedWall w = ie as LockedWall;
            if (p.HeldElement.ItemID == 1)
            {
                p.DeleteHeldItem();
                w.Open();
            }
        }
    }
}
