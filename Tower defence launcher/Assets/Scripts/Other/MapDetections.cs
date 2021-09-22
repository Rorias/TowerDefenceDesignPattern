using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MapDetections : MonoBehaviour
{
    private StatsManager SetStats;
    private SpawnWave spawnscript;

    void Start()
    {
        spawnscript = GameObject.Find("Wave Spawner").GetComponent<SpawnWave>();
        SetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>();
    }

    void FixedUpdate()
    {
        RaycastHit2D raycast2D = Physics2D.Linecast(Vector2.zero, Vector2.zero);     

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            raycast2D = Physics2D.Linecast(new Vector2(transform.position.x + 9.5f, transform.position.y - 11), transform.position - new Vector3(13, 11, 0));
            // Debug.DrawLine(new Vector3(transform.position.x + 9.5f, transform.position.y - 11, transform.position.z), transform.position - new Vector3(13, 11, 0), Color.green, 0.0f);
        }
        else
        {
            raycast2D = Physics2D.Linecast(new Vector2(transform.position.x + 9.5f, transform.position.y - 11), transform.position - new Vector3(13, 11, 0));
            Debug.DrawLine(new Vector3(transform.position.x + 9.5f, transform.position.y - 11, transform.position.z), transform.position - new Vector3(13, 11, 0), Color.green, 0.0f);
        }

        if (raycast2D)
        {
            if (raycast2D.collider.gameObject.tag == "Enemy")
            {
                spawnscript.Enemies.Remove(raycast2D.collider.gameObject);
                spawnscript.EnemyCount--;
                SetStats.LifeCount -= SetStats.Enemies[raycast2D.collider.gameObject.GetComponent<FollowLine>().thisEnemy].LivesLost;               
                Destroy(raycast2D.collider.gameObject);
            }
        }
    }
}
