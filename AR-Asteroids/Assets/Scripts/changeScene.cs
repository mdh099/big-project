using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
        Debug.Log("CLICKED!");
        SceneManager.LoadScene("SampleScene");
        Debug.Log("CHANGED SCENE!");
    }
}
