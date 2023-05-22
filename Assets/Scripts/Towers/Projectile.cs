using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public GameObject explosionEffect;

    //explosion
    [System.NonSerialized]
    public int explosionDamage;
    [System.NonSerialized]
    public Tower.DamageType damageType;
    [System.NonSerialized]
    public float explosionRange;
    [System.NonSerialized]

    private bool hasExploded = false;

    //lifetime
    [System.NonSerialized]
    public float maxLifetime;

    void FixedUpdate()
    {
        if (!hasExploded)
        {
            maxLifetime -= Time.deltaTime;
            if (maxLifetime <= 0)
                Explode();
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            collision.transform.GetComponent<Health>().TakeDamage(explosionDamage / 2, damageType);
            Explode();
        }
    }
    void Explode()
    {
        hasExploded = true;

        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRange);        
        foreach (Collider nearbyObject in targets)
        {            
            if (nearbyObject.GetComponent<Health>())
            {
                int damage = CalculateExplosionDamage(nearbyObject.transform.position, transform.position);
                nearbyObject.GetComponent<Health>().TakeDamage(damage, damageType);
            }
        }

        if (explosionEffect != null)
        {
            GameObject explosionGO = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionGO, 2f);
        }
        
        Destroy(gameObject);
    }
    private int CalculateExplosionDamage(Vector3 targetPosition, Vector3 explosionPosition)
    {
        Vector3 explosionToTarget = targetPosition - explosionPosition;

        //find distance
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (explosionRange - explosionDistance) / explosionRange;

        float damage = relativeDistance * explosionDamage;
        damage = Mathf.Max(explosionDamage / 3, damage);

        return Mathf.RoundToInt(damage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }     

}
