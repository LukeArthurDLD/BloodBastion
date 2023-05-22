using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public int currentBlood = 0;

    public TextMeshProUGUI display;

    public static CurrencyManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        UpdateUI();
    }

    public void AddCurrency(int currency)
    {
        currentBlood += currency;
        UpdateUI();
    }
    private void UpdateUI()
    {
        display.text = currentBlood.ToString("#,##0");
    }
}
