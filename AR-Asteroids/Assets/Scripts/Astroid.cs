using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] public GameObject playerObj;
    [SerializeField] private float speed;
    [SerializeField] private float rotationMaxSpeed;

    private AstroidTemplates templates;

    private Vector3 rotate;

    void Start()
    {
        rotate = calculateAstroidRotation();
        templates = FindObjectOfType<AstroidTemplates>();
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

    public void spawnAsteroids()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bolt has collied with something");

        if (other.CompareTag("Bolt"))
        {
            if(CompareTag("Asteroid"))
            {
                //Destroy bolt and get ran
                Destroy(other.gameObject);
                int rand = Random.Range(0, templates.AstroidsMedium.Length);
                float randx = Random.Range(1.0f, 2.0f);
                float randy = Random.Range(-0.5f, 0.5f);
                float randz = Random.Range(1.0f, 2.0f);

                //Spawn the first new asteroid
                Vector3 newPostion = new Vector3(transform.position.x + randx, transform.position.y + randy, transform.position.z + randz);
                GameObject newAsteroid = Instantiate(templates.AstroidsMedium[rand], newPostion, Quaternion.identity);
                Astroid newThing = newAsteroid.GetComponent<Astroid>();
                newThing.playerObj = playerObj;

                //Spawn the second new asteroid
                newPostion = new Vector3(transform.position.x - randx, transform.position.y + randy, transform.position.z - randz);
                newAsteroid = Instantiate(templates.AstroidsMedium[rand], newPostion, Quaternion.identity);
                newThing = newAsteroid.GetComponent<Astroid>();
                newThing.playerObj = playerObj;

                //Destroy asteroid
                Debug.Log("Bolt has destroied Large asteroid");
                Destroy(gameObject);
            }
            if (gameObject.CompareTag("AsteroidMed"))
            {
                //Destroy bolt and get ran
                Destroy(other.gameObject);
                int rand = Random.Range(0, templates.AstroidsSmall.Length);
                float randx = Random.Range(1.0f, 2.0f);
                float randy = Random.Range(-0.5f, 0.5f);
                float randz = Random.Range(1.0f, 2.0f);

                //Spawn the first new asteroid

                Vector3 newPostion = new Vector3(transform.position.x + randx, transform.position.y + randy, transform.position.z + randz);
                GameObject newAsteroid = Instantiate(templates.AstroidsSmall[rand], newPostion, Quaternion.identity);
                Astroid newThing = newAsteroid.GetComponent<Astroid>();
                newThing.playerObj = playerObj;

                //Spawn the second new asteroid
                newPostion = new Vector3(transform.position.x - randx, transform.position.y + randy, transform.position.z - randz);
                newAsteroid = Instantiate(templates.AstroidsSmall[rand], newPostion, Quaternion.identity);
                newThing = newAsteroid.GetComponent<Astroid>();
                newThing.playerObj = playerObj;

                //Destroy asteroid
                Destroy(gameObject);
                Debug.Log("Bolt has destroied Medium asteroid");
            }
            if (gameObject.CompareTag("AsteroidSmall"))
            {
                //Destroy everything
                Destroy(other.gameObject);
                Destroy(gameObject);
                Debug.Log("Bolt has destroied Small asteroid");
            }

            //Invoke("destroySelf", .1f);
        }
    }
}

/* Notes
    // Get direction from A to B
    Vector3 posA = ObjectA.position;
    Vector3 posB = ObjectB.position;
    //Destination - Origin
    Vector3 dir = (posB - posA).normalized;
 */
