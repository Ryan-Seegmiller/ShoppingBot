using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using TMPro;
using UnityEditor;
using audio;
public class EnemyBase : MonoBehaviour
{
    #region rays
    protected bool[] rayBools;
    protected Transform rayPointArmRight;
    protected Transform rayPointArmLeft;
    protected Transform rayPointArmStraight;
    #endregion
    #region AI base controls
    public bool GenerateRandomValues = true;
    protected bool hasFoundPlayer = false;//after multiple seconds of detection
    protected bool hasDetectedPlayer = false;//player is close enough
    #endregion
    #region AI modifiable stats
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
    protected Vector2 reverseModifierMinMax = new Vector2(2, 3);
    protected Vector2 stuckRotationMinMax = new Vector2(2, 3);
    #endregion
    #region timers, flips
    protected float time = 0;
    protected float firstDetectedTime = 0;
    protected bool lowChanceFlip = false;
    protected int lowChanceFlip2 = 1;
    float lastDamagingBumpTime = 0;
    float lastDetectionCheckTime = 0;
    #endregion
    #region health
    TMP_Text healthBar;
    protected float _health;
    public float health 
    { get { return _health; }
        set { _health = value; SetHealthbar(); if (_health <= 0 && time>1) { Die(); } if (startHealth == 0) { startHealth = _health; } }
    }
    public float startHealth { get { return _startHealth; } set { _startHealth = value; SetHealthbar(); } }
    float _startHealth = 0;
    #endregion
    #region components
    protected GameObject player;
    public GameObject meshBody;
    protected Rigidbody rb;
    float currentDistanceToPlayer;
    #endregion

    void Awake()
    {
        if (GenerateRandomValues)
            GetRandomAIValues();
        rb = GetComponent<Rigidbody>();
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
        healthBar = GetComponentInChildren<TMP_Text>();
        player = EnemyManager.instance.player;
    }
    private void Update()
    {
        time += Time.deltaTime;
    }
    protected void FixedUpdate()
    {
        DoFlips();
        //healthBar.transform.LookAt(Camera.main.transform);
        if (time > lastDetectionCheckTime + 0.5f)//slow down the amount of times this is called
        {
            if (transform.position.y < -10)//may as well be in here for less frequent checking
            {
                Die();
            }
            //this code replaces the trigger colliders for detecting the player
            lastDetectionCheckTime = time;
            currentDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (currentDistanceToPlayer <= detectionRadius && !hasFoundPlayer && !hasDetectedPlayer)
            {
                bool isStalker = this is StalkerController;
                if (isStalker && !Physics.Linecast(transform.position,player.transform.position))//make sure the flying drone has a clear shot before attacking
                {
                    hasDetectedPlayer = true;
                    firstDetectedTime = time;
                }
                else if(!isStalker)
                {
                    hasDetectedPlayer = true;
                    firstDetectedTime = time;
                }
            }
            else if (currentDistanceToPlayer > detectionRadius)
            {
                firstDetectedTime = 0;
                hasFoundPlayer = false;
                hasDetectedPlayer = false;
            }
            if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)//if its seen the player for long enough, it has 'found' it
            {
                hasFoundPlayer = true;
            }
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
        acceleration = Random.Range(5f,25f);

        reverseModifierMinMax = new Vector2(Random.Range(0.3f, 0.5f), Random.Range(0.5f, 0.9f));
        stuckRotationMinMax = new Vector2(Random.Range(0.1f, 0.5f), Random.Range(0.5f, 1f));
    }
    public void Die()
    {
        Debug.Log($"{this.GetType()} :: Enemy has died at {transform.position}", this);
        AudioManager.instance.PlaySound3D(4, transform.position);
        //iterate through body parts and make parent null and rotate for death effect
        for(int i = 0; i < meshBody.transform.childCount;i++)
        {
            PopBodyPart(meshBody.transform.GetChild(i).transform);
        }
        EnemyManager.instance.currentEnemies.Remove(this);
        if(this is CrawlerController)//this way the crawler will always explode
        {
            CrawlerController c = (CrawlerController)this;//didnt want to work unless cached
            c.explode();
        }
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
        if (rb.velocity.magnitude > 8 && time > lastDamagingBumpTime + 2f)
        {
            health-= rb.velocity.magnitude/10;
            lastDamagingBumpTime = time;
        }
    }
    public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    protected bool[] DoRays()
    {
        Ray rr = new Ray(rayPointArmRight.transform.position, rayPointArmRight.transform.forward); // right
        Ray rl = new Ray(rayPointArmLeft.transform.position, rayPointArmLeft.transform.forward); // left
        Ray rs = new Ray(rayPointArmStraight.transform.position, rayPointArmStraight.transform.forward); // straight
        bool r = Physics.Raycast(rr, lrArmRange);
        bool l = Physics.Raycast(rl, lrArmRange);
        bool s = Physics.Raycast(rs, sArmRange);
        return new bool[3] { l, s, r };
    }
}
