using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Load : MonoBehaviour
{

    private Image loadBar;
    private bool loadScene = false;
    private int time;
    private int flag = 0;

    AsyncOperation async;

    // Use this for initialization
    void Start ()
    {
        loadBar = GameObject.FindGameObjectWithTag("LoadBar").GetComponent<Image>();
        loadBar.fillAmount = 0;

        time = (int)Time.deltaTime;
        try
        {
            loadBar.fillAmount = 0.1f;
            async = null;
        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorMessage(7, "SceneLoader-Start", ex.ToString(), 1);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!loadScene)
        {
            try
            {
                loadScene = true;
                StartCoroutine(LoadNewScene());
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "SceneLoader-UpdateLoad", ex.ToString(), 1);
            }
        }

        loadBar.fillAmount += .002f;

        if (loadBar.fillAmount >= 1f && flag == 0)
        {
            flag = 1;
            async.allowSceneActivation = true;
        }
    }

    IEnumerator LoadNewScene()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync("Main");
        async.allowSceneActivation = false;

        yield return null;
    }
}