using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IPoolable
{
    public bool active { get; set; }
    public event Action<Enemy> OnDie;

    public GameObject enemy;

    public void Die()
    {
        OnDie?.Invoke(this);
    }

    public void OnEnableObject()
    {

    }

    public void OnDisableObject()
    {
        OnDie = null;
    }
}
