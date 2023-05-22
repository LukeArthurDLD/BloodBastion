using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : Weapon
{
    public GameObject projectile;

    // explosion
    [Header("Explosion Properties")]
    public float explosionRange;

    // forces
    [Header("Launch Properties")]
    public float forwardForce;
    public bool hasGravity = false;
    public float upwardForce;
        
    // lifetime
    [Header("Lifetime and Collisions")]
    public float maxLifetime;
    private void Start()
    {
        type = WeaponType.Projectile;
    }
    void AddProjectileProperties(GameObject projectile)
    {
        Projectile p = projectile.GetComponent<Projectile>();

        p.explosionDamage = tower.damage;
        p.explosionRange = explosionRange;
        p.damageType = damageType;

        projectile.GetComponent<Rigidbody>().useGravity = hasGravity;
        p.maxLifetime = maxLifetime;
    }
    
    public override void Shoot(Transform raycastOrigin, Transform weaponOrigin)
    {
        if (animator != null)
            animator.SetTrigger("Shoot");

        //find hit position     
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;         

        //checks if ray hits
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        //instatiates projectile
        GameObject currentProjectile = Instantiate(projectile, weaponOrigin.position, Quaternion.identity);
        currentProjectile.transform.forward = raycastOrigin.forward.normalized;

        AddProjectileProperties(currentProjectile);

        //adds force to projectile
        currentProjectile.GetComponent<Rigidbody>().AddForce(raycastOrigin.forward.normalized * forwardForce, ForceMode.Impulse);
        currentProjectile.GetComponent<Rigidbody>().AddForce(weaponOrigin.transform.up * upwardForce, ForceMode.Impulse);
                 
    }
   
}
