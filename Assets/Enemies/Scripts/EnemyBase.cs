using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using TMPro;

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
    protected float timeDetectionToFind = 2;
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
    float sh =0;
    public float startHealth { get { return sh; } set { sh = value; SetHealthbar(); } }
    public List<AudioClip> deathAudio = new List<AudioClip>();
    public List<AudioClip> detectedAudio = new List<AudioClip>();
    public List<AudioClip> attackAudio = new List<AudioClip>();
    TMP_Text healthBar;
    protected float _health;
    public float health 
    { get { return _health; }
        set { _health = value; SetHealthbar(); if (_health <= 0 && time>1) { Die(); } if (startHealth == 0) { startHealth = _health; } }
    }
    float lastDamagingBumpTime = 0;
    public AudioSource aS;
    protected GameObject player;
    protected float PointAtPlayerOffset
    {
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

    void Awake()
    {
        //Debug.Log("Enemy Created", this);
        if (GenerateRandomValues)
            GetRandomAIValues();
        GetComponent<SphereCollider>().radius = detectionRadius;
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb = GetComponent<Rigidbody>();
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
        aS = GetComponentInChildren<AudioSource>();
        healthBar = GetComponentInChildren<TMP_Text>();
        player = EnemyManager.instance.player;
    }
    private void Update()
    {
        time += Time.deltaTime;
        healthBar.transform.LookAt(Camera.main.transform);
    }
    protected void FixedUpdate()
    {
        DoFlips();

        if (transform.position.y < -10)
        {
            Die();
        }
        if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
    }
    public void SetHealthbar()
    {
        string s="";
        Color c;
        for (int i = 0; i < health; i++)
            s += "-";
        if (health >= 3)
            c = Color.green;
        else if (health >= 2)
            c = Color.yellow;
        else
            c = Color.red;
        healthBar.color = c;
        healthBar.text = s;
    }
    public void GetRandomAIValues()
    {
        lrArmRange = Random.Range(0.25f, 3f);
        sArmRange = Random.Range(0.25f, 3f);
        lrRayAngle = Random.Range(1, 75f);

        detectionRadius = Random.Range(10f, 25f);
        wanderRotationLimits = Random.Range(0.01f, 0.01f);
        wanderForceLimits = Random.Range(0.01f, 2f);

        yRotationPerArmDetection = Random.Range(0.001f, 0.1f);
        yRotationReturn = yRotationPerArmDetection+Random.Range(0.001f, 0.01f);

        pointAtPlayerChance = Random.Range(1, 90f);
        timeDetectionToFind = Random.Range(0.5f, 2f);
        pointAtPlayerOffset= Random.Range(-5, 5f);
        acceleration = Random.Range(5f,25f);

        reverseModifier = new Vector2(Random.Range(0.3f, 0.5f), Random.Range(0.5f, 0.9f));
        stuckRotation = new Vector2(Random.Range(0.1f, 0.5f), Random.Range(0.5f, 1f));
    }
    public void Die()
    {
        aS.transform.parent = null;
        aS.PlayOneShot(deathAudio[Random.Range(0, deathAudio.Count)]);
        Destroy(aS.gameObject, 3);
        Debug.Log($"{this.GetType()} :: Enemy has died at {transform.position}", this);
        //iterate through body parts and make parent null and rotate for death effect
        for(int i = 0; i < meshBody.transform.childCount;i++)
        {
            PopBodyPart(meshBody.transform.GetChild(i).transform);
        }
        EnemyManager.instance.currentEnemies.Remove(this);
        Destroy(this.gameObject);
    }
    private void PopBodyPart(Transform t)
    {
        t.transform.parent = null;
        Rigidbody tRb = t.gameObject.AddComponent<Rigidbody>();
        t.gameObject.AddComponent<SphereCollider>().radius = 0.25f;
        tRb.AddTorque(transform.up * Random.Range(-360, 360));
        tRb.AddForce(transform.forward * Random.Range(-25, 250));
        Destroy(t.gameObject, 3);
    }
    public void Hit(float mod)
    {
        if(meshBody.transform.childCount>0)
            PopBodyPart(meshBody.transform.GetChild(Random.Range(0,meshBody.transform.childCount)).transform);
        health-=3f*mod;//base damage from player is determined here
        if (health <= 0)
            Die();
    }
    public void Hit()
    {
        if (meshBody.transform.childCount > 0)
            PopBodyPart(meshBody.transform.GetChild(Random.Range(0, meshBody.transform.childCount)).transform);
        health -= 1f;
        if (health <= 0)
            Die();
    }
    private void DoFlips()
    {
        if (Random.Range(0, 100f) > 99.9f) { lowChanceFlip = !lowChanceFlip;}
        if (Random.Range(0, 100f) > 90f) { lowChanceFlip2 *= -1; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 3 && time > lastDamagingBumpTime + 0.5f)
        {
            health--;
            lastDamagingBumpTime = time;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !hasFoundPlayer && !hasDetectedPlayer)
        {
            hasDetectedPlayer = true;
            firstDetectedTime = time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
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
}
