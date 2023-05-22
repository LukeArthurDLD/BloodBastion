using UnityEngine;

public class TowerPlacement : MonoBehaviour   
{
    public Transform cursor;
    private Touch touch;

    private Camera mainCamera;
    
    [SerializeField]
    private LayerMask groundMask, pathMask;
    public Tower selectedTower;
    private GameObject currentTower;
    private CurrencyManager currencyManager;
    private bool isPlaceable = false;

    private TowerPlacementUI placementUI;

    public static TowerPlacement Instance;

    [Header("Effects")]
    public GameObject placeEffect;
    public Material yesPlaceMat;
    public Material noPlaceMat;

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

    private void Start()
    {
        // Set Managers
        placementUI = TowerPlacementUI.Instance;
        currencyManager = CurrencyManager.Instance;
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {        
         MyInput();               
    }

    void PlaceTower(Tower tower)
    {
        Object.Destroy(currentTower);
        selectedTower = null;

        GameObject effect = Instantiate(placeEffect, cursor.transform.position, placeEffect.transform.rotation);
        Destroy(effect, 2f);

        currencyManager.AddCurrency(-tower.cost);
        GameObject placedTower = Instantiate(tower.gameObject, cursor.position, Quaternion.identity);
        placedTower.GetComponent<TowerTargeting>().PlaceTower();
        placementUI.SetCurrentTower(placedTower.GetComponent<Tower>());       

    }
    public void SelectTower(Tower tower)
    {
        Deselect();
        if (selectedTower != null)
        {
            Object.Destroy(currentTower);
            selectedTower = null;
        }

        if (tower.cost <= currencyManager.currentBlood)
        {
            selectedTower = tower;

            currentTower = Instantiate(tower.gameObject, cursor);
            currentTower.transform.position = cursor.position;

            currentTower.GetComponent<TowerTargeting>().ShowRadius(true);
            currentTower.GetComponent<Collider>().enabled = false;
            Debug.Log("Tower selected " + selectedTower);
        }
        else
            Debug.Log("Insufficient Cost");       
    }
    void TouchInput()
    {
        bool hasTouched = false;

        if (Input.touchCount > 0 )
        {
            Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            cursor.position = touchPosition;

            if (selectedTower != null)
            {
                hasTouched = true;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && Utility.MouseOverUI() == false)
                {
                    GameObject towerObject = hit.collider.gameObject;

                    if (towerObject.GetComponent<Tower>())
                        placementUI.SetCurrentTower(towerObject.GetComponent<Tower>());
                    else
                        placementUI.Deselect();
                }                   
            }
        }
        if (Input.touchCount == 0 && hasTouched)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && Utility.MouseOverUI() == false)
            {
                if (hit.collider.gameObject.tag == "PlacableGround")
                {
                    PlaceTower(selectedTower);

                    Object.Destroy(currentTower);
                    selectedTower = null;
                }
                hasTouched = false;
            }
        }
    }
    void MyInput()
    {
        cursor.position = GetMousePos().point;
        if (!isPlaceable && GetMousePos(true).collider.gameObject.tag == "PlacableGround")
        {
            isPlaceable = true;
            if(yesPlaceMat && currentTower != null)
                SetPlacementShader(currentTower, isPlaceable);

        }
        else if(isPlaceable && GetMousePos(true).collider.gameObject.tag != "PlacableGround")
        {
            isPlaceable = false;
            if (yesPlaceMat && currentTower != null)
                SetPlacementShader(currentTower, isPlaceable);

        }

        if (Input.GetButtonDown("Fire1") && Utility.MouseOverUI() == false)
        {
            if (selectedTower != null && SystemInfo.deviceType == DeviceType.Desktop)
            {
                if (isPlaceable)
                     PlaceTower(selectedTower);
            }
            else
                FindTower();
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld && Input.GetButtonUp("Fire1") && Utility.MouseOverUI() == false)
        {
            if (selectedTower != null)
            {
                if (isPlaceable)
                    PlaceTower(selectedTower);
            }
        }
    }    
    void FindTower()
    {
        GameObject towerObject = GetMousePos(true).collider.gameObject;
        Debug.Log(towerObject.name);
        if (towerObject.GetComponent<Tower>())
        {
            SelectPlacedTower(towerObject.GetComponent<Tower>());
        }
        else
        {
            Deselect();
        }
    }
    public void SelectPlacedTower(Tower tower)
    {
        Deselect();

        placementUI.SetCurrentTower(tower);
        tower.gameObject.GetComponent<TowerTargeting>().ShowRadius(true);

    }
    public void Deselect()
    {
        if (placementUI)
            placementUI.Deselect();

        TowerTargeting[] targets = FindObjectsOfType<TowerTargeting>();
        foreach(TowerTargeting target in targets)
        {
            target.ShowRadius(false);
        }
    }

    private RaycastHit GetMousePos(bool getTower = false)
    {
        
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane);

        Vector3 worldMousePosFar = mainCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = mainCamera.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;   
        
        if(!getTower)
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.MaxValue, groundMask | pathMask);
        else
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, float.MaxValue);

        return hit;
    }   
    private void SetPlacementShader(GameObject tower, bool canPlace)
    {
        MeshRenderer[] towerMesh = tower.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] towerMeshSkin = tower.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach(MeshRenderer r in towerMesh)
        {
            if (r.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                if (!canPlace)
                    r.material.color = Color.red;
               
                continue;
            }

            if (canPlace)
                r.material = yesPlaceMat;
            else
                r.material = noPlaceMat; 
        }
        foreach (SkinnedMeshRenderer r in towerMeshSkin)
        {            
            if (canPlace)
                r.material = yesPlaceMat;
            else
                r.material = noPlaceMat;
        }
    }
}
