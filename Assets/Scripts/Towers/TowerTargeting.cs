using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tower))]
public class TowerTargeting : MonoBehaviour
{
    Tower tower;
    [System.NonSerialized]
    public bool isTowerPlaced = false;

    public enum TargettingOptions { First, Close, Strong, Last};
    [Header("Targetting")]
    public TargettingOptions targettingOptions = TargettingOptions.First;
    public bool turn = true;
    public float turnSpeed = 10f;
    public Transform rotationPoint;

    public string enemyTag = "Enemy";
    private Transform target;

    [System.NonSerialized]
    public Weapon weapon;
    private WeaponLaser laser;
    [Header("Weapon")]
    public Transform raycastOrigin;
    public Transform weaponOrigin;
    [Header("Effects")]
    public GameObject radius;
    public GameObject taunt;
   
    void Start()
    {
        tower = GetComponent<Tower>();
        weapon = GetComponent<Weapon>();

        // for laser
        laser = GetComponent<WeaponLaser>();

        if (weaponOrigin == null)
            weaponOrigin = raycastOrigin;

        if(taunt)
            taunt.SetActive(false);
        if(isTowerPlaced)
            InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
    }
    void UpdateTarget()
    {
        List<Enemy> enemies = Enemy.all;
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;
        
        foreach(Enemy enemy in enemies)
        {
            if (enemy.isStealthed && tower.detectCamo == false)
                continue;
            // get distance between tower and enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // enemy is taunting
            if (enemy.isTaunting && distanceToEnemy <= tower.range)
            {
                distanceToEnemy /= 100;
            }
            
            // checks distance            
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy; 
                nearestEnemy = enemy;
            }
        }
        if(nearestEnemy != null && shortestDistance <= tower.range)
        {
            if(target == null || nearestEnemy.isTaunting)
                target = nearestEnemy.transform;

            if(nearestEnemy.isTaunting && taunt)
                taunt.SetActive(true);
            else if(taunt)
                taunt.SetActive(false);  
                        
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist > tower.range)
                target = null;            
        }
        else 
            target = null;
    }
    void Update()
    {
        if (target == null || !isTowerPlaced)
        {
            if (laser != null)
                laser.ToggleBeam(false);
            return;
        }
        if (weapon != null)
        {
            weapon.OnFire(raycastOrigin, weaponOrigin);
        }

    } 
    public void LookAtTarget()
    {
        // rotate to face target
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation;
        if (turn)
            rotation = Quaternion.Lerp(rotationPoint.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        else
            rotation = lookRotation.eulerAngles;
        rotationPoint.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }
    public void ShowRadius(bool isActive)
    {
        radius.SetActive(isActive);
    }
    public void PlaceTower()
    {
        ShowRadius(true);

        if (!isTowerPlaced)
        {
            isTowerPlaced = true;
        }
    }
}
