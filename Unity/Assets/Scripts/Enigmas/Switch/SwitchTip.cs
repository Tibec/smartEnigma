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
			p.SendGameTip("indice", " Jette un coup d'oeil au dessus ;)");
		}
	}

	void Update()
	{

	}
}
