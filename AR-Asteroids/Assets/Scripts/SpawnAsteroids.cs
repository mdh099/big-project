using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    public float radius = 20.0f;
    public float yRange = 30.0f;
    float distanceX;
    float distanceY;
    float distanceZ;
    private Vector3 rotate;
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("spawn", 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 calculateAstroidRotation()
    {
        AstroidTemplates templates = FindObjectOfType<AstroidTemplates>();
        float rotationMinSpeed = (0 - templates.rotationMaxSpeed);
        float astroidX = Random.Range(rotationMinSpeed, templates.rotationMaxSpeed);
        float astroidY = Random.Range(rotationMinSpeed, templates.rotationMaxSpeed);
        float astroidZ = Random.Range(rotationMinSpeed, templates.rotationMaxSpeed);

        Vector3 newRotation = new Vector3(astroidX, astroidY, astroidZ);
        return newRotation;
    }

    private void spawn()
    {
        rotate = calculateAstroidRotation();
        AstroidTemplates templates = FindObjectOfType<AstroidTemplates>();
        int rand = Random.Range(0, templates.AstroidsLarge.Length);
        distanceY = Random.Range(-yRange, yRange);
        distanceX = Random.Range(-radius, radius);
        //Debug.Log("radius: " + radius);
        //Debug.Log("X: " + distanceX);
        //Debug.Log("Y: " + distanceY);

        distanceZ = Mathf.Sqrt((Mathf.Pow(radius, 2f) - Mathf.Pow(distanceX, 2f)));
        //Debug.Log("Z: " + distanceZ);

        int isNeg = 1;
        isNeg = Random.Range(0, 100)%2;
        //Debug.Log("Sign: " + isNeg);

        if (isNeg == 0)
        {
            distanceZ = 0 - distanceZ;
        }

        Vector3 newPostion = new Vector3(distanceX, distanceY, transform.position.z + distanceZ);

        GameObject newAsteroid = Instantiate(templates.AstroidsLarge[rand], newPostion, Quaternion.identity);
        Astroid newThing = newAsteroid.GetComponent<Astroid>();
        newThing.playerObj = playerObj;
        //newAsteroid.transform.Rotate(rotate);

        Invoke("spawn", 5.0f);
    }
}

