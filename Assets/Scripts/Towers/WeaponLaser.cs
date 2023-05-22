using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Weapon
{
    [Header("Effects")]
    public LineRenderer laserBeam;
    public ParticleSystem impactEffect;
    Transform targetTransform;
  
    private void Start()
    {
        type = WeaponType.Laser;
    }
    public override void Shoot(Transform raycastOrigin, Transform weaponOrigin)
    {
        ToggleBeam(true);

        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, tower.range * 2, LayerMask.GetMask("Enemy")))
        {
            Debug.Log("Target hit, target: " + hit.collider.name);
            Health target = hit.transform.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(tower.damage, damageType);
                targetTransform = target.transform;
            }
            else
                targetTransform = null;
            if (targetTransform)
            {
                laserBeam.SetPosition(0, weaponOrigin.position);
                laserBeam.SetPosition(1, targetTransform.position);

                Vector3 dir = weaponOrigin.position - targetTransform.position;
                impactEffect.transform.rotation = Quaternion.LookRotation(dir);
                impactEffect.transform.position = targetTransform.position;
            }
            
        }
    }   
    public void ToggleBeam(bool isActive)
    {
        if (laserBeam.enabled = isActive)
            return;
        laserBeam.enabled = isActive;

        if(isActive && impactEffect)
            impactEffect.Play();
        if (!isActive && impactEffect)
            impactEffect.Stop();
    }
}

