using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUI : MonoBehaviour
{
    [Header("Button UI")]
    public TextMeshProUGUI nameLable;
    public TextMeshProUGUI costLable;
    public Image towerIcon;
    [Header("Effects")]
    public GameObject upgradeEffect;

    [Header("Tower")]
    public Tower tower;
    public TowerPlacementUI parentUI;
    private CurrencyManager currencyManager;

    public void SetTower(Tower t, TowerPlacementUI tp)
    {
        tower = t;
        parentUI = tp;
        if (nameLable) // set name
            nameLable.text = tower.towerName;
        if (costLable) // set cost
            costLable.text = tower.cost.ToString("000");
        if (towerIcon) // set icon
            towerIcon.sprite = tower.towerImage;

    }

    void Start()
    {
        currencyManager = CurrencyManager.Instance;

        Button button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (parentUI.currentTower == null)
        {
            TowerPlacement.Instance.SelectTower(tower);
        }
        else 
        {
            if (tower.cost <= currencyManager.currentBlood)
            {
                // upgrade the current tower
                parentUI.currentTower.Upgrade(tower);

                // upgrade effect
                GameObject effect = Instantiate(upgradeEffect, parentUI.currentTower.transform.position, upgradeEffect.transform.rotation);
                Destroy(effect, 10f);
            }
        }
    }
}
