using System.Collections;

using UnityEngine;
public class CrawlerController : EnemyBase
{
    public int roamMode = 0;
    public float ramForce = 15;
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        if (GenerateRandomValues)
            GetRandomAIValues();
        transform.position = new Vector3(transform.position.x, EnemyManager.instance.groundEnemyHeight, transform.position.z);
        GetComponent<SphereCollider>().radius = detectionRadius;
        pointAtPlayerOffsetVector = new Vector3(pointAtPlayerOffset, 0, pointAtPlayerOffset);
        rb = GetComponent<Rigidbody>();
        rayPointArmLeft.localEulerAngles = new Vector3(0, -lrRayAngle, 0);
        rayPointArmRight.localEulerAngles = new Vector3(0, lrRayAngle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //found player timer
        if (hasDetectedPlayer && !hasFoundPlayer && time > firstDetectedTime + timeDetectionToFind)
        {
            hasFoundPlayer = true;
        }
    }
    private void FixedUpdate()
    {

        DoFlips();

        if (hasFoundPlayer)
        {
            rb.AddForce(transform.forward * ramForce);
            if (Random.Range(0, 100f) > 90f)
                transform.LookAt(PlayerControllerTest.instance.transform.position);
        }
        else
        {
            DoRoamAI();
        }
        RaycastHit h;
        if (hasFoundPlayer && Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out h, sArmRange+0.5f))
        {
            if (h.transform == PlayerControllerTest.instance.transform)
            {
                explode();
                Die();
            }
        }
        DropAndClampTargetYRot();
        if (transform.position.y < -100)
        {
            Die();
        }
        //set final rotation
        transform.Rotate(0, targetRotationY, 0);
    }
    void DropAndClampTargetYRot()
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
    void DoFlips()
    {
        if (Random.Range(0, 100f) > 99.9f) { lowChanceFlip = !lowChanceFlip; roamMode = Random.Range(0, 2); }
        if (Random.Range(0, 100f) > 90f) { lowChanceFlip2 *= -1; }
    }
    void DoRoamAI()
    {
        //SENSORS
        //arms/sensor rays
        Ray rr = new Ray(rayPointArmRight.transform.position, rayPointArmRight.transform.forward); // right
        Ray rl = new Ray(rayPointArmLeft.transform.position, rayPointArmLeft.transform.forward); // left
        Ray rs = new Ray(rayPointArmStraight.transform.position, rayPointArmStraight.transform.forward); // straight
        bool r = Physics.Raycast(rr, lrArmRange);
        bool l = Physics.Raycast(rl, lrArmRange);
        bool s = Physics.Raycast(rs, sArmRange);

        Debug.DrawRay(rr.origin, rr.direction * lrArmRange, Color.blue);
        Debug.DrawRay(rl.origin, rl.direction * lrArmRange, Color.red);
        Debug.DrawRay(rs.origin, rs.direction * sArmRange, Color.green);
        //rotate if arms have collided
        if (r)
            targetRotationY += yRotationPerArmDetection;
        if (l)
            targetRotationY -= yRotationPerArmDetection;

        //move forward / back up and rotate, depending on sensors
        else if (roamMode==0)
        {
            //use sensors in this mode mode
            if (!s)
            {
                transform.Translate(Vector3.forward * acceleration);
            }
            else
            {
                targetRotationY += Random.Range(-wanderRotationLimits, wanderRotationLimits);
            }
        }
        else if (roamMode == 1)
        {//random move mode
            if(Random.Range(0,100f)>98f)
                targetRotationY = Random.Range(-3, 3);
        }

        if (s)
        {
            //backup if stuck
            transform.Translate(-Vector3.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            targetRotationY += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }

    }
    void explode()
    {
        explosion.gameObject.transform.parent = null;
        explosion.Play();
        Destroy(explosion,3);
    }
}
