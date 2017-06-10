using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class ExitDoor : InteractableElementBehaviour
{

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        Assert.IsNotNull(loader, "Cannot found SceneLoader !");

        if (ie.name == "ExitDoor")
            loader.LoadScene("Scene/Menu");
        else if(ie.name == "SecretDoor")
            loader.LoadScene("Test");
    }
}
