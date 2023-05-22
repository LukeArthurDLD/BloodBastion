using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TowerTargeting))]
public abstract class Weapon : MonoBehaviour
{
    [System.NonSerialized]
    public Tower tower;
    [System.NonSerialized]
    public float nextFire = 0f;
    [System.NonSerialized]
    public Tower.DamageType damageType;
    public enum WeaponType { Raycast, Projectile, Laser };
    [System.NonSerialized]
    public WeaponType type;

    [Header("Sounds")]
    public AudioClip shootingSound;
    public Animator animator;
    //raycast
    [System.NonSerialized]
    public Ray ray;
    [System.NonSerialized]
    public RaycastHit hit;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        animator = GetComponentInChildren<Animator>();
    }
    public void OnFire(Transform raycastOrigin, Transform weaponOrigin)
    {
        if (Time.time >= nextFire)
        {
            tower.GetComponent<TowerTargeting>().LookAtTarget();

            Debug.Log("Trigger Pulled");
            Shoot(raycastOrigin, weaponOrigin);

            // effects and sounds
            if (shootingSound != null)
            {
                tower.audioSource.clip = shootingSound;
                tower.audioSource.Play();
            }
            if(animator != null)
                animator.SetTrigger("Shoot");

            nextFire = Time.time + 1f / tower.attackSpeed; //manage firerate
        }       
    }
    public abstract void Shoot(Transform raycastOrigin, Transform weaponOrigin);
    
}
public class WeaponRaycast : Weapon
{
    [Header("Effects")]
    public LineRenderer tracer;
    public ParticleSystem muzzleFlash;
    private void Start()
    {
        type = WeaponType.Raycast;
    }

    public override void Shoot(Transform raycastOrigin, Transform weaponOrigin)
    {
        //bullet tracers for debug
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;
        if (Physics.Raycast(ray, out hit))
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1.0f);

        // play effects
        if (muzzleFlash)
        {
            muzzleFlash.transform.position = raycastOrigin.position;
            muzzleFlash.Play();
        }

        //if raycast hits
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, tower.range * 2, LayerMask.GetMask("Enemy")))
        {           
            Debug.Log("Target hit, target: " + hit.collider.name);
            Health target = hit.transform.GetComponent<Health>();
             if (target != null)
             {
                 target.TakeDamage(tower.damage, damageType);
             }
            if (tracer)
                SpawnTrail(weaponOrigin, hit.point);
        }
        else if (tracer)
            SpawnTrail(weaponOrigin, ray.GetPoint(tower.range));
    }

    public void SpawnTrail(Transform origin, Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(tracer.gameObject, origin.position, Quaternion.identity);
        LineRenderer line = bulletTrailEffect.GetComponent<LineRenderer>();

        line.SetPosition(0, origin.position);
        line.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 1f);
    }


}
