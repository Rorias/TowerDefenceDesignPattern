using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class FollowLine : MonoBehaviour
{
    private SpawnWave spawnscript;
    private StatsManager GetStats;
    public StatsManager.Enemynames thisEnemy = StatsManager.Enemynames.none;
    private int currentWayPoint = 0;
    public int health;

    private void Start()
    {
        spawnscript = GameObject.Find("Wave Spawner").GetComponent<SpawnWave>();
        GetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
        if (gameObject.name == GetStats.Enemies[StatsManager.Enemynames.thief].Name) GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Enemies/Runner");
        else GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Enemies/guay");
        name = GetStats.Enemies[thisEnemy].Name;
        health = GetStats.Enemies[thisEnemy].MaxHealth;
    }

    private void Update()
    {
        Vector2 wayPointDirection = new Vector2(spawnscript.wps[currentWayPoint].x - transform.position.x, spawnscript.wps[currentWayPoint].y - transform.position.y);

        float xPath = Vector2.Dot(wayPointDirection.normalized, transform.right);
        float yPath = Vector2.Dot(wayPointDirection.normalized, transform.up);

        transform.Translate(Time.deltaTime * xPath * GetStats.Enemies[thisEnemy].Speed, Time.deltaTime * yPath * GetStats.Enemies[thisEnemy].Speed, 0);

        if (Mathf.Round(transform.position.x * 10) / 10 == spawnscript.wps[currentWayPoint].x && Mathf.Round(transform.position.y * 10) / 10 == spawnscript.wps[currentWayPoint].y)
        {
            currentWayPoint++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), other.gameObject.GetComponent<BoxCollider2D>());
        }
    }
}
