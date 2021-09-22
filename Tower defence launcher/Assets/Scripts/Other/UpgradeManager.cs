using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UpgradeManager : MonoBehaviour
{
    private StatsManager GetStats;
    private ColorBlock[] colors = new ColorBlock[18];
    private EventSystem clickHandler = null;
    public bool[] UpgradeBought = new bool[18];

    public int[] upgrades = new int[18];

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode LSM)
    {
        scene = SceneManager.GetActiveScene();
        LSM = LoadSceneMode.Single;

        if (SceneManager.GetActiveScene().name != "Skilltree")
            gameObject.GetComponent<Canvas>().enabled = false;
        else
        {
            GetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
            GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
            clickHandler = EventSystem.current;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < transform.childCount - 7; i++)
        {
            colors[i] = transform.GetChild(i).GetComponent<Button>().colors;
        }
    }

    void Update()
    {
        if (GetStats != null)
            transform.GetChild(20).GetComponent<Text>().text = "Skillpoints: " + GetStats.Skillpoints.ToString();

        if (clickHandler != null)
        {
            for (int i = 0; i < transform.childCount - 7; i += 4)
            {
                SetUpgraded(i, 0);
                SetUpgraded(i, 1);
                SetUpgraded(i, 2);
            }
        }
    }

    void SetUpgraded(int i, int plusI)
    {
        if (transform.GetChild(i + plusI).GetComponent<Button>().colors.normalColor == Color.grey && clickHandler.currentSelectedGameObject == transform.GetChild(i + plusI).gameObject && GetStats.Skillpoints > plusI)
        {
            colors[i + plusI].normalColor = Color.green;
            transform.GetChild(i + plusI).GetComponent<Button>().colors = colors[i + plusI];
            colors[i + plusI].normalColor = Color.grey;
            colors[i + plusI].colorMultiplier = 1;
            transform.GetChild(i + 1 + plusI).GetComponent<Button>().interactable = true;
            transform.GetChild(i + 1 + plusI).GetComponent<Button>().colors = colors[i + plusI];
            GetStats.Skillpoints -= plusI + 1;
            Debug.Log(transform.GetChild(i + plusI));
            if (transform.GetChild(i + plusI).name == "Archers 1")
            {

                UpgradeBought[0] = true;
                StatsManager.Tower t = GetStats.Towers[StatsManager.Towernames.arrow1];
                StatsManager.Tower t2 = GetStats.Towers[StatsManager.Towernames.arrow2];
                StatsManager.Tower t3 = GetStats.Towers[StatsManager.Towernames.arrow3];

                t.Range = 7;
                t2.Range = 8;
                t3.Range = 9;

                GetStats.Towers[StatsManager.Towernames.arrow1] = t;
                GetStats.Towers[StatsManager.Towernames.arrow2] = t2;
                GetStats.Towers[StatsManager.Towernames.arrow3] = t3;
            }
        }
    }
}
