using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TileData))]
public class Door : InteractableElement {
    [Serializable]
    public class DoorInfo { public uint bodyTile; public uint headTile; }

    private TileData tiledata;
    public List<DoorInfo> TileId = new List<DoorInfo>();

    public enum eDoorState { Closed = 0, Open = 1 };
    public eDoorState CurrentState { get; protected set; }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A Door require a TileData component!");
        CurrentState = (eDoorState)tiledata.Tile.paramContainer.GetIntParam("state");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        NotifyController(p);
    }

    public void SetState(eDoorState state)
    {
        CurrentState = state;
        tiledata.Map.SetTileData(tiledata.Position, TileId[(int)state].bodyTile);
        tiledata.Map.SetTileData(tiledata.Position + Vector2.up, TileId[(int)state].headTile);
        tiledata.Map.UpdateMesh();
    }
}
