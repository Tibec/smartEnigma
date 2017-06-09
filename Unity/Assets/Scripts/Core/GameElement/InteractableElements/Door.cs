using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(TileData))]
public class Door : InteractableElement {
    private TileData tiledata;
    public List<uint> TileId = new List<uint>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tiledata = GetComponent<TileData>();
        Assert.IsNotNull(tiledata, "A Door require a TileData component!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        NotifyController(p);
    }

}
