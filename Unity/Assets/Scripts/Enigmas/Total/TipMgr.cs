using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipMgr : InteractableElementBehaviour {

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if( o is Player)
        {
            Player p = o as Player;
            if(ie.name == "CrateTipArea")
                p.SendGameTip("Info", "Les humains ne peuvent passer ...");
            else if(ie.name == "HiddenSwitchTipArea")
                p.SendGameTip("Info", "Il y a quelque chose à droite.");
        }
    }
}
