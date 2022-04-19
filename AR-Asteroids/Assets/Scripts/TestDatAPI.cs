using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDatAPI : MonoBehaviour
{
    Login test;
    void Start()
    {
        test = gameObject.transform.Find("LoginTest").GetComponent<Login>();

        test.addScore();
    }
}
