using audio;
using enemymanager;
using System.Collections;
using UnityEngine;
public class CrawlerController : EnemyBase
{
    float ramForce = 20;
    public ParticleSystem explosion;
    public float explosionRadius = 5f;
    public Vector2 explosionForceRange = new Vector2(25, 100);
    public float explosionTorqueRange = 25;
    bool hasExploded = false;
    private void Start()
    {
        health = 2;
    }
    private new void FixedUpdate()
    {
        bool paused = EnemyManager.instance.PauseEnemies;
        if (!paused)
        {
            if (paused != wasPausedLastFrame)//if the player just unpaused, reset vel to cached value
            {
                UnPausePhysics();
            }
            base.FixedUpdate();

            if (hasFoundPlayer)
            {
                rb.AddForce(transform.forward * ramForce);
                if (Random.Range(0, 100f) > 95f)
                {
                    transform.LookAt(player.transform.position);
                    Vector3 loc = transform.localEulerAngles;
                    transform.localEulerAngles = new Vector3(0, loc.y, loc.z);
                }
                if (currentDistanceToPlayer < 1.3f && !hasExploded)
                {
                    StartCoroutine(explode());
                }
            }
            else
            {
                DoWander();
            }
        }
        else if (paused != wasPausedLastFrame)//if the player just paused for the first time, store its velocity, and set to 0
        {
            UnPausePhysics();
        }
        wasPausedLastFrame = paused;
    }
    public IEnumerator explode()
    {
        hasExploded = true;
        AudioManager.instance.PlaySound3D(7, transform.position);
        yield return new WaitForSeconds(0.55f);
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

        Die(true);
        Destroy(explosion,3);
    }
}
