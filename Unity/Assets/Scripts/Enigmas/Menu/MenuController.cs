using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class MenuController : InteractableElementBehaviour
{
    private PlayerMgr mgr;
    private SceneLoader loader;
    public void Awake()
    {
        mgr = FindObjectOfType<PlayerMgr>();
        Assert.IsNotNull(mgr, "Cannot found PlayerMgr !");  
        loader = FindObjectOfType<SceneLoader>();
        Assert.IsNotNull(loader, "Cannot found SceneLoader !");
    }

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        if(ie.GetType() == typeof(Lever))
        {
            if (ie.name == "LeverStart" && PlayerMgr.Instance().Players.Length >= 2)
                loader.LoadScene("Scene/EnigmaSelect");
            if (ie.name == "LeverTest")
                loader.LoadScene("Scene/Test");
        }
    }

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if(ie.GetType() != typeof(Lever) && o is Player)
        {
            Player p = o as Player;
            mgr.RemovePlayer(p.Username);
        }
    }
}
