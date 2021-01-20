using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsSceneContextMenu : Editor
{
    [MenuItem("GameObject/Physics Scene Simulator/Create Scene/Simluation Scene 2D", false, 0)]
    public static void CreateSimulationScene2D()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<SimulationPhysicScene2DUpdater>();
        newObject.name = "Simulation Scene: SpeedUp 1x";
    }

    [MenuItem("GameObject/Physics Scene Simulator/Create Scene/Custom Scene 2D", false, 0)]
    public static void CreateCustomScene2D()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<CustomPhysicsScene2DUpdater>();
        newObject.name = "Custom Scene: SpeedUp 1x";
    }

    [MenuItem("GameObject/Physics Scene Simulator/Scene Teleport Area/Create or Convert to Scene Teleport Area 2D", false, 0)]
    public static void CreateSceneTeleportArea2D()
    {
        GameObject teleportArea = Selection.activeObject == null? new GameObject() : Selection.activeGameObject;

        Rigidbody rb = teleportArea.GetComponent<Rigidbody>();
        if (rb != null)
            DestroyImmediate(rb);

        Collider col = teleportArea.GetComponent<Collider>();
        if (col != null)
            DestroyImmediate(col);

        Collider2D col2D = teleportArea.GetComponent<Collider2D>();
        if (col2D == null)
            col2D = teleportArea.AddComponent<BoxCollider2D>();

        col2D.isTrigger = true;

        if(teleportArea.GetComponent<SceneTeleportArea2D>() == null)
            teleportArea.AddComponent<SceneTeleportArea2D>();

        PhysicsScene2DCloneHandler cloneHandler = teleportArea.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler == null)
            cloneHandler = teleportArea.AddComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.includeSimulationPhysics = true;
        cloneHandler.syncTransform = false;

        teleportArea.name = "SceneTeleport - OnEnter: Current | OnExit: Current";
    }

    [MenuItem("GameObject/Physics Scene Simulator/Make Physics Scenes Interactable 2D", false, -10)]
    public static void MakeMultiSceneInteractable2D()
    {
        if (Selection.activeObject == null)
            return;

        GameObject activeObject = Selection.activeGameObject;

        Rigidbody rb = activeObject.GetComponent<Rigidbody>();
        if (rb != null)
            DestroyImmediate(rb);

        Collider col = activeObject.GetComponent<Collider>();
        if (col != null)
            DestroyImmediate(col);

        if (activeObject.GetComponent<Collider2D>() == null)
            activeObject.AddComponent<BoxCollider2D>();

        PhysicsScene2DCloneHandler cloneHandler = activeObject.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler == null)
            cloneHandler = activeObject.AddComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.includeSimulationPhysics = true;
    }

    [MenuItem("GameObject/Physics Scene Simulator/Projection Objects/Convert to Simple Projection Object 2D", false, 0)]
    public static void ConvertToSimpleProjectionObject2D()
    {
        if (Selection.activeObject == null)
            return;

        SimulationPhysicScene2DUpdater simulationUpdater = FindObjectOfType<SimulationPhysicScene2DUpdater>();
        if (simulationUpdater == null)
            CreateSimulationScene2D();

        GameObject activeObject = Selection.activeGameObject;

        Rigidbody rb = activeObject.GetComponent<Rigidbody>();
        if (rb != null)
            DestroyImmediate(rb);

        Collider col = activeObject.GetComponent<Collider>();
        if (col != null)
            DestroyImmediate(col);

        if (activeObject.GetComponent<Rigidbody2D>() == null)
            activeObject.AddComponent<Rigidbody2D>();

        if (activeObject.GetComponent<Collider2D>() == null)
            activeObject.AddComponent<CircleCollider2D>();

        if (activeObject.GetComponent<TrajectoryProjection2DComponent>() == null)
            activeObject.AddComponent<TrajectoryProjection2DComponent>();

        PhysicsScene2DCloneHandler cloneHandler = activeObject.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler == null)
            cloneHandler = activeObject.AddComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.includeSimulationPhysics = false;
        cloneHandler.syncTransform = true;

        PhysicsScene2DTeleportComponent teleportComponent = activeObject.GetComponent<PhysicsScene2DTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DTeleportComponent>() != null)
            DestroyImmediate(teleportComponent);

        PhysicsScene2DSimpleTeleportComponent simpleTeleport = activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>() != null)
            DestroyImmediate(simpleTeleport);
    }

    [MenuItem("GameObject/Physics Scene Simulator/Projection Objects/Convert to Scene Teleportable Object 2D", false, 0)]
    public static void ConvertToSceneTeleportableObject2D()
    {
        if (Selection.activeObject == null)
            return;

        SimulationPhysicScene2DUpdater simulationUpdater = FindObjectOfType<SimulationPhysicScene2DUpdater>();
        if (simulationUpdater == null)
            CreateSimulationScene2D();

        GameObject activeObject = Selection.activeGameObject;

        Rigidbody rb = activeObject.GetComponent<Rigidbody>();
        if (rb != null)
            DestroyImmediate(rb);

        Collider col = activeObject.GetComponent<Collider>();
        if (col != null)
            DestroyImmediate(col);

        if (activeObject.GetComponent<Rigidbody2D>() == null)
            activeObject.AddComponent<Rigidbody2D>();

        if (activeObject.GetComponent<Collider2D>() == null)
            activeObject.AddComponent<CircleCollider2D>();

        if (activeObject.GetComponent<TrajectoryProjection2DComponent>() == null)
            activeObject.AddComponent<TrajectoryProjection2DComponent>();

        PhysicsScene2DCloneHandler cloneHandler = activeObject.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler == null)
            cloneHandler = activeObject.AddComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.includeSimulationPhysics = false;
        cloneHandler.syncTransform = true;

        PhysicsScene2DTeleportComponent teleportComponent = activeObject.GetComponent<PhysicsScene2DTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DTeleportComponent>() == null)
            activeObject.AddComponent<PhysicsScene2DTeleportComponent>();

        PhysicsScene2DSimpleTeleportComponent simpleTeleport = activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>() != null)
            DestroyImmediate(simpleTeleport);
    }

    [MenuItem("GameObject/Physics Scene Simulator/Projection Objects/Convert to Custom Time Object 2D", false, 0)]
    public static void ConvertToCustomTimeObject2D()
    {
        if (Selection.activeObject == null)
            return;

        SimulationPhysicScene2DUpdater simulationUpdater = FindObjectOfType<SimulationPhysicScene2DUpdater>();
        if (simulationUpdater == null)
            CreateSimulationScene2D();

        GameObject activeObject = Selection.activeGameObject;

        Rigidbody rb = activeObject.GetComponent<Rigidbody>();
        if (rb != null)
            DestroyImmediate(rb);

        Collider col = activeObject.GetComponent<Collider>();
        if (col != null)
            DestroyImmediate(col);

        if (activeObject.GetComponent<Rigidbody2D>() == null)
            activeObject.AddComponent<Rigidbody2D>();

        if (activeObject.GetComponent<Collider2D>() == null)
            activeObject.AddComponent<CircleCollider2D>();

        if (activeObject.GetComponent<TrajectoryProjection2DComponent>() == null)
            activeObject.AddComponent<TrajectoryProjection2DComponent>();

        PhysicsScene2DCloneHandler cloneHandler = activeObject.GetComponent<PhysicsScene2DCloneHandler>();
        if (cloneHandler == null)
            cloneHandler = activeObject.AddComponent<PhysicsScene2DCloneHandler>();
        cloneHandler.includeSimulationPhysics = false;
        cloneHandler.syncTransform = true;

        PhysicsScene2DTeleportComponent teleportComponent = activeObject.GetComponent<PhysicsScene2DTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DTeleportComponent>() != null)
            DestroyImmediate(teleportComponent);

        PhysicsScene2DSimpleTeleportComponent simpleTeleport = activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>();
        if (activeObject.GetComponent<PhysicsScene2DSimpleTeleportComponent>() == null)
            activeObject.AddComponent<PhysicsScene2DSimpleTeleportComponent>();
    }
}
