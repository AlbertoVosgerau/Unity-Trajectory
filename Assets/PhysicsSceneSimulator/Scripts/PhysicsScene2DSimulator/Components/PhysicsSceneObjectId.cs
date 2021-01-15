using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSceneObjectId : MonoBehaviour
{
    public string Id => _id;
    private string _id;

    public void SetId(string id)
    {
        _id = id;
    }
}
