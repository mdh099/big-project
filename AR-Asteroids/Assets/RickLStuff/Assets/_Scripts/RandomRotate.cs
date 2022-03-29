using UnityEngine;
using System.Collections;

public class RandomRotate : MonoBehaviour
{

    public float tumble;
    private float speed;
    private ManagerGame managerGameObject;

    void Start()
    {
        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
        speed = managerGameObject.getAsteroidSpeed();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * tumble;
		rb.velocity = transform.forward * speed;
	}

}
