using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleportZone : MonoBehaviour
{
    [SerializeField] private CustomPhysicsScene2DUpdater onEnterScene;
    [SerializeField] private CustomPhysicsScene2DUpdater onExitScene;
}
