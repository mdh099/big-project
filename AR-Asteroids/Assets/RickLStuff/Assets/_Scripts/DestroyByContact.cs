using UnityEngine;
using System.Collections;
using System;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion, playerExplosion;
    private ManagerHUD managerHudObject;
    private ManagerGame managerGameObject;

    void Start()
    {
        managerHudObject = GameObject.FindGameObjectWithTag("ManagerHUD").GetComponent<ManagerHUD>();
        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
    }

    void OnTriggerEnter(Collider other)
    {
        bool isCrash = false;

        if (other.tag.Equals("Boundary")) return;

        if (gameObject.tag == "Drone")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            managerHudObject.SetThisTurnsWinnings(managerHudObject.GetEntryLevel() * managerGameObject.getAstroidValue() * -5);
            if (other.gameObject.tag == "Bolt") Destroy(other.gameObject);
            else other.gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if (other.tag == "Player")
        {
            // If the Player is destoryed Set up the explosion, add the the players winnings the money they earned this round, reset the counter for the current round, set game state variables, turn on game over panel.
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            managerHudObject.setLifeLevelZero();
            managerGameObject.addToWinnings(managerHudObject.GetComponent<ManagerHUD>().GetThisTurnsWinnings());
            managerHudObject.playCashSound();
            managerGameObject.setGameIsInProgress(false);
            managerGameObject.setGameIsOver(true);
            managerHudObject.setGameOverPanelActive(true);
            managerHudObject.ExitButtonIsEnabled(true);
            //managerHudObject.EntryLVLDownButtonIsEnabled(true);
            //managerHudObject.EntryLVLUpButtonIsEnabled(true);
            managerHudObject.PlayButtonIsEnabled(true);
            managerHudObject.BuyTimeButtonIsEnabled(true);
            managerHudObject.TimePlusButtonIsEnabled(true);
            isCrash = true;
            other.gameObject.SetActive(false);
        }

        if (gameObject.tag == "Satellite")
        {
            //Instantiate(playerExplosion, other.transform.position, other.transform.rotation); // Removed for now but looks cool so we may put it back in.
            if (other.gameObject.tag == "Player") other.gameObject.SetActive(false); 
            else Destroy(other.gameObject);
        }

        if (gameObject.tag == "Asteroid")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            if (!isCrash) managerHudObject.SetThisTurnsWinnings(managerHudObject.GetEntryLevel() * managerGameObject.getAstroidValue());
            if (!isCrash) managerGameObject.incrementNumAsteroidsHit();
            if(other.gameObject.tag == "Bolt")Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
