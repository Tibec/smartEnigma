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
    private bool deleteWall = false;
    public int despawnSpeed = 10; // frame
    private int counter = 0;

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
        if (deleteWall && counter <= 0)
        {
            if (LinkedTiles.Count == 0)
            {
                tiledata.Map.Erase(tiledata.Position);
                deleteWall = false;
            }
            else
            {
                int target = UnityEngine.Random.Range(0, LinkedTiles.Count);
                Vector2 pos = LinkedTiles[target];
                tiledata.Map.Erase(pos);
                LinkedTiles.RemoveAt(target);
                counter = despawnSpeed;
            }
            tiledata.Map.UpdateMesh();
        }
        --counter;
    }

    public override void Interact(Player p)
    {
        if (currentState == eWallState.Open)
            return;

        if(controller != null)
        {
            controller.OnInteraction(this, p);
        }
    }

    public void Open()
    {
        if (currentState == eWallState.Closed)
        {
            currentState = eWallState.Open;
        }

        deleteWall = true;        
    }

    public eWallState CurrentState()
    {
        return currentState;
    }

}
