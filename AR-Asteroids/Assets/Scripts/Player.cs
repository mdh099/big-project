using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

using System.Text;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    //public GameObject bolt;
    //public Camera fpsCam;

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    // Start is called before the first frame update
    void Start()
    {
        //myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/scenes");
        //scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    public string sceneName = "";

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Astroid collided with player");
        if (other.CompareTag("Asteroid"))
        {
            Destroy(other);
            Debug.Log("Astroid collided with player and triggered death");
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
