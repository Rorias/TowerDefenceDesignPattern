using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TowerManager : MonoBehaviour
{
    private GameObject clone = null;
    private Vector3 mousePosition;
    private StatsManager SetStats;
    private Text TooltipText;

    public GameObject[] Loc = new GameObject[3];

    private float[] DistLoc = new float[3];
    private bool canPlaceTowers = true;
    private int towerCounter = 1;

    [HideInInspector]
    public StatsManager.Towernames towerChosen = StatsManager.Towernames.none;
    [HideInInspector]
    public bool towerPlaced = false;
    [HideInInspector]
    public bool[] isUpgraded = new bool[10];
    [HideInInspector]
    public StatsManager.Towernames[] towerPlacedName = new StatsManager.Towernames[3];
    [HideInInspector]
    public GameObject[] towerPlacedObj = new GameObject[3];

    void Start()
    {
        SetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
        TooltipText = GameObject.Find("Tooltips").GetComponent<Text>();
    }

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float ArrowDist = Vector2.Distance(transform.GetChild(0).transform.position, mousePosition);
        float MagicDist = Vector2.Distance(transform.GetChild(1).transform.position, mousePosition);
        float BombardDist = Vector2.Distance(transform.GetChild(2).transform.position, mousePosition);
        float DistSellbutton = Vector2.Distance(GameObject.Find("Sell button").transform.position, mousePosition);
        float DistUpgradebutton = Vector2.Distance(GameObject.Find("Upgrade button").transform.position, mousePosition);

        if (ArrowDist < 1.2f) SetTooltip(StatsManager.Towernames.arrow1);
        else if (MagicDist < 1.1f) SetTooltip(StatsManager.Towernames.magic1);
        else if (BombardDist < 1.1f) SetTooltip(StatsManager.Towernames.bombard1);
        else TooltipText.enabled = false;

        canPlaceTowers = true;
        for (int i = 0; i < 3; i++)
        {
            DistLoc[i] = Vector2.Distance(Loc[i].transform.position, mousePosition);
            canPlaceTowers &= (towerPlacedName[i] != StatsManager.Towernames.none);

            if (towerPlacedName[i] != StatsManager.Towernames.none && towerPlacedObj[i].transform.position == GameObject.Find("Selector").transform.position && GameObject.Find("Selector").GetComponent<MeshRenderer>().enabled)
            {
                if (DistUpgradebutton < 0.75f && !towerPlacedName[i].ToString().Contains("3"))
                {
                    TooltipText.text = "$ " + SetStats.Towers[SetStats.TowerSuccesors[towerPlacedName[i]]].Cost.ToString();
                    GameObject.Find("Tooltips").transform.position = new Vector2(GameObject.Find("Upgrade button").transform.position.x, GameObject.Find("Upgrade button").transform.position.y + 1);
                    TooltipText.alignment = TextAnchor.UpperCenter;
                    TooltipText.enabled = true;
                }

                if (DistLoc[i] < 1)
                {
                    TooltipText.text = Stats(towerPlacedName[i]);
                    GameObject.Find("Tooltips").transform.localPosition = new Vector2(0, 0);
                    TooltipText.alignment = TextAnchor.MiddleLeft;
                    TooltipText.enabled = true;
                }
            }
        }
        canPlaceTowers = !canPlaceTowers;

        if (!towerPlaced)
        {
            if (clone != null)
                clone.transform.position = new Vector3(mousePosition.x, mousePosition.y, -1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (towerChosen == StatsManager.Towernames.none)
            {
                ClickHUDTower(ArrowDist, StatsManager.Towernames.arrow1, transform.GetChild(0).gameObject);
                ClickHUDTower(MagicDist, StatsManager.Towernames.magic1, transform.GetChild(1).gameObject);
                ClickHUDTower(BombardDist, StatsManager.Towernames.bombard1, transform.GetChild(2).gameObject);
            }
            else
            {
                TowerPlace(0);
                TowerPlace(1);
                TowerPlace(2);
            }

            bool sel = false;

            for (int i = 0; i < 3; i++)
            {
                if (!sel) sel = SelectTower(i);

                if (towerPlacedName[i] != StatsManager.Towernames.none && towerPlacedObj[i].transform.position == GameObject.Find("Selector").transform.position)
                {
                    if (DistSellbutton < 0.75f)
                    {
                        if (towerPlacedName[i].ToString().Contains("1")) SetStats.GoldCount += (int)(((float)SetStats.Towers[towerPlacedName[i]].Cost / 100) * 90);
                        else if (towerPlacedName[i].ToString().Contains("2")) SetStats.GoldCount += (int)(((float)(SetStats.Towers[towerPlacedName[i]].Cost + SetStats.Towers[SetStats.TowerPredecessors[towerPlacedName[i]]].Cost) / 100) * 90);
                        else SetStats.GoldCount += (int)(((float)(SetStats.Towers[towerPlacedName[i]].Cost + SetStats.Towers[SetStats.TowerPredecessors[towerPlacedName[i]]].Cost + SetStats.Towers[SetStats.TowerPredecessors[SetStats.TowerPredecessors[towerPlacedName[i]]]].Cost) / 100) * 90);
                        Destroy(towerPlacedObj[i]);
                        towerPlacedObj[i] = null;
                        towerPlacedName[i] = StatsManager.Towernames.none;
                        Deselect(false);
                        TooltipText.enabled = false;
                        towerCounter--;
                    }

                    if (DistUpgradebutton < 0.75f && !towerPlacedName[i].ToString().Contains("3") && SetStats.GoldCount >= SetStats.Towers[SetStats.TowerSuccesors[towerPlacedName[i]]].Cost)
                    {
                        towerPlacedObj[i].GetComponent<Animator>().SetInteger("isUpgraded", towerPlacedObj[i].GetComponent<Animator>().GetInteger("isUpgraded") + 1);
                        //towerPlacedObj[i].transform.GetChild(0).GetComponent<Animator>().SetBool("isFiring", true);
                        towerPlacedName[i] = SetStats.TowerSuccesors[towerPlacedName[i]];
                        SetStats.GoldCount -= SetStats.Towers[towerPlacedName[i]].Cost;
                        towerPlacedObj[i].name = SetStats.Towers[towerPlacedName[i]].Name + " " + towerCounter;
                        towerPlacedObj[i].GetComponent<ShootEnemy>().thisTowername = towerPlacedName[i];
                    }
                }
            }

            if (!sel)
            {
                Deselect(false);
                TooltipText.enabled = false;
            }
        }
    }

    bool SelectTower(int _selectedTower)
    {
        if (DistLoc[_selectedTower] < 1 && (towerPlacedName[_selectedTower] != StatsManager.Towernames.none))
        {
            GameObject.Find("Selector").transform.localScale = new Vector3(SetStats.Towers[towerPlacedName[_selectedTower]].Range * 2, 0, SetStats.Towers[towerPlacedName[_selectedTower]].Range * 2);
            GameObject.Find("Selector").transform.position = Loc[_selectedTower].transform.position;
            Deselect(true);
            return true;
        }
        return false;
    }

    void TowerPlace(int _towerPlacedPos)
    {
        if (!towerPlaced && (towerPlacedName[_towerPlacedPos] == StatsManager.Towernames.none) && DistLoc[_towerPlacedPos] < 1)
        {
            clone.transform.position = Loc[_towerPlacedPos].transform.position;
            towerPlaced = true;
            towerPlacedName[_towerPlacedPos] = towerChosen;
            SetStats.GoldCount -= SetStats.Towers[towerChosen].Cost;
            towerChosen = StatsManager.Towernames.none;
            towerPlacedObj[_towerPlacedPos] = clone;
            towerCounter++;
        }
    }

    void ClickHUDTower(float Dist, StatsManager.Towernames tower, GameObject Tower)
    {
        if (Dist < 1.2f && SetStats.GoldCount >= SetStats.Towers[tower].Cost && canPlaceTowers)
        {
            towerPlaced = false;
            towerChosen = tower;
            clone = (GameObject)Instantiate(Tower, Tower.transform.position, Quaternion.identity);
            clone.GetComponent<ShootEnemy>().thisTowername = towerChosen;
            clone.transform.localScale = new Vector3(4.5f, 4.5f, 1);
            clone.name = SetStats.Towers[tower].Name + " " + towerCounter;
        }
    }

    private void Deselect(bool TorF)
    {
        GameObject.Find("Selector").GetComponent<MeshRenderer>().enabled = TorF;
        GameObject.Find("Sell button").GetComponent<SpriteRenderer>().enabled = TorF;
        GameObject.Find("Upgrade button").GetComponent<SpriteRenderer>().enabled = TorF;
    }

    private void SetTooltip(StatsManager.Towernames _tower)
    {
        TooltipText.text = Stats(_tower);
        GameObject.Find("Tooltips").transform.localPosition = new Vector2(160, 200);
        TooltipText.alignment = TextAnchor.UpperRight;
        TooltipText.enabled = true;
    }

    private string Stats(StatsManager.Towernames tower)
    {
        return SetStats.Towers[tower].Name +
            "\nDamage: " + SetStats.Towers[tower].Damage +
            "\nRange: " + SetStats.Towers[tower].Range +
            "\nFirerate: " + SetStats.FirerateTranslate[SetStats.Towers[tower].Firerate] +
            "\nCost: " + SetStats.Towers[tower].Cost;
    }
}
