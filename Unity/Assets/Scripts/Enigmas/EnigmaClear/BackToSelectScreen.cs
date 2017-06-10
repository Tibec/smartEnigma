using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BackToSelectScreen : InteractableElementBehaviour {
    public override void OnInteraction(InteractableElement ie, Player p)
    {
        Utils.LoadScene("EnigmaSelect");
    }
}
