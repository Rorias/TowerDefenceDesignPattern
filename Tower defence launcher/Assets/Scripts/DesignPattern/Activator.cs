using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Activator : MonoBehaviour
{
    public static Activator instance;

    public GameObject PrefabEnemy;

    public static GameObject staticPrefab
    {
        get
        {
            return instance.PrefabEnemy;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public static object CreateInstance(Type T)
    {
        Enemy x = new Enemy();
        x.enemy = Instantiate(staticPrefab);
        x.enemy.name = "TESTHEREWATCHCLOSE";
        return x;
    }
}
