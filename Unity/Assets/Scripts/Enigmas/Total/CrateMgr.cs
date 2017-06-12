using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateMgr : InteractableElementBehaviour
{

    public GameObject crate;
    public FootInteruptor crateTarget;

    private bool crateOK = false;

    public override void ObjectEnterTrigger(InteractableElement ie, Object o)
    {
        if (!crateOK && 
                (ie.name == "FootInteruptorYellow" && o is Player)
            || (ie.name == "RespawnCrateTrigger" && o is GrabbableElement) )
        {
            ResetCrate();
        }
    }

    private void Update()
    {
        if(crateTarget.CurrentState() == FootInteruptor.eInteruptorState.Down)
        {
            crateOK = true;
        }
    }

    public override void ObjectExitTrigger(InteractableElement ie, Object o)
    {
        if (!crateOK && ie.name == "FootInteruptorYellow" && o is Player)
        {
            crate.SetActive(false);
        }

    }
    private void ResetCrate()
    {
        Player p = crate.GetComponentInParent<Player>();
        if (p != null)
        {
            p.ReleaseGrabbedElement();
        }

        crate.transform.position = new Vector3(22.5f, 5.5f, 0f);
        crate.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        crate.SetActive(true);
    }
}
