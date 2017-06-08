using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
public class GrabbableElement : GameElement
{
    protected Rigidbody2D body;
    protected Player owner;
    public float LaunchAngle;
    public float LaunchForce;
    private Transform originalParent;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        InteractText = "Saisir";
        body = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(body, "A GrabbableElement must have a Rigidbody2D !");
        originalParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        if (owner != null)
            throw new AlreadyGrabbedException();
        owner = p;
        transform.parent = p.transform;
        body.isKinematic = true;
        body.velocity = Vector2.zero;
        body.inertia = 0f;
        PlayerAnimation pa = p.GetComponent<PlayerAnimation>();
        pa.OnOrientationChanged += FlipTransform;
        Vector3 pos = transform.localPosition;
        pos.y = 0.75f;
        pos.x = pa.IsSpriteFacingRight ? 1.2f  : - 1.2f;
        transform.localPosition = pos;
    }

    private void FlipTransform(eControllerActions direction)
    {
        Vector3 pos = transform.localPosition;
        pos.x = direction == eControllerActions.Left ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
        transform.localPosition = pos;
    }

    virtual public void Use(Player p)
    {

    }

    public void Throw()
    {
        PlayerAnimation pa = owner.GetComponent<PlayerAnimation>();
        pa.OnOrientationChanged -= FlipTransform;
        body.isKinematic = false;
        transform.parent = originalParent;
        body.AddForce(pa.IsSpriteFacingRight ? ComputeLaunchVector() : Vector2.Scale (new Vector2(-1, 1), ComputeLaunchVector()));
        owner = null;
    }

	public void Release()
	{
		PlayerAnimation pa = owner.GetComponent<PlayerAnimation>();
		pa.OnOrientationChanged -= FlipTransform;
		body.isKinematic = false;
		transform.parent = originalParent;
		owner = null;
	}

    private Vector2 ComputeLaunchVector()
    {
        Vector2 v;
        v.x = LaunchForce * Mathf.Cos((LaunchAngle * Mathf.PI) / 360);
        v.y = LaunchForce * Mathf.Sin((LaunchAngle * Mathf.PI) / 360);
        return v;
    }

}
public class AlreadyGrabbedException : Exception { }
