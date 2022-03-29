using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManagerButton : MonoBehaviour
{

    // Dynamically link in the HUD and Game controllers
    private ManagerHUD ManagerHudObject;
    private ManagerGame ManagerGameObject;
    public GameObject GameOverviewPanel3;

    private void Awake()
    {
        ManagerHudObject = GameObject.FindGameObjectWithTag("ManagerHUD").GetComponent<ManagerHUD>();
        ManagerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
    }

    public void EntryLevelUP()
    {
        ManagerHudObject.ChangeEntryLevel(1);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void EntryLevelDown()
    {
        ManagerHudObject.ChangeEntryLevel(-1);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void BuyTimePlus()
    {
        ManagerHudObject.ChangeBuyTime();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void BuyMoreTime()
    {
        ManagerGameObject.buyMoreTime(ManagerHudObject.GetBuyTime());
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Overview()
    {
        ManagerHudObject.setGameOverviewPanelActive(true);
        ManagerHudObject.setGameOverviewPanel2Active(false);
        GameOverviewPanel3.SetActive(false);
        Time.timeScale = 0;
    }

    public void backtoGame()
    {
        if (ManagerGameObject.getTutorialMode())
        {
            exitTutorialMode();
        }
        else
        {
            GameOverviewPanel3.SetActive(false);
            ManagerHudObject.setGameOverviewPanel2Active(false);
            ManagerHudObject.setGameOverviewPanelActive(false);
            ManagerHudObject.setTutorialPanelActive(false);
            ManagerGameObject.setGameIsPaused(false);
            Time.timeScale = 1;
        }
    }

    public void OverviewPage2()
    {
        ManagerHudObject.setGameOverviewPanel2Active(true);
        GameOverviewPanel3.SetActive(false);
    }

    public void OverviewPage3()
    {
        ManagerHudObject.setGameOverviewPanel2Active(true);
        GameOverviewPanel3.SetActive(true);
    }

    public void enterTutorialMode()
    {
        ManagerGameObject.setTutorialMode(true);
        ManagerHudObject.setTutorialPanelActive(true);
        ManagerHudObject.setGameOverviewPanelActive(false);
        ManagerHudObject.setNewGamePanelActive(false);
        Time.timeScale = 1;
    }

    public void exitTutorialMode()
    {
        Time.timeScale = 1;
        ManagerGameObject.ResetTheGame();
    }

    public void Play()
    {
        if (ManagerGameObject.getGameIsInProgress() == false && ManagerGameObject.getGameIsOver() == true) ManagerGameObject.ResetTheGame();
        else
        {
            Time.timeScale = 1;
            ManagerGameObject.setGameIsInProgress(true);
            ManagerHudObject.setNewGamePanelActive(false);
            ManagerHudObject.TutorialButtonIsEnabledbool(false);
            EventSystem.current.SetSelectedGameObject(null);
            //ManagerHudObject.setWarningPanelActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

}
