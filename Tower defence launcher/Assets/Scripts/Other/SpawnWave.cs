using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SpawnWave : MonoBehaviour
{
    private StatsManager SetStats;
    public List<GameObject> Enemies = null;
    public List<Vector2> wps = null;
    public GameObject Enemy;
    private GameObject clone;

    private int enemiesSpawned = 0;
    public int EnemyCount = 0;

    private float nextWaveIn;
    private float maxnextWaveIn;
    private float nextEnemyIn = 1;

    private bool noNewSpawns = false;
    private bool waveStarted = false;

    public void StartWave()
    {
        Enemies = new List<GameObject>();
        Destroy(GameObject.Find("Tutorial"));
        GameObject.Find("Start").GetComponent<Button>().enabled = false;
        waveStarted = true;
    }

    public void StartWaveEarly()
    {
        GameObject.Find("Start").GetComponent<Button>().enabled = false;
        GameObject.Find("Start").GetComponent<Image>().color = Color.white;
        SetStats.GoldCount += ((int)nextWaveIn * 2) + EnemyCount;
        SetStats.WaveCount++;
        enemiesSpawned = 0;
        noNewSpawns = false;
        nextWaveIn = 10 + SetStats.WaveCount * 2;
        maxnextWaveIn = nextWaveIn;
        GameObject.Find("Start").GetComponent<Image>().fillAmount = 1;
    }

    void Start()
    {
        SetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
        GameObject.Find("Start").GetComponent<Button>().onClick.AddListener(StartWave);
        CreateSceneWayPoints();
        WaveControl();
    }

    void Update()
    {
        if (waveStarted)
        {
            nextEnemyIn -= Time.fixedDeltaTime;
            GameObject.Find("Start").GetComponent<Image>().fillAmount = nextWaveIn / maxnextWaveIn;
            nextWaveIn -= Time.fixedDeltaTime;
        }
        WaveControl();
    }

    private void WaveControl()
    {
        int maxWaveEnemies = SetStats.WaveCount * 2;

        if (!noNewSpawns)
        {
            if (enemiesSpawned < maxWaveEnemies && nextEnemyIn <= 0)
            {
                EnemyCount++;
                enemiesSpawned++;
                CreateEnemies();
                if (enemiesSpawned == maxWaveEnemies)
                {
                    noNewSpawns = true;
                }
            }
        }

        if (SetStats.WaveCount == SetStats.MaxWaves)
        {
            GameObject.Find("Start").GetComponent<Button>().enabled = false;
            if (EnemyCount <= 0 && noNewSpawns)
            {
                GameObject.Find("Fader").GetComponent<GoToTutorial>().TutorialFinished = true;
                if (SetStats.PlayerLevel == 0 && SetStats.ExpCount > 250)
                {
                    SetStats.Skillpoints++;
                    SetStats.PlayerLevel++;
                }
                SceneManager.LoadScene(0);
            }
        }
        else if (nextWaveIn < 10 && waveStarted && EnemyCount <= SetStats.WaveCount && noNewSpawns && SetStats.WaveCount < SetStats.MaxWaves)
        {
            GameObject.Find("Start").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("Start").GetComponent<Button>().onClick.AddListener(StartWaveEarly);
            GameObject.Find("Start").GetComponent<Button>().enabled = true;
            GameObject.Find("Start").GetComponent<Image>().color = Color.green;
        }

        if (EnemyCount <= 0 && noNewSpawns || nextWaveIn <= 0 && SetStats.WaveCount < SetStats.MaxWaves)
        {
            SetStats.WaveCount++;
            enemiesSpawned = 0;
            noNewSpawns = false;
            nextWaveIn = 10 + SetStats.WaveCount * 2;
            maxnextWaveIn = nextWaveIn;
            GameObject.Find("Start").GetComponent<Image>().fillAmount = 1;
        }

    }

    private void CreateEnemies()
    {
        clone = (GameObject)Instantiate(Enemy, transform.position, Quaternion.identity);
        Enemies.Add(clone);
        nextEnemyIn = 1f;

        switch (SetStats.WaveCount)
        {
            case 1:
                EnemyCreation(StatsManager.Enemynames.man);
                break;
            case 2:
                switch (enemiesSpawned)
                {
                    case 4:
                        EnemyCreation(StatsManager.Enemynames.soldier);
                        break;
                    default:
                        EnemyCreation(StatsManager.Enemynames.man);
                        break;
                }
                break;
            case 3:
                switch (enemiesSpawned)
                {
                    case 4:
                    case 5:
                    case 6:
                        EnemyCreation(StatsManager.Enemynames.thief);
                        break;
                    default:
                        EnemyCreation(StatsManager.Enemynames.man);
                        break;
                }
                break;
            case 4:
                switch (enemiesSpawned)
                {
                    case 3:
                        EnemyCreation(StatsManager.Enemynames.armysoldier);
                        break;
                    case 6:
                    case 7:
                    case 8:
                        EnemyCreation(StatsManager.Enemynames.hippie);
                        break;
                    default:
                        EnemyCreation(StatsManager.Enemynames.man);
                        break;
                }
                break;
            case 5:
                switch (enemiesSpawned)
                {
                    case 10:
                        EnemyCreation(StatsManager.Enemynames.executioner);
                        break;
                    default:
                        EnemyCreation(StatsManager.Enemynames.man);
                        break;
                }
                break;
            case 6:
                switch (enemiesSpawned)
                {
                    case 12:
                        EnemyCreation(StatsManager.Enemynames.bob);
                        break;
                    default:
                        EnemyCreation(StatsManager.Enemynames.man);
                        break;
                }
                break;
        }
    }

    private void EnemyCreation(StatsManager.Enemynames enemyname)
    {
        clone.GetComponent<FollowLine>().thisEnemy = enemyname;
    }

    private void CreateSceneWayPoints()
    {
        wps = new List<Vector2>();
        switch (SceneManager.GetActiveScene().name)
        {
            case "Tutorial":
                SetStats.MaxWaves = 6;
                wps.Add(new Vector2(-7.5f, 9.5f));
                wps.Add(new Vector2(-7f, 7));
                wps.Add(new Vector2(-7, 5.5f));
                wps.Add(new Vector2(-7.5f, 3));
                wps.Add(new Vector2(-7.5f, 1));
                wps.Add(new Vector2(-7, 0));
                wps.Add(new Vector2(-6.5f, -1));
                wps.Add(new Vector2(-5.5f, -2));
                wps.Add(new Vector2(-4.5f, -3));
                wps.Add(new Vector2(-3.5f, -4.5f));
                wps.Add(new Vector2(-4f, -11));
                break;
            case "Level 1":
                SetStats.MaxWaves = 30;
                
                break;
        }
    }
}