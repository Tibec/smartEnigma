using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SwitchDoor : InteractableElementBehaviour {

	// number of switches maintained
	private int i = 0;

    private void Awake()
    {
    }

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
		if (ie is FootInteruptor) {
			i++;
		}
    }

	public override void OnInteraction(InteractableElement ie, Player p)
	{
		if (ie is Door) {
			if (i == 3)
				Debug.Log ("NIVEAU TERMINE");
			else
				p.SendGameTip(ie.name, " As tu actionné TOUS les intterupteurs ?");
		}
	} 

	public override void ObjectExitTrigger(InteractableElement ie, Object o)
	{
		if (ie is FootInteruptor) {
			i--;
		}
	}

    void Update()
    {

    }
}
