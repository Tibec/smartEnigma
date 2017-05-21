using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableElement : GameElement {

    public int ItemID;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update () {

	}

    public override void Interact(Player p)
    {
        /*
        if(p.AddInventory(ItemID))
        {
            this.enabled = false;
            this.transform.parent = p.transform;
        }
        else
        {
            p.SendMessage("Votre inventaire est plein !");
        }
        */
    }

    virtual public void Use(Player p)
    {

    }

    public void Throw()
    {
        this.transform.parent = this.GetComponentInParent<Transform>();
    }
}
