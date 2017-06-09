using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateRespawn : InteractableElementBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void ObjectEnterTrigger (InteractableElement ie, Object o)
	{
		if (o is InteractableElement) {
            InteractableElement c = o as InteractableElement;
			Player p = c.GetComponentInParent<Player> ();
			if (p != null) {
				p.ReleaseGrabbedElement ();
			}
			c.transform.position = new Vector3 (-8.5f, 3.5f, 0f);
		}
	}
}
