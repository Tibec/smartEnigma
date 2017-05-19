using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableElement 
{
    public enum eLeverState { Left = 0, Middle = 1, Right = 2};

    public List<uint> TileId = new List<uint>();
    public Tileset tileset;

    // Use this for initialization
    void Start () {
        Debug.Log("Levier créer");
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact(Player p)
    {
        Debug.Log("On m'a actionner !");
    }
}
