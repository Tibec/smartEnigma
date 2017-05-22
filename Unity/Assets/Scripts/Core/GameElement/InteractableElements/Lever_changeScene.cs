using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_changeScene : Lever {

	public void executeAction()
	{
		Application.LoadLevel("EnigmaSelect");
	}
}
