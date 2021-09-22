using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CameraBehaviour : MonoBehaviour
{
    private float maxTop = 2.4f;
    private float maxBottom = -2.6f;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Canon")
        {
            transform.position = new Vector3(1.6f, 0, -11);
        }
    }

    void Update()
    {
        float verticalAxis = Input.GetAxis("Vertical");

        if (SceneManager.GetActiveScene().name != "Canon")
        {
            if (transform.position.y <= maxTop && transform.position.y >= maxBottom && verticalAxis != 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (verticalAxis / 2), -11);
            }

            if (transform.position.y >= maxTop)
            {
                transform.position = new Vector3(transform.position.x, maxTop, -11);
            }

            if (transform.position.y <= maxBottom)
            {
                transform.position = new Vector3(transform.position.x, maxBottom, -11);
            }
        }
        else
        {
            if (transform.position.x >= 11.5f)
            {
                transform.position = new Vector3(11.5f, transform.position.y, -10);
            }

            if (transform.position.x <= 0)
            {
                transform.position = new Vector3(0, transform.position.y, -10);
            }

            if (transform.position.y > 0 || transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, -10);
            }

            if (GameObject.Find("CanonCat").GetComponent<Transform>().position.x > gameObject.transform.position.x && transform.position.x <= 11.49f)
            {
                transform.position = new Vector3(GameObject.Find("CanonCat").GetComponent<Transform>().position.x, transform.position.y, -10);
            }
        }
    }
}
