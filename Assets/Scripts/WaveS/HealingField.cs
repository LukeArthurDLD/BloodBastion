using UnityEngine;
public class HealingField : MonoBehaviour
{
    private Health health;
    private SphereCollider trigger;
    [Header("Regeneration Stats")]
    public float range;
    [Range(0, 1)]
    public float regenRate;
    public float regenDelay = 2;
    public int regenAmount;

    private void Start()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.radius = range;

        health = GetComponent<Health>();
        StartRegen(health);
    } 
    private void OnTriggerEnter(Collider other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null && !h.regenerates)
            StartRegen(h);
    }
    private void OnTriggerExit(Collider other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null)
            StopRegen(h);
    }
    void StartRegen(Health h)
    {
        h.ToggleRegen(true, regenRate, regenDelay, regenAmount);
    }
    void StopRegen(Health h)
    {
        h.ToggleRegen(false);
    }
    public void OnDeath()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range + 2);
        foreach (Collider enemy in enemies)
        {
            if (enemy.GetComponent<Health>())
            {
                StopRegen(enemy.GetComponent<Health>());
            }
        }
    }

}
