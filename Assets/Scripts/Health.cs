using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [System.NonSerialized]
    public float currentHealth;
    [System.NonSerialized]
    public float maxHealth = 100;

    public ValueDisplay healthbar; //healthbar
    public bool showAtFull = false;

    //health Regeneration
    public bool regenerates = false;
    private int regenAmount = 0;
    private float regenRate = 0f;
    private float regenDelay = 2f;

    private Enemy enemy;
    private Player player;
    [System.NonSerialized]
    public bool isDead = false;
    
    void Start()
    {
        if (healthbar == null)
            healthbar = GetComponentInChildren<ValueDisplay>();

        enemy = GetComponent<Enemy>();
        player = GetComponent<Player>();

        currentHealth = maxHealth;
        UpdateUI();
    }
    private void Update()
    {
        if (healthbar)
        {
            if (!showAtFull && currentHealth == maxHealth)
                healthbar.gameObject.SetActive(false);            
            else
                healthbar.gameObject.SetActive(true);            
        }      
        
    }
    public void ToggleRegen(bool isActive, float rate = 0, float delay = 0, int amount = 0)
    {
        IEnumerator regen = RegenHP();
        regenerates = isActive;

        if (regenerates)
        {
            regenDelay = delay;
            regenRate = rate;
            regenAmount = amount;

            StartCoroutine(regen);
        }
        else if (!regenerates)
        {
            StopCoroutine(regen);
        }
    }
    private IEnumerator RegenHP()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentHealth < maxHealth && !isDead && regenerates)
        {
            if (regenAmount == 0)
                regenAmount++;
            float amount = maxHealth / regenAmount;
            if (amount == 0)
                amount++;
            currentHealth += amount;
            UpdateUI();

            yield return new WaitForSeconds(regenRate);
        }       
    }
    public void Heal(int heal)
    {
        if (currentHealth < maxHealth && !isDead)
        {
            currentHealth += heal;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UpdateUI();
        }
    }
    public void TakeDamage(int damage, Tower.DamageType damageType = Tower.DamageType.Normal)
    {
        if (!isDead)
        {
            if(enemy) //enemy unique calculations
            {
                if (enemy.isArmoured && damageType != Tower.DamageType.ArmourPiercing)
                    return; 
            }       

            // damage is done            
            currentHealth -= (damage);

            //health is capped
            if (currentHealth < 0)
                currentHealth = 0;

            //destroys if dies
            if (currentHealth == 0)
            {
                isDead = true;
                OnDeath();
                return;
            }

            //regen
            if (regenerates && currentHealth != maxHealth)
            {
                StartCoroutine(RegenHP());
            }
            UpdateUI();
        }
    }
    void UpdateUI()
    {
        if (healthbar)
        {
            healthbar.SetMaxValue(maxHealth);
            healthbar.SetValue(currentHealth);
        }    
    }
    void OnDeath()
    {
        UpdateUI();
        if (enemy)
            enemy.WhenKilled();
        if (player)
            player.OnDeath();
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
        UpdateUI();
    }


}
