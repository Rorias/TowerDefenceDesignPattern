using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StatsManager : MonoBehaviour
{
    public enum Towernames { none, arrow1, arrow2, arrow3, magic1, magic2, magic3, bombard1, bombard2, bombard3, barracks1, barracks2, barracks3 }
    public enum Enemynames { none, man, soldier, armysoldier, thief, executioner, hippie, bob }
    public Dictionary<float, string> FirerateTranslate = new Dictionary<float, string>();

    private UpgradeManager ArrowUp;

    public struct Tower
    {
        public string Name;
        public int Damage;
        public float Range;
        public float Firerate;
        public int Cost;
    }

    public struct Enemy
    {
        public string Name;
        public float Speed;
        public int MaxHealth;
        public int LivesLost;
        public int Damage;
        public int GoldWorth;
        public int ExpWorth;
    }

    public Dictionary<Towernames, Tower> Towers = new Dictionary<Towernames, Tower>();
    public Dictionary<Enemynames, Enemy> Enemies = new Dictionary<Enemynames, Enemy>();

    public Dictionary<Towernames, Towernames> TowerSuccesors = new Dictionary<Towernames, Towernames>();
    public Dictionary<Towernames, Towernames> TowerPredecessors = new Dictionary<Towernames, Towernames>();

    private Text GoldCounter;
    private Text LifeCounter;
    private Text ExpCounter;
    private Text WaveCounter;

    [HideInInspector]
    public int PlayerLevel = 0;
    [HideInInspector]
    public int Skillpoints = 0;
    [HideInInspector]
    public int MaxWaves;
    [HideInInspector]
    public int WaveCount;
    [HideInInspector]
    public int GoldCount;
    [HideInInspector]
    public int LifeCount;
    [HideInInspector]
    public int ExpCount;
    [HideInInspector]
    public int Bonus;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        ArrowUp = GameObject.Find("Canvas").GetComponent<UpgradeManager>();

        #region TowerScheme
        Towers.Add(Towernames.arrow1, new Tower { Name = "Scratch Outpost", Damage = 25, Range = 6, Firerate = 1, Cost = 60 });
        Towers.Add(Towernames.arrow2, new Tower { Name = "Catstle Outpost", Damage = 35, Range = 7, Firerate = 1, Cost = 90 });
        Towers.Add(Towernames.arrow3, new Tower { Name = "??? Outpost", Damage = 40, Range = 8, Firerate = 0.5f, Cost = 130 });
        Towers.Add(Towernames.magic1, new Tower { Name = "Catacoil", Damage = 40, Range = 6, Firerate = 1.5f, Cost = 80 });
        Towers.Add(Towernames.magic2, new Tower { Name = "Catacomb", Damage = 70, Range = 6, Firerate = 1.5f, Cost = 130 });
        Towers.Add(Towernames.magic3, new Tower { Name = "Catastrophy", Damage = 110, Range = 6, Firerate = 1, Cost = 200 });
        Towers.Add(Towernames.bombard1, new Tower { Name = "Kittenpult", Damage = 30, Range = 5, Firerate = 2.5f, Cost = 100 });
        Towers.Add(Towernames.bombard2, new Tower { Name = "Catapult", Damage = 50, Range = 6, Firerate = 2, Cost = 150 });
        Towers.Add(Towernames.bombard3, new Tower { Name = "Trebuchat", Damage = 80, Range = 7, Firerate = 1.5f, Cost = 180 });
        Towers.Add(Towernames.barracks1, new Tower { Name = "Cat house", Damage = 10, Range = 3, Firerate = 0, Cost = 60 });
        Towers.Add(Towernames.barracks2, new Tower { Name = "Cattacks", Damage = 15, Range = 3, Firerate = 0, Cost = 80 });
        Towers.Add(Towernames.barracks3, new Tower { Name = "Catarangs", Damage = 25, Range = 5, Firerate = 1, Cost = 130 });

        TowerSuccesors.Add(Towernames.arrow1, Towernames.arrow2);
        TowerSuccesors.Add(Towernames.arrow2, Towernames.arrow3);
        TowerSuccesors.Add(Towernames.magic1, Towernames.magic2);
        TowerSuccesors.Add(Towernames.magic2, Towernames.magic3);
        TowerSuccesors.Add(Towernames.bombard1, Towernames.bombard2);
        TowerSuccesors.Add(Towernames.bombard2, Towernames.bombard3);
        TowerSuccesors.Add(Towernames.barracks1, Towernames.barracks2);
        TowerSuccesors.Add(Towernames.barracks2, Towernames.barracks3);

        TowerPredecessors.Add(Towernames.arrow3, Towernames.arrow2);
        TowerPredecessors.Add(Towernames.arrow2, Towernames.arrow1);
        TowerPredecessors.Add(Towernames.magic3, Towernames.magic2);
        TowerPredecessors.Add(Towernames.magic2, Towernames.magic1);
        TowerPredecessors.Add(Towernames.bombard3, Towernames.bombard2);
        TowerPredecessors.Add(Towernames.bombard2, Towernames.bombard1);
        TowerPredecessors.Add(Towernames.barracks3, Towernames.barracks2);
        TowerPredecessors.Add(Towernames.barracks2, Towernames.barracks1);
        #endregion

        #region EnemyScheme
        Enemies.Add(Enemynames.man, new Enemy { Name = "Guy", Speed = 1, MaxHealth = 100, Damage = 1, LivesLost = 1, GoldWorth = 5, ExpWorth = 5 });
        Enemies.Add(Enemynames.soldier, new Enemy { Name = "Militia", Speed = 1, MaxHealth = 150, Damage = 3, LivesLost = 1, GoldWorth = 10, ExpWorth = 5 });
        Enemies.Add(Enemynames.armysoldier, new Enemy { Name = "General", Speed = 0.8f, MaxHealth = 220, Damage = 5, LivesLost = 1, GoldWorth = 15, ExpWorth = 10 });
        Enemies.Add(Enemynames.hippie, new Enemy { Name = "Dude", Speed = 1.4f, MaxHealth = 180, Damage = 1, LivesLost = 1, GoldWorth = 15, ExpWorth = 15 });
        Enemies.Add(Enemynames.thief, new Enemy { Name = "Runner", Speed = 2, MaxHealth = 80, Damage = 4, LivesLost = 2, GoldWorth = 25, ExpWorth = 10 });
        Enemies.Add(Enemynames.executioner, new Enemy { Name = "Executive Cutter", Speed = 0.5f, MaxHealth = 500, Damage = 25, LivesLost = 5, GoldWorth = 50, ExpWorth = 25 });
        Enemies.Add(Enemynames.bob, new Enemy { Name = "bob", Speed = 0.3f, MaxHealth = 5000, Damage = 100, LivesLost = 100, GoldWorth = 200, ExpWorth = 50 });
        #endregion

        FirerateTranslate.Add(0.5f, "Very fast");
        FirerateTranslate.Add(1, "Fast");
        FirerateTranslate.Add(1.5f, "Average");
        FirerateTranslate.Add(2, "Slow");
        FirerateTranslate.Add(2.5f, "Very Slow");

        LifeCount = 10;
        ExpCount = 0;
        WaveCount = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode LSM)
    {
        scene = SceneManager.GetActiveScene();
        LSM = LoadSceneMode.Single;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            GoldCounter = GameObject.Find("Gold").GetComponent<Text>();
            LifeCounter = GameObject.Find("Lives").GetComponent<Text>();
            ExpCounter = GameObject.Find("Exp").GetComponent<Text>();
            WaveCounter = GameObject.Find("Wave").GetComponent<Text>();
        }

        if (SceneManager.GetActiveScene().name == "Skilltree") GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        GoldCount = 300 + Bonus;
    }

    void Update()
    {
        if (GoldCounter != null)
        {
            GoldCounter.text = GoldCount.ToString();
            LifeCounter.text = LifeCount.ToString();
            ExpCounter.text = ExpCount.ToString();
            WaveCounter.text = WaveCount.ToString() + "/" + MaxWaves.ToString();
        }
    }
}
