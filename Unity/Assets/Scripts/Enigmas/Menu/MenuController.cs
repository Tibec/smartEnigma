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
        if (mgr.Players.Count < 2)
            return false;
        return true;
    }

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        SceneManager.LoadScene("Scene/TestEnigmaSelect", LoadSceneMode.Single);
    }
}
