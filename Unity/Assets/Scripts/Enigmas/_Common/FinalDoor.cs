using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : InteractableElementBehaviour {

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        Utils.LoadScene("Scene/EnigmaClear");
    }
}
