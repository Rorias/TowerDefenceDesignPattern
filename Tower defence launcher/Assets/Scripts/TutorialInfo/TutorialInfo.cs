using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TutorialInfo : MonoBehaviour
{
    private const string TutorialInfo1 = "Hello, and welcome to this Tower Cattack.. I mean.. Tower Defence game";
    private const string TutorialInfo2 = "Over here are your towers. Click one to select it, and then click the area where you want it to be on the map";
    private const string TutorialInfo3 = "Here you can see all your stats. Lives, Gold, Exp, the current level and the wave you're in";
    private const string TutorialInfo4 = "Press W (or up) and S (or down) to shift the cameraview";
    private const string TutorialInfo5 = "This is the button to start the game. Click it when you're ready. Good Luck!";

    void Start()
    {
        GetComponent<Text>().text = TutorialInfo1;
    }

    void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (Input.GetMouseButtonDown(0))
        {
            switch (GetComponent<Text>().text)
            {
                case TutorialInfo1:
                    GetComponent<Text>().text = TutorialInfo2;
                    rt.sizeDelta = new Vector2(720, 1);
                    transform.localPosition = new Vector2(100, 200);
                    break;
                case TutorialInfo2:
                    GetComponent<Text>().text = TutorialInfo3;
                    rt.sizeDelta = new Vector2(910, 1);
                    transform.localPosition = new Vector2(90, -110);
                    break;
                case TutorialInfo3:
                    GetComponent<Text>().text = TutorialInfo4;
                    rt.sizeDelta = new Vector2(950, 1);
                    transform.localPosition = new Vector2(0, 20);
                    break;
                case TutorialInfo4:
                    GetComponent<Text>().text = TutorialInfo5;
                    rt.sizeDelta = new Vector2(950, 1);
                    transform.localPosition = new Vector2(90, -180);
                    break;
                case TutorialInfo5:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
