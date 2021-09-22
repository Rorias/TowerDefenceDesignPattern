using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GoToTutorial : MonoBehaviour
{
    public bool TutorialFinished = false;
    private Color color;
    private ColorBlock colors;

    void Start()
    {
        color = GetComponent<GUITexture>().color;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!TutorialFinished && SceneManager.GetActiveScene().name == "Map")
        {
            color.a += Time.fixedDeltaTime / 3;
            GetComponent<GUITexture>().color = color;
        }
        else if (SceneManager.GetActiveScene().name != "Map")
        {
            color.a = 0;
            GetComponent<GUITexture>().color = color;
        }

        if (Input.GetButton("Fire1") && TutorialFinished)
        {
            color.a += Time.fixedDeltaTime / 8;
            GetComponent<GUITexture>().color = color;
        }

        if (color.a >= 0.5 && !TutorialFinished)
        {
            SceneManager.LoadScene(1);
            color.a = 0;
            GetComponent<GUITexture>().color = color;
        }
    }
}
