using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectileAction : BaseProjectileAction
{
    public override void PhysicsAction(GameObject actionObject)
    {
        Rigidbody2D rb = actionObject.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(15,15), ForceMode2D.Impulse);
    }
}
