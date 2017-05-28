using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaSwitch : InteractableElementBehaviour {

    int switchBack = -1;
    HandButton button;

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        if (button == null)
            button = ie as HandButton;

        button.SetState(HandButton.eButtonState.Green);
        switchBack = 10;
        RequestNextPage();
    }

    private void Update()
    {
        if (switchBack > 0)
            --switchBack;
        else if(switchBack == 0)
        {
            button.SetState(HandButton.eButtonState.Red);
        }
    }

    public delegate void StateChanged();
    public event StateChanged RequestNextPage;
}
