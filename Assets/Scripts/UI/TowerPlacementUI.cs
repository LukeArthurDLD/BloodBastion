using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementUI : MonoBehaviour
{
    public TowerUI towerUI;
    List<TowerUI> uiList = new List<TowerUI>();
    
    public Tower[] towers;
    public Tower currentTower = null;
    public static TowerPlacementUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SetCurrentTower(null);
    }

    public void SetCurrentTower(Tower tower)
    {
        currentTower = tower;
        foreach(TowerUI ui in uiList)
        {
            Destroy(ui.gameObject);
        }
        uiList.Clear();

        Tower[] ts = tower == null ? towers : tower.upgrades;

        foreach(Tower t in ts)
        {
            TowerUI ui = Instantiate(towerUI, transform);
            ui.SetTower(t, this);
            uiList.Add(ui);
        }
    }

    [ContextMenu("Show Main")]
    public void Deselect()
    {
        SetCurrentTower(null);
    }
}
