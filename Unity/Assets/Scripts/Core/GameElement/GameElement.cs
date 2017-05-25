using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public class GameElement : MonoBehaviour {

    public string InteractText; // If null no interaction popup will be shown
    protected Collider2D triggerArea; 

	// Use this for initialization
	protected virtual void Start () {
        Collider2D[] colliders = GetComponents<Collider2D>();	
        foreach(Collider2D c in colliders)
            if (c.isTrigger)
                triggerArea = c;
        Assert.IsNotNull(triggerArea, "There must be at least one collider set as trigger on this gameobject !");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    virtual public void Interact(Player p)
    {

    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            PlayerTriggerEnter(player);
            if (!string.IsNullOrEmpty(InteractText))
            {
                player.SetInteraction(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            PlayerTriggerExit(player);
            player.RemoveInteraction(this);
        }
    }

    protected virtual void PlayerTriggerEnter(Player p)
    {

    }

    protected virtual void PlayerTriggerExit(Player p)
    {

    }

    private void OnDestroy()
    {

    }
}
