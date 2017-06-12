using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class SwitchDoor : InteractableElementBehaviour {

	// number of switches maintained
	private int i = 0;
    private bool doorOpen = false;
    public Door door;
    private void Start()
    {
        door.SetState(Door.eDoorState.Closed);
    }

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
		if (ie is FootInteruptor) {
            FootInteruptor fi = ie as FootInteruptor;
            if(fi.CurrentState() == FootInteruptor.eInteruptorState.Up) // if not already pressed
			    i++;
		}
    }

	public override void OnInteraction(InteractableElement ie, Player p)
	{
		if (ie is Door) {
			if (doorOpen)
				Utils.LoadScene ("Scene/EnigmaClear");
			else
				p.SendGameTip(ie.name, " As tu actionné TOUS les intterupteurs ?");
		}
	} 

	public override void ObjectExitTrigger(InteractableElement ie, Object o)
	{
        FootInteruptor fi = ie as FootInteruptor;
        if (fi.CurrentState() == FootInteruptor.eInteruptorState.Down) // if not already released
            i--;
    }

    void Update()
    {
        if( !doorOpen &&  i == 3 )
        {
            doorOpen = true;
            door.SetState(Door.eDoorState.Open);
        }
    }
}
