using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[RequireComponent(typeof(TileData))]
public class Lever : InteractableElement 
{
    public enum eLeverState { Left = 0, Middle = 1, Right = 2};
    private eLeverState lastState;
    private eLeverState currentState;
    private TileData tiledata;
    public List<uint> TileId = new List<uint>();
    public bool DualStateOnly = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A lever require a TileData component!");
        currentState = (eLeverState)tiledata.Tile.paramContainer.GetIntParam("state");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override void Interact(Player p)
    {
        if(InteractionAllowed(p))
        {
            NotifyController(p);
            ChangeState();

			executeAction ();
        }
    }

	public virtual void executeAction()
	{
		
	}

    private void ChangeState()
    {
        if(currentState == eLeverState.Left || currentState == eLeverState.Right)
        {
            lastState = currentState;
            currentState = DualStateOnly ? (currentState == eLeverState.Left) ? eLeverState.Right : eLeverState.Left : eLeverState.Middle;
        }
        else // if(currentState == eLeverState.Middle)
        {
            currentState = (lastState == eLeverState.Left) ? eLeverState.Right : eLeverState.Left;
            lastState = eLeverState.Middle;
        }
        tiledata.Map.SetTileData(tiledata.Position, TileId[(int)currentState]);
        tiledata.Map.UpdateMesh();
    }

    public eLeverState CurrentState()
    {
        return currentState;
    }
}
