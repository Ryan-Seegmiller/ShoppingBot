using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public bool GenerateRandomValues = true;

    [SerializeField]
    protected bool hasFoundPlayer = false;
    //alerted
    [SerializeField]
    protected bool hasDetectedPlayer = false;
    [SerializeField]
    [Header("Rays")]
    protected Transform rayPointArmRight;
    [SerializeField]
    protected Transform rayPointArmLeft;
    [SerializeField]
    protected Transform rayPointArmStraight;
    public GameObject meshBody;
    protected Rigidbody rb;
    //Ai controls
    protected float time = 0;
    protected float firstDetectedTime = 0;
    protected bool isFlying;
    protected float pointAtPlayerOffset;
    protected float detectionRadius = 15;
    protected float timeDetectionToFind = 5;
    protected float pointAtPlayerChance =30;
    protected float targetRotationY = 0;
    protected float yRotationReturn = 0.2f;
    protected float yRotationPerArmDetection = 0.5f;
    protected float lrArmRange = 1;
    protected float lrRayAngle = 35;
    protected float sArmRange = 1;
    protected float wanderRotationLimits =0.2f;
    protected float wanderForceLimits =5;
    protected float acceleration =15;
    protected Vector3 pointAtPlayerOffsetVector;
    protected Vector2 reverseModifier = new Vector2(2, 3);
    protected Vector2 stuckRotation = new Vector2(2, 3);
    protected bool lowChanceFlip = false;
    protected int lowChanceFlip2 = 1;
    protected float PointAtPlayerOffset{
        set
        {
            pointAtPlayerOffset = value;
            pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        }
        get
        {
            return pointAtPlayerOffset;
        }
    }

    //Trigger sphere collider on all enemies for detection of player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerControllerTest.instance.gameObject && !hasFoundPlayer && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            firstDetectedTime = time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerControllerTest.instance.gameObject)
        {
            firstDetectedTime = 0;
            hasFoundPlayer = false;
            hasDetectedPlayer = false;
        }
    }
    public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
    public void GetRandomAIValues()
    {
        lrArmRange = Random.Range(0.25f, 3f);
        sArmRange = Random.Range(0.25f, 3f);
        lrRayAngle = Random.Range(1, 75f);

        detectionRadius = Random.Range(5, 10f);
        wanderRotationLimits = Random.Range(0.001f, 0.01f);
        wanderForceLimits = Random.Range(0.001f, 2f);

        yRotationPerArmDetection = Random.Range(0.001f, 0.1f);
        yRotationReturn = yRotationPerArmDetection+Random.Range(0.001f, 0.01f);

        pointAtPlayerChance = Random.Range(1, 90f);
        timeDetectionToFind = Random.Range(1, 5f);
        pointAtPlayerOffset= Random.Range(-5, 5f);
        acceleration = Random.Range(0.01f, 0.15f);

        reverseModifier = new Vector2(Random.Range(0.01f, 0.02f), Random.Range(0.02f, 0.025f));
        stuckRotation = new Vector2(Random.Range(0.01f, 0.2f), Random.Range(0.2f, 1f));
    }
    public void Die()
    {
        Debug.Log(gameObject.name + " enemy has died");
        //iterate through body parts and make parent null and rotate for death effect
        for(int i = 0; i < meshBody.transform.childCount;i++)
        {
            Transform t = meshBody.transform.GetChild(i);
            t.transform.parent = null;
            Rigidbody tRb= t.AddComponent<Rigidbody>();
            t.AddComponent<SphereCollider>().radius=0.1f;
            tRb.AddTorque(transform.up * Random.Range(-360, 360));
            tRb.AddForce(transform.forward * Random.Range(-25, 25));
            Destroy(t.gameObject, 3);
        }
        EnemyManager.instance.currentEnemies.Remove(this);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Die();
    }
}
