using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject boltPrefab;
    public Transform boltSpawn;
    public float speed = 10;
    public float boltLifeTime = 15;

    private GameObject currentBolt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            shoot();
        }

    }

    private void shoot()
    {
        currentBolt = Instantiate(boltPrefab);

        //currentBolt.transform.position = boltSpawn.position;

        Vector3 rotation = currentBolt.transform.rotation.eulerAngles;
        currentBolt.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        currentBolt.GetComponent<Rigidbody>().AddForce(boltSpawn.forward * speed, ForceMode.Impulse);
        Invoke("expire", boltLifeTime);
    }

    private void expire()
    {
        Destroy(currentBolt);
    }
}
