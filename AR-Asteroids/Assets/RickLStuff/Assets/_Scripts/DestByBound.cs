using UnityEngine;
using System.Collections;

public class DestByBound : MonoBehaviour
{

    private ManagerGame managerGameObject;
    public AudioSource sound;

    private void Awake()
    {
        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Asteroid")
        {
            managerGameObject.incrementNumberOfMisses();
            sound.Play();
        }

        Destroy(other.gameObject);
    }

}
