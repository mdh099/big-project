using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float speed;


    // Update is called once per frame
    void Update()
    {
        Vector3 astroidMovement = calculateAstroidMovement(playerObj);
        transform.position += astroidMovement;
    }

    public Vector3 calculateAstroidMovement(GameObject playerObj)
    {
        float astroidX = transform.position.x;
        float astroidY = transform.position.y;
        float astroidZ = transform.position.z;

        float playerX = playerObj.transform.position.x;
        float playerY = playerObj.transform.position.y;
        float playerZ = playerObj.transform.position.z;

        /*Figures out the direction the player is in for each plane and then
         *moves in that direction for each plane based of the value of speed
         */
        Vector3 newPostition = new Vector3(Mathf.Sign(playerX - astroidX) * speed,
                                           Mathf.Sign(playerY - astroidY) * speed,
                                           Mathf.Sign(playerZ - astroidZ) * speed);
        return newPostition;
    }
}
