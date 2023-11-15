using UnityEngine;
using enemymanager;
using TMPro;
using audio;
public class EnemyBase : MonoBehaviour
{
    #region rays
    protected bool[] rayBools;
    protected Vector3 rp;
    protected Vector3 lp;
    protected Vector3 sp;
    #endregion
    #region AI base controls
    public bool GenerateRandomValues = true;
    protected bool hasFoundPlayer = false;//after multiple seconds of detection
    #endregion
    #region AI modifiable stats
    //This is where you can make the AI stronger or weaker
    protected float detectionRadius = 15;
    protected float targetRotationY = 0;
    protected float yRotationReturn = 0.2f;
    protected float yRotationPerArmDetection = 0.5f;
    protected float lrArmRange = 1;
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
    float lastDetectionCheckTime = 0;
    protected int wanderFlip1 = 1;
    protected int wanderFlip2 = 1;
    protected int wanderFlip1Chance = 30;
    #endregion
    #region health
    protected TMP_Text healthBar;
    protected float _health=-1;
    public float health 
    { get { return _health; }
        set { if (_health != -1) { SetHealthbar(true, value);/*dont update health bar on first go, so sound doesnt play on initial health set*/ } _health = value; if (_health <= 0) { Die(true); } }
    }
    #endregion
    #region components
    protected GameObject player;
    protected Rigidbody rb;
    protected Animator anim;
    #endregion

    [SerializeField]
    [Header("Debug")]
    protected float currentDistanceToPlayer;



    void Awake()
    {
        if(!(this is CrawlerController))
            anim = GetComponentInChildren<Animator>();

        if (GenerateRandomValues)
            GetRandomAIValues();
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<TMP_Text>();
        player = EnemyManager.instance.player;
        if (!Physics.Raycast(transform.position + transform.up * 1, Vector3.up * 50))//quick fix for enemies spawning on roof;
        {
            Die(false);
            EnemyManager.instance.SpawnEnemies(1, Random.Range(0, 3));
            Debug.Log($"{this.GetType()} :: Enemy killed by system - failed roof check. Enemy has been replaced.");
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
    }
    protected void FixedUpdate()
    {
        DoFlips();
        DropAndClampTargetYRot();

        //healthBar.transform.LookAt(Camera.main.transform);
        if (time > lastDetectionCheckTime + 0.5f)//slow down the amount of times this is called
        {
            lastDetectionCheckTime = time;

            //this code replaces the trigger colliders for detecting the player
            currentDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            hasFoundPlayer = currentDistanceToPlayer <= detectionRadius;
        }
        rayBools = DoRays();

        if (rayBools[2])
            targetRotationY += yRotationPerArmDetection;
        if (rayBools[0])
            targetRotationY -= yRotationPerArmDetection;
        if (rayBools[1])
        {
            //backup if stuck
            rb.AddForce(-Vector3.forward * acceleration * Random.Range(reverseModifierMinMax.x, reverseModifierMinMax.y));
            targetRotationY += Random.Range(stuckRotationMinMax.x, stuckRotationMinMax.y) * Random.Range(-1, 2);
        }

        transform.Rotate(0, targetRotationY, 0);
    }
    public void SetHealthbar(bool playAudio, float healthValue)
    {
        string healthText="";
        Color healthColor;
        for (int i = 0; i < healthValue; i++)
            healthText += "-";

        if (healthValue >= 3)
            healthColor = Color.green;
        else if (healthValue >= 2)
            healthColor = Color.yellow;
        else
            healthColor = Color.red;
        if (healthBar.text != healthText && playAudio)//use this to play sound only when its taken 1 full damage
        {
            AudioManager.instance.PlaySound3D(8, transform.position);
        }
        healthBar.color = healthColor;
        healthBar.text = healthText;
    }
    public void GetRandomAIValues()
    {
        lrArmRange = Random.Range(0.25f, 3f);
        sArmRange = Random.Range(0.25f, 3f);

        detectionRadius = Random.Range(10f, 25f);
        wanderRotationLimits = Random.Range(0.01f, 0.01f);
        wanderForceLimits = Random.Range(0.01f, 2f);

        yRotationPerArmDetection = Random.Range(0.001f, 0.1f);
        yRotationReturn = yRotationPerArmDetection+Random.Range(0.001f, 0.01f);

        acceleration = Random.Range(5f,15f);

        reverseModifierMinMax = new Vector2(Random.Range(0.3f, 0.5f), Random.Range(0.5f, 0.9f));
        stuckRotationMinMax = new Vector2(Random.Range(0.1f, 0.5f), Random.Range(0.5f, 1f));
    }
    public void Die(bool playAudio)
    {
        Debug.Log($"{this.GetType()} :: Enemy has died at {transform.position}", this);

        EnemyManager.instance.currentEnemies.Remove(this);
        if (playAudio)
        {
            AudioManager.instance.PlaySound3D(4, transform.position);
        }
        Destroy(this.gameObject); 
    }
    public void Hit(float mod)//used for hit by player
    {
        health-=3f*mod;//base damage from player is determined here
    }
    public void Hit()
    {
        health -= 1f;
    }
    private void DoFlips()
    {
        wanderFlip1Chance = 99;//this is so it has a higher chance of flipping if its -1, because it controls forward or backwards on wander
        if (wanderFlip1 == -1)
        {
            wanderFlip1Chance = 30;
        }
        if (Random.Range(0f, 100f) > wanderFlip1Chance) { wanderFlip1 *= -1; }
        if (Random.Range(0f, 100f) > 97f) { wanderFlip2 *= -1; }
    }
    public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }

    protected bool[] DoRays()
    {
        rp = transform.position + transform.forward + transform.right;
        lp = transform.position + transform.forward - transform.right;
        sp = transform.position + transform.forward;
        Ray rr = new Ray(rp, transform.forward+(transform.right*0.5f)); // right
        Ray rl = new Ray(lp, transform.forward - (transform.right * 0.5f)); // left
        Ray rs = new Ray(sp, transform.forward); // straight
        bool r = Physics.Raycast(rr, lrArmRange);
        bool l = Physics.Raycast(rl, lrArmRange);
        bool s = Physics.Raycast(rs, sArmRange);
        return new bool[3] { l, s, r };
    }

    protected void DropAndClampTargetYRot()
    {
        if (targetRotationY > 0)
            targetRotationY -= yRotationReturn;
        if (targetRotationY < 0)
            targetRotationY += yRotationReturn;
        if (targetRotationY >= 360)
            targetRotationY -= 360;
        if (targetRotationY <= -360)
            targetRotationY += 360;
    }

    public void Wander()
    {
        rb.AddForce(transform.forward * acceleration * Random.Range(0, wanderForceLimits) * wanderFlip1);
        targetRotationY += Random.Range(0, wanderRotationLimits) * wanderFlip2;
    }
}
