using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CollectableElement : GameElement {

    public int ItemID;
    public string ItemName;
    public string ItemDescription;
    private Transform originalParent;
    private Player owner;
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Collider2D[] collision;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        originalParent = transform.parent;
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collision = GetComponents<Collider2D>();
    }

    // Update is called once per frame
    void Update () {

	}

    private void SetVisibility(bool visible)
    {
        body.isKinematic = !visible;
        sprite.enabled = visible;
        foreach(Collider2D coll in collision)
            coll.enabled = visible;
    }

    public override void Interact(Player p)
    {
        if (owner != null)
            throw new AlreadyOwnItemException();
        transform.parent = p.transform;
        transform.localPosition = Vector3.zero;
        owner = p;
        SetVisibility(false);
    }

    virtual public void Use(Player p)
    {

    }

    public void Throw()
    {
        transform.parent = originalParent;
        SetVisibility(true);
        owner = null;
    }
}

public class AlreadyOwnItemException : Exception { }