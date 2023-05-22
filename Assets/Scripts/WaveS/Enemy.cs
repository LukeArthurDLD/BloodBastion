using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(WaypointPathfinding))]
public class Enemy : MonoBehaviour
{
    [Header("Identity")]
    public string enemyName;
    public string discription;
    [Header("Sounds")]
    public AudioClip idleSound;
    public AudioClip deathSound;
    private AudioSource audioSource;
    [Header("Statistics")]
    public int hp;
    public int dmg;
    public float speed;
    public int blood = 10;
    [Header("Properties")]
    public bool isArmoured = false;
    public bool isStealthed = false;
    public bool isTaunting = false;
    [Header("Effects")]
    public GameObject deathEffect;
    //components
    private Animator anim;
    private Health health, gameManager;
    private WaypointPathfinding waypointPathfinding;
    private CurrencyManager currencyManager;

    public static List<Enemy> all = new List<Enemy>();

    private void Start()
    {
        // assign to list
        all.Add(this);
        // set stats
        health = GetComponent<Health>();
        health.SetHealth(hp);

        // get components
        gameManager = Player.Instance.gameObject.GetComponent<Health>();
        audioSource = GetComponent<AudioSource>();
        waypointPathfinding = GetComponent<WaypointPathfinding>();
        currencyManager = CurrencyManager.Instance;
        anim = GetComponent<Animator>();

        // play idle sound effect
        if(idleSound && audioSource)
        {
            audioSource.clip = idleSound;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    private void OnDestroy()
    {
        all.Remove(this);
    }

    private void Update()
    {
        if (waypointPathfinding.isPathFinished)
        {
            EndPath();
        }
    }
    void EndPath()
    {
        gameManager.TakeDamage(dmg);
        OnDeath();
    }
    public void WhenKilled()
    {        
        currencyManager.AddCurrency(blood);

        // play sound effects
        if (audioSource)
        {
            audioSource.clip = deathSound;
            audioSource.loop = false;
            audioSource.Play();
        }
        
        if (anim)
            anim.SetBool("Death",true);

        if(deathEffect)
        {
            GameObject effect =  Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
            Destroy(effect, 1.25f);
        }    
        OnDeath();
    }
    void OnDeath()
    {
        all.Remove(this);
        WaveSpawner.EnemiesAlive--;
        HealingField healingField = GetComponent<HealingField>();
        if (healingField != null)
            healingField.OnDeath();

        Destroy(gameObject);
    }
}
