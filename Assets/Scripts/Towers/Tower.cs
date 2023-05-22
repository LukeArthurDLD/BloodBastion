using UnityEngine;
[RequireComponent(typeof(TowerTargeting))]
public class Tower : MonoBehaviour
{
    [Header("Identity")]
    public string towerName;
    public string towerDiscription;
    public Sprite towerImage;

    [Header("Sounds")]
    public AudioClip upgradeSound;
    [System.NonSerialized]
    public AudioSource audioSource;
    [Header("Attacking")]
    public float range = 15f;
    public int damage = 10;
    public enum DamageType { Normal, ArmourPiercing };
    public DamageType damageType;
    public float attackSpeed = 2.5f;
    public bool detectCamo = false;
    [Header("Economy")]
    public int cost = 150;      

    private Weapon weapon;
    private TowerTargeting targeting;
    private CurrencyManager currencyManager;
    private TowerPlacement towerPlacement;


    public Tower[] upgrades;
    
    private void Start()
    {
        currencyManager = CurrencyManager.Instance;
        audioSource = GetComponent<AudioSource>();
        towerPlacement = TowerPlacement.Instance;

        weapon = GetComponent<Weapon>();
        targeting = GetComponent<TowerTargeting>();

        weapon.damageType = damageType;
    }

    [ContextMenu("Select")]
    void DebugSelect()
    {
        FindObjectOfType<TowerPlacementUI>().SetCurrentTower(this);
    }
    public void Upgrade(Tower upgrade)
    {
        {
            if (audioSource)
            {
                audioSource.clip = upgradeSound;
                audioSource.Play();
            }
            GameObject newTower = Instantiate(upgrade.gameObject, transform.position, transform.rotation);
            newTower.GetComponent<TowerTargeting>().isTowerPlaced = true;

            currencyManager.AddCurrency(-upgrade.cost);

            towerPlacement.SelectPlacedTower(newTower.GetComponent<Tower>());

            Destroy(gameObject);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
