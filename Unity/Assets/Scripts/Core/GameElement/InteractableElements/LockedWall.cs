using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

[RequireComponent(typeof(TileData))]
public class LockedWall : InteractableElement
{
    public enum eWallState { Closed = 0, Open = 1};
    private eWallState currentState;
    private TileData tiledata;
    public List<Vector2> LinkedTiles = new List<Vector2>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A LockedWall require a TileData component!");
        currentState = eWallState.Closed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        if (currentState == eWallState.Open)
            return;

        if (InteractionAllowed(p))
        {
            ChangeState();
            NotifyController(p);
        }
    }

    private void ChangeState()
    {
        if (currentState == eWallState.Closed)
        {
            currentState = eWallState.Open;
        }

        // TODO:
        //tiledata.Map.SetTileData(tiledata.Position, TileId[(int)currentState]);
        tiledata.Map.UpdateMesh();
    }

    public eWallState CurrentState()
    {
        return currentState;
    }

}
