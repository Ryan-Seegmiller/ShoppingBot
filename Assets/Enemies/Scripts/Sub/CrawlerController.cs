using audio;
using System.Collections;
using UnityEngine;
public class CrawlerController : EnemyBase
{
    public int roamMode = 0;
    float ramForce = 25;
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
        base.FixedUpdate();

        if (Random.Range(0, 100f) > 99.9f) {roamMode = Random.Range(0, 2); }

        if (hasFoundPlayer)
        {
            rb.AddForce(transform.forward * ramForce);
            if (Random.Range(0, 100f) > 90f)
            {
                transform.LookAt(player.transform.position);
                Vector3 loc = transform.localEulerAngles;
                transform.localEulerAngles = new Vector3(0, loc.y,loc.z);
            }
            if(currentDistanceToPlayer < 1.3f && !hasExploded)
            {
                StartCoroutine(explode());
            }
        }
        else
        {
            Wander();
        }
    }
    public IEnumerator explode()
    {
        hasExploded = true;
        AudioManager.instance.PlaySound3D(7, transform.position);
        yield return new WaitForSeconds(0.7f);
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
