using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CheckArrowColl : MonoBehaviour
{
    private GameObject Enemy;
    private SpawnWave spawnscript;
    private StatsManager SetStats;
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        spawnscript = GameObject.Find("Wave Spawner").GetComponent<SpawnWave>();
        Enemy = gameObject.GetComponentInParent<ShootEnemy>().TargetEnemy;
        SetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
    }

    void Update()
    {
        if (Enemy == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector2 wayPointDirection = new Vector2(Enemy.transform.position.x - transform.position.x, Enemy.transform.position.y - transform.position.y);
            float xPath = Vector2.Dot(wayPointDirection.normalized, transform.right);
            float yPath = Vector2.Dot(wayPointDirection.normalized, transform.up);

            if (transform.parent.tag == "Bombard")
            {
                transform.Translate(Time.fixedDeltaTime * xPath * 4, Time.fixedDeltaTime * yPath * 4, 0);
                GetComponent<CircleCollider2D>().radius = SetStats.Towers[transform.parent.GetComponent<ShootEnemy>().thisTowername].Range / 3;

                if (Mathf.Round(transform.position.x * 10) / 10 == Mathf.Round(Enemy.transform.position.x * 10) / 10 && Mathf.Round(transform.position.y * 10) / 10 == Mathf.Round(Enemy.transform.position.y * 10) / 10)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        HandleDamage(enemy);
                    }
                    Destroy(gameObject);
                }
            }
            else transform.Translate(Time.fixedDeltaTime * xPath * 6, Time.fixedDeltaTime * yPath * 6, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemies.Add(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Enemy)
        {
            HandleDamage(other.gameObject);
            Destroy(gameObject);
        }
    }

    void HandleDamage(GameObject enemy)
    {
        float t = 0;

        StatsManager.Enemynames thisTowersEnemy = enemy.GetComponent<FollowLine>().thisEnemy;
        Transform healthbarTransform = enemy.transform.GetChild(0).GetComponent<Transform>();

        if (transform.parent.name.Contains(SetStats.Towers[transform.parent.GetComponent<ShootEnemy>().thisTowername].Name))
        {
            t = (float)SetStats.Towers[transform.parent.GetComponent<ShootEnemy>().thisTowername].Damage / SetStats.Enemies[thisTowersEnemy].MaxHealth;
            enemy.GetComponent<FollowLine>().health -= SetStats.Towers[transform.parent.GetComponent<ShootEnemy>().thisTowername].Damage;
        }
        healthbarTransform.position = new Vector3(healthbarTransform.position.x - (t / 2), healthbarTransform.position.y, -0.1f);
        healthbarTransform.localScale = new Vector3(healthbarTransform.localScale.x - t, healthbarTransform.localScale.y, 0);

        if (enemy.GetComponent<FollowLine>().health <= 0)
        {
            Destroy(enemy);
            SetStats.ExpCount += SetStats.Enemies[thisTowersEnemy].ExpWorth;
            SetStats.GoldCount += SetStats.Enemies[thisTowersEnemy].GoldWorth;
            spawnscript.EnemyCount--;
            spawnscript.Enemies.Remove(enemy);
        }
    }
}
