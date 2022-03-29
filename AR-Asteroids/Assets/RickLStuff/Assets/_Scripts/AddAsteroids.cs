using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AddAsteroids : MonoBehaviour
{

    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;
    public GameObject asteroid4;
    public GameObject asteroid5;
    public GameObject asteroid6;
    public GameObject Drone;
    public GameObject Satellite;

    float recordedTime;
    float startTime;
    public float asteroidDelta;
    public float myDiff;
    int index = 0;
    private ManagerGame managerGameObject;
    private ManagerHUD managerHudObject;
    private Text countdown;

    void Start()
    {
        recordedTime = Time.time;
        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
        managerHudObject = GameObject.FindGameObjectWithTag("ManagerHUD").GetComponent<ManagerHUD>();
        managerGameObject.setAsteroidSpeed(managerHudObject.GetEntryLevel() * 1.25f);
        asteroidDelta = managerGameObject.getAsteroidDelta();
        //countdown = GameObject.FindGameObjectWithTag("WarningPanelText").GetComponent<Text>();
    }

    void Update()
    {
        bool gameIsInProgress = managerGameObject.getGameIsInProgress();
        if (!gameIsInProgress) asteroidDelta = managerGameObject.getAsteroidDelta();

        if (Time.time - recordedTime >= asteroidDelta && gameIsInProgress)
        {
            myDiff = 0.05f * asteroidDelta;
            if (asteroidDelta > 0.25) asteroidDelta -= myDiff;
            float x = 6 - (UnityEngine.Random.value * 12);
            switch (index)
            {
                case 0:
                    Instantiate(asteroid1, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(asteroid2, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(asteroid3, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(asteroid4, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 4:
                    Instantiate(asteroid5, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 5:
                    Instantiate(asteroid6, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                case 6:
                    Instantiate(Satellite, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
                default:
                    Instantiate(Drone, new Vector3(x, 0, 18), Quaternion.identity);
                    break;
            }

            System.Random RandoCarissian = new System.Random();
            if (managerGameObject.getCurrentLevel() < 2) index = RandoCarissian.Next(0, 7);
            else if (managerGameObject.getCurrentLevel() < 3) index = RandoCarissian.Next(0, 8);
            else if (managerGameObject.getCurrentLevel() < 4) index = RandoCarissian.Next(0, 10);
            else index = RandoCarissian.Next(0, 12);

            recordedTime = Time.time;
            managerGameObject.setAsteroidSpeed(1.03f);
        }
    }
}
