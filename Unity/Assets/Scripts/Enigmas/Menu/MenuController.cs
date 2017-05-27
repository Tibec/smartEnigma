using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class MenuController : InteractableElementBehaviour
{
    private PlayerMgr mgr;

    public void Start()
    {
        mgr = FindObjectOfType<PlayerMgr>();
        Assert.IsNotNull(mgr, "Cannot found PlayerMgr !");    
    }

    public override bool CanInteract(InteractableElement ie, Player p)
    {
       // if (mgr.Players.Count < 2 && ie.GetType() == typeof(Lever))
       //     return false;
        return true;
    }

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        if(ie.GetType() == typeof(Lever))
        {
            if (ie.name == "LeverStart")
                SceneManager.LoadScene("Scene/EnigmaSelect", LoadSceneMode.Single);
            if (ie.name == "LeverTest")
                SceneManager.LoadScene("Scene/Test", LoadSceneMode.Single);
        }
    }

    public override void PlayerEnterTrigger(InteractableElement ie, Player p)
    {
        if(ie.GetType() != typeof(Lever))
        {
            mgr.RemovePlayer(p.Username);
        }
    }
}
