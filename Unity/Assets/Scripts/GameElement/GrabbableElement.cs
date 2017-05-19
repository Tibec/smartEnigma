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
    public float LauncheForce;
    
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(body, "A GrabbableElement must have a Rigidbody2D !");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(Player p)
    {
        owner = p;
        this.transform.parent = p.transform;
    }

    virtual public void Use(Player p)
    {

    }

    public void Throw()
    {
        transform.parent = GetComponentInParent<Transform>();
        body.AddForce(owner.GetComponent<PlayerAnimation>().IsSpriteFacingRight ? ComputeLaunchVector() : -1 * ComputeLaunchVector());
        owner = null;
    }

    private Vector2 ComputeLaunchVector()
    {
        Vector2 v;
        v.x = LauncheForce * Mathf.Cos(LaunchAngle);
        v.y = LauncheForce * Mathf.Sin(LaunchAngle);
        return v;
    }

}
