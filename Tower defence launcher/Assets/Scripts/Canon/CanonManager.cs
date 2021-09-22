using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CanonManager : MonoBehaviour
{
    private bool filled = false;
    private bool maxAngleReached = false;

    void Update()
    {
        if (filled)
        {
            GameObject.Find("Power Bar").GetComponent<Image>().fillAmount -= Time.fixedDeltaTime;
            filled = Convert.ToBoolean(GameObject.Find("Power Bar").GetComponent<Image>().fillAmount);
        }
        else
        {
            GameObject.Find("Power Bar").GetComponent<Image>().fillAmount += Time.fixedDeltaTime;
            if (GameObject.Find("Power Bar").GetComponent<Image>().fillAmount == 1) filled = true;
        }

        if (!maxAngleReached)
        {
            transform.rotation = new Quaternion(0, 0, transform.rotation.z - (Time.fixedDeltaTime / 3), 1);
            if (transform.rotation.z <= 0) maxAngleReached = true;
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, transform.rotation.z + (Time.fixedDeltaTime / 3), 0.9f);
            if (transform.rotation.z >= 0.4f) maxAngleReached = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            GameObject.Find("CanonCat").GetComponent<Transform>().rotation = new Quaternion(0, 0, transform.rotation.z, 1);
            GameObject.Find("CanonCat").GetComponent<Rigidbody2D>().gravityScale = 1;
            GameObject.Find("CanonCat").GetComponent<Rigidbody2D>().velocity = new Vector2(10 * GameObject.Find("Power Bar").GetComponent<Image>().fillAmount, (transform.rotation.z * 40) * GameObject.Find("Power Bar").GetComponent<Image>().fillAmount);
        }
    }
}
