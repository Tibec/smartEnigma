using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class EnigmaDoor : InteractableElementBehaviour {

    EnigmaLoader loader;

    private void Awake()
    {
        loader = transform.parent.GetComponent<EnigmaLoader>();
        Assert.IsNotNull(loader, "Cannot found EnigmaLoader for EnigmaDoors");
    }

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        int toLoad;
        if (ie.name == "DoorLv1")
            toLoad = 0;
        else if (ie.name == "DoorLv2")
            toLoad = 1;
        else //  if(ie.name == "DoorLv3")
            toLoad = 2;

        if (loader.VisibleEnigmas[0] != null)
            SceneManager.LoadScene(loader.VisibleEnigmas[toLoad].Scene);
        else
            Debug.LogError("Cannot open door " + (toLoad + 1) + " because theres no engima behind");

    }

    void Update()
    {

    }
}
