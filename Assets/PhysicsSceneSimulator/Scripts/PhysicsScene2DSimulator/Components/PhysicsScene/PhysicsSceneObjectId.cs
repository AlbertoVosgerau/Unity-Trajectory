using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicsSceneObjectId : MonoBehaviour
{
    public bool IsOriginal => _isOriginal;
    public string Id => _id;
    private string _id;
    private bool _isOriginal = true;

    public void SetId(string id)
    {
        _id = id;
    }

    public void SetIsOriginal(bool isOriginal)
    {
        _isOriginal = isOriginal;
    }
    private void OnDestroy()
    {
        Debug.Log($"Destroyed {gameObject.name}");
    }
}
