using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[RequireComponent(typeof(TileData))]
public class FootInteruptor : InteractableElement
{
    public enum eInteruptorState { Up = 0, Down = 1};
    private eInteruptorState currentState;
    private TileData tiledata;
    public List<uint> TileId = new List<uint>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A FootInteruptor require a TileData component!");
        currentState = (eInteruptorState)tiledata.Tile.paramContainer.GetIntParam("state");
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void PlayerTriggerEnter(Player p)
    {
        SetState(eInteruptorState.Down);
        base.PlayerTriggerEnter(p);
    }

    protected override void PlayerTriggerExit(Player p)
    {
        SetState(eInteruptorState.Up);
        base.PlayerTriggerExit(p);
    }

    protected override void GrabbableTriggerEnter(GrabbableElement p)
    {
        SetState(eInteruptorState.Down);
        base.GrabbableTriggerEnter(p);
    }

    protected override void GrabbableTriggerExit(GrabbableElement p)
    {
        SetState(eInteruptorState.Up);
        base.GrabbableTriggerExit(p);
    }

    private void SetState(eInteruptorState state)
    {
        currentState = state;
        tiledata.Map.SetTileData(tiledata.Position, TileId[(int)currentState]);
        tiledata.Map.UpdateMesh();
    }

    public eInteruptorState CurrentState()
    {
        return currentState;
    }
}
