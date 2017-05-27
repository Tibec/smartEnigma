using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : InteractableElementBehaviour
{

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        SceneManager.LoadScene("Scene/Menu", LoadSceneMode.Single);
    }
}
