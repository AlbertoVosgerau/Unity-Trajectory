using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public float time;
    public float fixedTime;

    void Update()
    {
        time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        fixedTime += Time.fixedDeltaTime;
    }
}
