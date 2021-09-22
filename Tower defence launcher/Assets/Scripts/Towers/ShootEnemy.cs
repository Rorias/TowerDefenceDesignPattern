using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ShootEnemy : MonoBehaviour
{
    private StatsManager.Tower GetStats;
    private TowerManager placetower;

    public Sprite CatFrontSprite;
    public Sprite CatSideSprite;
    public Sprite CatBackSprite;
    public SpriteRenderer catSpriteRenderer;

    [HideInInspector]
    public StatsManager.Towernames thisTowername = StatsManager.Towernames.none;
    public GameObject arrow;
    public GameObject magicbolt;
    public GameObject kitten;
    [HideInInspector]
    public GameObject TargetEnemy = null;

    private float countdown = 1;

    void Start()
    {
        placetower = GameObject.Find("HUD").GetComponent<TowerManager>();
    }

    void Update()
    {
        countdown -= Time.fixedDeltaTime;
        TargetEnemy = null;
        int pos = 0;
        for (pos = 0; pos < 3; pos++)
        {
            if (placetower.towerPlacedObj[pos] == gameObject) break;
        }

        if (pos < 3)
        {
            GetStats = GameObject.Find("Stats Manager").GetComponent<StatsManager>().Towers[placetower.towerPlacedName[pos]];
            TargetEnemy = GameObject.Find("Wave Spawner").GetComponent<SpawnWave>().Enemies.Find((x) => Vector2.Distance(transform.position, x.transform.position) < GetStats.Range);
        }

        if (TargetEnemy != null && name.Any(char.IsDigit) && placetower.towerPlaced)
        {
            float dX = transform.position.x - TargetEnemy.transform.position.x;
            float dY = transform.position.y - TargetEnemy.transform.position.y;
            float RC = (dX == 0) ? 2 : Mathf.Abs(dY / dX);

            if (RC < 1)
            {
                catSpriteRenderer.sprite = CatSideSprite;
                catSpriteRenderer.flipX = (dX < 0);
            }
            else
            {
                catSpriteRenderer.sprite = (dY < 0) ? CatBackSprite : CatFrontSprite;
                catSpriteRenderer.flipX = false;
            }

            //if (TargetEnemy.transform.position.y >= transform.position.y + 3)
            //else if (TargetEnemy.transform.position.x < transform.position.x && TargetEnemy.transform.position.y < transform.position.y + 3)
            //if (TargetEnemy.transform.position.y <= transform.position.y - 3)
            //else if (TargetEnemy.transform.position.x > transform.position.x) && TargetEnemy.transform.position.y > transform.position.y - 3)

            SetAnim(true);

            if (countdown < 0)
            {
                if (tag == "Archer")
                {
                    GameObject clone = (GameObject)Instantiate(arrow, transform.position, Quaternion.identity);
                    clone.transform.parent = transform;
                    clone.transform.position = new Vector3(transform.position.x, transform.position.y + 2, 0);
                }

                if (tag == "Mage")
                {
                    GameObject clone = (GameObject)Instantiate(magicbolt, transform.position, Quaternion.identity);
                    clone.transform.parent = transform;
                }

                if (tag == "Bombard")
                {
                    GameObject clone = (GameObject)Instantiate(kitten, transform.position, Quaternion.identity);
                    clone.transform.parent = transform;
                    clone.transform.localScale = new Vector3(0.3f,0.3f, 1);
                }
                countdown = GetStats.Firerate;
                SetAnim(false);
            }
        }
        else
        {
            SetAnim(false);
        }
    }

    void SetAnim(bool TorF)
    {
        GetComponent<Animator>().SetBool("IsFiring", TorF);
        transform.GetChild(0).GetComponent<Animator>().SetBool("isFiring", TorF);
    }
}
