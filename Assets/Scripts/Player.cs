using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Statistics")]
    public int hp;
    public int blood = 350;

    //components
    private Health health;
    private CurrencyManager currencyManager;
    private GameStateManager gsManager;
    public static Player Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
        currencyManager.AddCurrency(blood);

        gsManager = GameStateManager.Instance;

        health = GetComponent<Health>();
        health.SetHealth(hp);
    }
    private void Update()
    {
        blood = currencyManager.currentBlood;
    }
    public void OnDeath()
    {
        gsManager.GameOver();
    }
}
