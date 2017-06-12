using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SwitchTip : InteractableElementBehaviour {

	private void Awake()
	{
	}

	public override void ObjectEnterTrigger(InteractableElement ie, Object o)
	{
		if (o is Player) {
			Player p = (Player)o;
            if (ie.name == "TriggerArea1")
                p.SendGameTip("indice", " Jette un coup d'oeil au dessus :)");
            if (ie.name == "TriggerArea2")
                p.SendGameTip("indice", "Tout à gauche");
        }
    }

	void Update()
	{

	}
}
