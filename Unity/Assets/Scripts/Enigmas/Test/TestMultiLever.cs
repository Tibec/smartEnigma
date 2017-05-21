using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMultiLever : InteractableElementBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        p.SendGameTip(ie.name, p.Username + " m'a actionner !");
    }
}
