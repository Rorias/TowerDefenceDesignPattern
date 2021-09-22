using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class HitBonus : MonoBehaviour
{
    private StatsManager SetStats;
    private int currentLevel;
    private float waitaSecond = 1;
    private bool hasLanded = false;

    void Awake()
    {
        currentLevel = 2;
    }

    void Start()
    {
        SetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
    }

    void Update()
    {
        if (hasLanded) waitaSecond -= Time.fixedDeltaTime;

        if (waitaSecond <= 0)
        {
            SceneManager.LoadScene(currentLevel);
            currentLevel++;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Bonus"))
        {
            switch (other.gameObject.name)
            {
                case "Bonus 5":
                    SetStats.Bonus = 5;
                    break;
                case "Bonus 10":
                    SetStats.Bonus = 10;
                    break;
                case "Bonus 15":
                    SetStats.Bonus = 15;
                    break;
                case "Bonus 20":
                    SetStats.Bonus = 20;
                    break;
                case "Bonus 50":
                    SetStats.Bonus = 50;
                    break;
                case "Bonus 0":
                    SetStats.Bonus = 0;
                    break;
            }
            hasLanded = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
