using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectorySimulator : MonoBehaviour
{
    public float timeStepMultiplier = 1;
    public Vector3 applyForce = new Vector3(0, 20, 15);

    private Scene mainScene;
    private Scene simScene;
    private PhysicsScene simPhysicsScene;
    private PhysicsScene physicsScene;

    public GameObject colisionPlane;

    private List<GameObject> pathItems = new List<GameObject>();
    public GameObject ball;
    private Rigidbody rb;
    private string filePath = "trajectories.txt";

    private void Start()
    {
        Physics.autoSimulation = false;
        mainScene = SceneManager.CreateScene("MainScene");
        physicsScene = mainScene.GetPhysicsScene();

        CreateSceneParameters sceneParams = new CreateSceneParameters(LocalPhysicsMode.Physics3D);

        simScene = SceneManager.CreateScene("SimScene", sceneParams);
        simPhysicsScene = simScene.GetPhysicsScene();

        rb = ball.GetComponent<Rigidbody>();
        rb.useGravity = false;

        filePath = $"{Application.persistentDataPath}/{filePath}";
    }


    private void FixedUpdate()
    {
        if (!physicsScene.IsValid())
            return;

        physicsScene.Simulate(Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))// || Input.GetMouseButtonDown(0))
            StartCoroutine(ShootBall());

        if (Input.GetKeyDown(KeyCode.Q))
            ClearPath();
    }

    private IEnumerator ShootBall()
    {
        if (!physicsScene.IsValid() || !simPhysicsScene.IsValid())
            yield return null;

        GameObject predictionBall = Instantiate(ball);
        Rigidbody predictionBallRb = predictionBall.GetComponent<Rigidbody>();
        SceneManager.MoveGameObjectToScene(predictionBall, simScene);
        predictionBallRb.useGravity = true;
        predictionBallRb.AddForce(applyForce, ForceMode.Impulse);

        HitNotifier hitNotifier = predictionBall.AddComponent<HitNotifier>();

        Material redMaterial = new Material(Shader.Find("Diffuse"));
        redMaterial.color = Color.red;

        SceneManager.MoveGameObjectToScene(colisionPlane, simScene);


        while(!hitNotifier.hit)
        {
            simPhysicsScene.Simulate(Time.fixedDeltaTime * timeStepMultiplier);

            GameObject pathMarkSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pathMarkSphere.GetComponent<Collider>().isTrigger = true;
            pathMarkSphere.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            pathMarkSphere.transform.position = predictionBall.transform.position;
            pathMarkSphere.GetComponent<MeshRenderer>().material = redMaterial;
            SceneManager.MoveGameObjectToScene(pathMarkSphere, simScene);

            pathItems.Add(pathMarkSphere);

            var file = File.Open(filePath, FileMode.Append);
            string location = $"{pathMarkSphere.transform.position.ToString()}\n";
            file.Write(Encoding.Default.GetBytes(location), 0, location.Length);
            file.Close();
            yield return null;
        }

        Destroy(predictionBall);
        SceneManager.MoveGameObjectToScene(colisionPlane, mainScene);

        rb.useGravity = true;
        SceneManager.MoveGameObjectToScene(ball, mainScene);
        rb.AddForce(applyForce, ForceMode.Impulse);
        yield return null;
    }

    private void ClearPath()
    {
        for (int i = 0; i < pathItems.Count; i++)
        {
            Destroy(pathItems[i]);
        }
        pathItems.Clear();

        ball.transform.position = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.useGravity = false;

        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
