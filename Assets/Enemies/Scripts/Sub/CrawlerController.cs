using System.Collections;
using enemymanager;
using UnityEngine;
public class CrawlerController : EnemyBase
{
    public int roamMode = 0;
    public float ramForce = 15;
    public ParticleSystem explosion;
    public float explosionRadius = 5f;
    public Vector2 explosionForceRange = new Vector2(25, 100);
    public float explosionTorqueRange = 25;
    private void Start()
    {
        health = 2;
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (Random.Range(0, 100f) > 99.9f) {roamMode = Random.Range(0, 2); }

        if (hasFoundPlayer)
        {
            rb.AddForce(transform.forward * ramForce);
            if (Random.Range(0, 100f) > 90f)
                transform.LookAt(player.transform.position);
        }
        else
        {
            DoRoamAI();
        }
        RaycastHit h;
        if (hasFoundPlayer && Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out h, sArmRange+0.1f))
        {
            if (h.transform == player.transform)
            {
                explode();
                Die();
            }
        }
        DropAndClampTargetYRot();
        transform.Rotate(0, targetRotationY, 0);
    }
    private void DropAndClampTargetYRot()
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

    private void DoRoamAI()
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
            //use sensors in this mode
            if (!s)
            {
                rb.AddForce(transform.forward * acceleration);
            }
            else
            {
                targetRotationY += Random.Range(-wanderRotationLimits, wanderRotationLimits);
            }
        }
        else if (roamMode == 1)
        {//random rotate mode
            if(Random.Range(0,100f)>98f)
                targetRotationY = Random.Range(-3, 3);
        }

        if (s)
        {
            //backup if stuck
            rb.AddForce(-transform.forward * acceleration * Random.Range(reverseModifier.x, reverseModifier.y));
            targetRotationY += Random.Range(stuckRotation.x, stuckRotation.y) * Random.Range(-1, 2);
        }

    }
    private void explode()
    {
        aS.PlayOneShot(attackAudio[Random.Range(0, attackAudio.Count)]);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            Rigidbody currentRb;
            if ((hitCollider.gameObject.tag == "Item" || hitCollider.gameObject.tag == "Player") && hitCollider.gameObject.TryGetComponent<Rigidbody>(out currentRb))
            {
                //enable items hit by explosion, add force and torque
                currentRb.isKinematic = false;
                currentRb.AddExplosionForce(Random.Range(explosionForceRange.x, explosionForceRange.y), transform.position, explosionRadius);
                currentRb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(-explosionTorqueRange, explosionTorqueRange));

            }
        }
        explosion.gameObject.transform.parent = null;
        explosion.Play();
        Destroy(explosion,3);
    }
}
