using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float speed;
    [SerializeField] private float rotationMaxSpeed;

    private Vector3 rotate;

    void Start()
    {
        rotate = calculateAstroidRotation();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, speed * Time.deltaTime);
        transform.Rotate(rotate);
    }

    public Vector3 calculateAstroidRotation()
    {
        float rotationMinSpeed = (0 - rotationMaxSpeed);
        float astroidX = Random.Range(rotationMinSpeed, rotationMaxSpeed);
        float astroidY = Random.Range(rotationMinSpeed, rotationMaxSpeed);
        float astroidZ = Random.Range(rotationMinSpeed, rotationMaxSpeed);

        Vector3 newRotation = new Vector3(astroidX, astroidY, astroidZ);
        return newRotation;
    }
}
