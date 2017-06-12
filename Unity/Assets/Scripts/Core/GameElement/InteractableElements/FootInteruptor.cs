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
    private int entityIn = 0;

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
        base.PlayerTriggerEnter(p);
        ++entityIn;
        SetState(eInteruptorState.Down);
    }

    protected override void PlayerTriggerExit(Player p)
    {
        base.PlayerTriggerExit(p);
        --entityIn;
        if(entityIn == 0)
            SetState(eInteruptorState.Up);
    }

    protected override void GrabbableTriggerEnter(GrabbableElement p)
    {
        base.GrabbableTriggerEnter(p);
        ++entityIn;
        SetState(eInteruptorState.Down);
    }

    protected override void GrabbableTriggerExit(GrabbableElement p)
    {
        base.GrabbableTriggerExit(p);
        --entityIn;
        if (entityIn == 0)
            SetState(eInteruptorState.Up);
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
