using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationDotCreator : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public void CreateDot(Transform storedTransform, Transform parent)
    {
        GameObject.Instantiate(prefab, storedTransform.position, storedTransform.rotation, parent);
    }
}
