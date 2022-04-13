using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bolt;
    public Camera fpsCam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bolt, transform.position, Quaternion.LookRotation(fpsCam.transform.forward));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Astroid collided with player");
        if (other.CompareTag("Asteroid"))
        {
            Destroy(other);
            Debug.Log("Astroid collided with player and triggered death");
        }
    }
}
