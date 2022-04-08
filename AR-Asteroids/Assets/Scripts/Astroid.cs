using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float speed;
    [SerializeField] private float rotationMaxSpeed;

    private AstroidTemplates templates;

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bolt has collied with something");

        if (other.CompareTag("Bolt"))
        {
            if(CompareTag("Asteroid"))
            {
                Destroy(other.gameObject);
            
                int rand = Random.Range(0, templates.AstroidsMedium.Length);
                Instantiate(templates.AstroidsMedium[rand], transform.position, Quaternion.identity);

                Debug.Log("Bolt has destroied Large asteroid");
                Destroy(gameObject);
            }
            if (gameObject.CompareTag("AsteroidMed"))
            {
                Destroy(other.gameObject);

                int rand = Random.Range(0, templates.AstroidsSmall.Length);
                Instantiate(templates.AstroidsSmall[rand], transform.position, Quaternion.identity);

                Destroy(gameObject);
                Debug.Log("Bolt has destroied Medium asteroid");
            }
            if (gameObject.CompareTag("AsteroidSmall"))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
                Debug.Log("Bolt has destroied Small asteroid");
            }

            //Invoke("destroySelf", .1f);
        }
    }
}
