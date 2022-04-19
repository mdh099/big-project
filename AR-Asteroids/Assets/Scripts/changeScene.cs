using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class changeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToGameScene()
    {
        Login.UserInfo ui = new Login.UserInfo();

        string username = GameObject.FindGameObjectWithTag("User").GetComponent<Text>().text;
        string password = GameObject.FindGameObjectWithTag("Pass").GetComponent<Text>().text;
        Login login = GameObject.FindGameObjectWithTag("Login").GetComponent<Login>();

        Debug.Log("CLICKED!");
        Debug.Log(username + " " + password);

        ui = login.DoLogin(username, password);

        if (ui.error.Length == 0)
        {
            PlayerPrefs.SetInt("userID", ui.userID);
            PlayerPrefs.SetString("username", username);
            goToActualGame();
        }
        else
        {
            Debug.Log(ui.error);
        }


    }

    public void goToActualGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
