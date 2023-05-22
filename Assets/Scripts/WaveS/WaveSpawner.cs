using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [Header("Path Settings")]
    public WaypointManager[] path;
    private int pathIndex = 0;
    public enum PathType { None, Wave, Enemy, Group, OnRounds };
    public PathType pathType;
    public int[] onRounds;

    [Header("Wave Settings")]
    public Wave[] waves;
    private int waveIndex = 0;
    private bool isWaveSpawning = false;
    //time
    public float timeBetweenWaves = 10f;
    public float countdown = 20f;
    public static int EnemiesAlive = 0;
    //money
    private CurrencyManager currencyManager;
    public int endRoundBlood = 100;

    [Header("Sounds")]
    public GameObject singleFootstepSound;
    public GameObject multipleFootstepSound;

    [Header("UI")]
    public TextMeshProUGUI waveCount;
    public TextMeshProUGUI countDownText;
   
    private void Start()
    {
        if (waveCount) //text
            waveCount.text = (waveIndex).ToString() + "/" + waves.Length.ToString();

        currencyManager = CurrencyManager.Instance;

        isWaveSpawning = false;
        EnemiesAlive = 0;
        pathIndex = 0;
    }
    private void Update()
    {
        FootstepSound();

        if (EnemiesAlive > 0 || isWaveSpawning)
            return;        

        if(countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            if(countDownText)
                countDownText.gameObject.SetActive(false);
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        if (countDownText)
        {
            if (countDownText.gameObject.activeInHierarchy == false)
                countDownText.gameObject.SetActive(true);
            countDownText.text = string.Format("{0:00.00}", countdown);
        }       

    }
    IEnumerator SpawnWave()
    {
        isWaveSpawning = true;

        if (waveCount) //text
            waveCount.text = (waveIndex + 1).ToString() + "/" + waves.Length.ToString();

        Wave wave = waves[waveIndex];
                
        for (int i = 0; i < wave.groups.Length; i++)
        {
            for (int j = 0; j < wave.groups[i].count; j++)
            {
                SpawnEnemy(wave.groups[i].enemy);
                yield return new WaitForSeconds(wave.groups[i].rate);
            }
            if (pathType == PathType.Group)
                PathIndexUp();

            yield return new WaitForSeconds(wave.groups[i].delay);
            
        }
        yield return new WaitUntil(() => EnemiesAlive == 0);       
        EndWave();
    }
    void EndWave()
    {
        currencyManager.AddCurrency(endRoundBlood);
        isWaveSpawning = false;
        waveIndex++;
        

        if (pathType == PathType.Wave)
            PathIndexUp();

        if (pathType == PathType.OnRounds)
        {
            for (int i = 0; i < onRounds.Length; i++)
            {
                if (waveIndex + 1 == onRounds[i])
                    PathIndexUp();
            }
        }
        if (waveIndex == waves.Length)
            this.enabled = false;
    }
    void SpawnEnemy(Enemy enemy)
    {
        enemy.GetComponent<WaypointPathfinding>().manager = path[pathIndex];

        Transform start = path[pathIndex].waypoints[0];
        Instantiate(enemy, start.position, start.rotation);

        if (pathType == PathType.Enemy)
            PathIndexUp();

        EnemiesAlive++;        
    }
    void PathIndexUp()
    {
        pathIndex++;
        if (pathIndex >= path.Length)
            pathIndex = 0;

        Debug.Log("path is now " + pathIndex);
    }
    void FootstepSound()
    {
        if (EnemiesAlive > 1)
        {
            singleFootstepSound.SetActive(false);
            multipleFootstepSound.SetActive(true);
        }
        else if (EnemiesAlive == 1)
        {
            multipleFootstepSound.SetActive(false);
            singleFootstepSound.SetActive(true);
        }
        else
        {
            multipleFootstepSound.SetActive(false);
            singleFootstepSound.SetActive(false);
        }
    }

    [ContextMenu("Battle Computer")]
    void BattleComputer()
    {
        int totalCoin = 0;
        int index = 0;
        System.IO.StreamWriter file = new System.IO.StreamWriter("BC.txt", false);
        foreach (Wave wave in waves)
        {
            index++;
            int waveCoin = 0;
            foreach (Groups group in wave.groups)
            {
                waveCoin += group.enemy.blood * group.count;
            }
            waveCoin += endRoundBlood;
            totalCoin += waveCoin;
            Debug.Log("Wave #" + index + "  " + waveCoin + "  " + totalCoin);
            file.Write("" + index + "," + waveCoin + "," + totalCoin + "\n");
        }
        file.Close();
    }
}

