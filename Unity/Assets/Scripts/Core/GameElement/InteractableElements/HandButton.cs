using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[RequireComponent(typeof(TileData))]
public class HandButton : InteractableElement
{
    public enum eButtonState { Red = 0, Green = 1};
    private eButtonState currentState;
    private TileData tiledata;
    public List<uint> TileId = new List<uint>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A HandButton require a TileData component!");
        currentState = (eButtonState)tiledata.Tile.paramContainer.GetIntParam("state");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        NotifyController(p);
    }

    public void SetState(eButtonState state)
    {
        currentState = state;
        tiledata.Map.SetTileData(tiledata.Position, TileId[(int)currentState]);
        tiledata.Map.UpdateMesh();
    }

    public eButtonState CurrentState()
    {
        return currentState;
    }
}
