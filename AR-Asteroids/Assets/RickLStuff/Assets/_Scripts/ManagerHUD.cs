using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerHUD : MonoBehaviour {

    private Button Exit;
    private Button Overview;
    private Button BuyTime;
    //private Button Autoplay; There is not autoplay funtionality in AstroRun
    private Button Play; // In AstroRun this is actually the play button
    private Button UpEntryLVL;
    private Button DownEntryLVL;
    private Button PlusTime; // Cycles through the amount of time the user would purchase if they pressed the buy time button
    private Button TutuorialMode;
    private Button BackToGame;

    private ManagerGame managerGameObject;
    private ManagerHUD managerHudObject;

    private GameObject gameOverPanel; // Displays when the user has lost the current round
    private GameObject gameOverviewPanel; // Panel with information about how to play the game
    private GameObject gameOverviewPanel2; // Second Panel with information about how to play the game
    private GameObject OutOfEntriesPanel; // Panel that covers up play button when you are out of entries
    private GameObject TutorialPanel; // Contains information about how to fly the space ship, displays during tutuorial
    private GameObject newGamePanel; // Displays at the beginning of a new game with instructions for users on how to start
    private GameObject LevelTextPanel; // Warns the user that asteroids are incoming.
    private GameObject warpAnimation; // Shows during warp to another level.

    private Text TotalWins; // The amount of winnings the user currently has saved up
    private Text Entries; // Number of entries the user has available
    private Text Time; // The currently listed amount of time the user would purchase
    private Text EntryLevel; // The multiplier that will be applied to winnings
    private Text EntriesPerPlay; // Number of entries that will be used each time the user plays the game
    private Text ThisTurnsWinnings; // Amount the current user has won so far this round
    private Text LevelText;

    private GameObject LifeIndicator0;
    private GameObject LifeIndicator1;
    private GameObject LifeIndicator2;
    private GameObject LifeIndicator3;

    //Variables for storing the data values of the above text fields
    private int buyTimeAmount = 1; // Equals -1 when the ALL setting is selected
    private double totalWinnings = 0;
    private double thisTurnsWinnings = 0;
    private int entryLevel = 1;
    private int entries;

    private void Awake()
    {
        TotalWins = GameObject.FindGameObjectWithTag("TotalWins").GetComponent<Text>();
        Entries = GameObject.FindGameObjectWithTag("Entries").GetComponent<Text>();
        Time = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        EntryLevel = GameObject.FindGameObjectWithTag("EntryLevel").GetComponent<Text>();
        EntriesPerPlay = GameObject.FindGameObjectWithTag("EntriesPerPlay").GetComponent<Text>();
        ThisTurnsWinnings = GameObject.FindGameObjectWithTag("ThisTurnsWinnings").GetComponent<Text>();
        LevelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<Text>();

        LifeIndicator0 = GameObject.FindGameObjectWithTag("LifeIndicator0");
        LifeIndicator1 = GameObject.FindGameObjectWithTag("LifeIndicator1");
        LifeIndicator2 = GameObject.FindGameObjectWithTag("LifeIndicator2");
        LifeIndicator3 = GameObject.FindGameObjectWithTag("LifeIndicator3");

        Exit = GameObject.FindGameObjectWithTag("ButtonExit").GetComponent<Button>();
        Overview = GameObject.FindGameObjectWithTag("ButtonOverview").GetComponent<Button>();
        BuyTime = GameObject.FindGameObjectWithTag("ButtonBuyTime").GetComponent<Button>();
        //Autoplay = GameObject.FindGameObjectWithTag("ButtonAutoplay").GetComponent<Button>();
        Play = GameObject.FindGameObjectWithTag("ButtonPlay").GetComponent<Button>();
        //UpEntryLVL = GameObject.FindGameObjectWithTag("ButtonUpEntryLVL").GetComponent<Button>(); Removed for this game since users dont choose entry Level
        //DownEntryLVL = GameObject.FindGameObjectWithTag("ButtonDownEntryLVL").GetComponent<Button>(); Removed for this game since users dont choose entry level
        PlusTime = GameObject.FindGameObjectWithTag("ButtonPlusTime").GetComponent<Button>();
        TutuorialMode = GameObject.FindGameObjectWithTag("ButtonTutorialMode").GetComponent<Button>();
        BackToGame = GameObject.FindGameObjectWithTag("ButtonBackToGame").GetComponent<Button>();

        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
        managerHudObject = GameObject.FindGameObjectWithTag("ManagerHUD").GetComponent<ManagerHUD>();

        gameOverPanel = GameObject.FindGameObjectWithTag("GameOverPanel");
        gameOverviewPanel = GameObject.FindGameObjectWithTag("GameOverviewPanel");
        gameOverviewPanel2 = GameObject.FindGameObjectWithTag("GameOverviewPanel2");
        newGamePanel = GameObject.FindGameObjectWithTag("NewGamePanel");
        TutorialPanel = GameObject.FindGameObjectWithTag("TutorialPanel");
        warpAnimation = GameObject.FindGameObjectWithTag("warpAnimation");
        LevelTextPanel = GameObject.FindGameObjectWithTag("LevelTextPanel");
        OutOfEntriesPanel = GameObject.FindGameObjectWithTag("OutOfEntriesPanel");

        newGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameOverviewPanel.SetActive(false);
        gameOverviewPanel2.SetActive(false);
        TutorialPanel.SetActive(false);
        warpAnimation.SetActive(false);
        LevelTextPanel.SetActive(false);
        OutOfEntriesPanel.SetActive(false);
    }

    private void Update()
    {
        // THIS CODE IS NOT NECESSARY AT THE CURRENT MOMENT BECAUSE THE USER CANNOT CONTROL ENTRY LEVEL
        // SO THERE IS NO NEED TO VET THE CURRENT ENTRY LEVEL
        //int maxPossibleMultiplier = (entries - 5) / (GameUtilities.WinLines.NUMBER_OF_WIN_LINES);
        //if (maxPossibleMultiplier < 1) maxPossibleMultiplier = 1;
        //if (maxPossibleMultiplier <= entryLevel)
        //{
        //    SetEntryLevel(maxPossibleMultiplier);
        //    EntryLVLUpButtonIsEnabled(false);
        //}
        //else if(entryLevel != 10) EntryLVLUpButtonIsEnabled(true);

        if(managerGameObject.getGameIsInProgress() || managerGameObject.getTutorialMode())
        {
            //managerHudObject.EntryLVLDownButtonIsEnabled(false); Button is inactive by default now
            //managerHudObject.EntryLVLUpButtonIsEnabled(false); Button is inactive by default now
            TimePlusButtonIsEnabled(false);
            BuyTimeButtonIsEnabled(false);
            ExitButtonIsEnabled(false);
            PlayButtonIsEnabled(false);
        }

        if(entries < managerGameObject.getCostPerPlay())
        {
            PlayButtonIsEnabled(false);
            OutOfEntriesPanel.SetActive(true);
        }
        else if(!managerGameObject.getGameIsInProgress())
        {
            
        }
    }


    // Getter and Setter Methods
    public void SetTotalWins(double winnings)
    {
        totalWinnings = winnings;
        TotalWins.text = "$" + string.Format("{0:0.00}", winnings);
    }

    private void setTotalWinColor(int r, int g, int b)
    {
        TotalWins.color = new Color(r, g, b);
    }

    public IEnumerator WinsIncrementer(double newTotalWins)
    {
        double oldTotalWins = managerHudObject.GetTotalWins();
        setTotalWinColor(0, 1, 0);
        while (oldTotalWins < newTotalWins)
        {
            oldTotalWins += .01;
            UpdateTotalWins(oldTotalWins);
            if (newTotalWins - oldTotalWins < .25) yield return new WaitForSeconds(.08f);
            else yield return null;
        }
        setTotalWinColor(1, 1, 1);
        yield return null;
    }

    public void UpdateTotalWins(double winnings)
    {
        totalWinnings = winnings;
        TotalWins.text = "$" + string.Format("{0:0.00}", winnings);
    }

    public double GetTotalWins()
    {
        return totalWinnings;
    }

    public void SetEntries(int entries)
    {
        this.entries = entries;
        Entries.text = Convert.ToString(entries);
    }

    public int GetEntries()
    {
        return entries;
    }

    public void ChangeBuyTime()
    {
        //TODO with switch
        //Check if user has the available money
        switch (buyTimeAmount)
        {
            case 1:
                if (totalWinnings >= 5.00) buyTimeAmount = 5;
                else buyTimeAmount = -1;
                break;
            case 5:
                if (totalWinnings >= 10.00) buyTimeAmount = 10;
                else buyTimeAmount = -1;
                break;
            case 10:
                if (totalWinnings >= 25.00) buyTimeAmount = 25;
                else buyTimeAmount = -1;
                break;
            case 25:
                buyTimeAmount = -1;
                break;
            case -1:
                if (totalWinnings >= 1.00) buyTimeAmount = 1;
                else buyTimeAmount = -1;
                break;
        }//end switch

        Time.text = (buyTimeAmount > 0) ? (buyTimeAmount + " min") : "All";

    }

    public void SetBuyTime(int time)
    {
        if (time == -1) Time.text = "All";
        else Time.text = Convert.ToString(time) + " min";
        buyTimeAmount = time;
    }

    public int GetBuyTime()
    {
        return buyTimeAmount; // Returns -1 if "ALL"
    }

    public void ChangeEntryLevel(int direction)
    {
        if (direction == 1)
        {

            if (entryLevel < 10) SetEntryLevel(entryLevel + 1);
        }

        if(direction == -1)
        {
            if (entryLevel > 1) SetEntryLevel(entryLevel - 1);
        }
    }

    public void SetEntryLevel(int entryLevel)
    {
        this.entryLevel = entryLevel;
        EntryLevel.text = Convert.ToString(entryLevel);
    }

    public int GetEntryLevel()
    {
        return entryLevel;
    }

    public void SetEntriesPerPlay(int entryAmount)
    {
        EntriesPerPlay.text = Convert.ToString(entryAmount);
    }

    public int GetEntriesPerPlay()
    {
        return Convert.ToInt32(EntriesPerPlay.text);
    }

    public void SetThisTurnsWinnings(double tempWinnings)
    {
        thisTurnsWinnings += tempWinnings;
        if (thisTurnsWinnings < 0) thisTurnsWinnings = 0;
        ThisTurnsWinnings.text = "$" + string.Format("{0:0.00}", thisTurnsWinnings);
    }

    public void ResetThisTurnsWinnings()
    {
        thisTurnsWinnings = 0.0f;
        ThisTurnsWinnings.text = "$" + string.Format("{0:0.00}", Convert.ToString(thisTurnsWinnings));
    }

    public double GetThisTurnsWinnings()
    {
        return thisTurnsWinnings;
    }
// End Getter and Setter Methods

// Button State Methods
    public void activateAllButtons()
    {
        Exit.interactable = true;
        Overview.interactable = true;
        BuyTime.interactable = true;
        Play.interactable = true;
        //DownEntryLVL.interactable = true;
        //UpEntryLVL.interactable = true;
        PlusTime.interactable = true;
        TutuorialMode.interactable = true;
    }
    public void ExitButtonIsEnabled(bool active)
    {
        Exit.interactable = active;
    }

    public void OverviewButtonIsEnabled(bool active)
    {
        Overview.interactable = active;
    }

    public void BuyTimeButtonIsEnabled(bool active)
    {
        BuyTime.interactable = active;
    }

    //public void AutoplayButtonIsEnabled(bool active)
    //{
    //    Autoplay.interactable = active;
    //}

    public void PlayButtonIsEnabled(bool active)
    {
        Play.interactable = active;
    }

    public void EntryLVLUpButtonIsEnabled(bool active)
    {
        UpEntryLVL.interactable = active;
    }

    public void EntryLVLDownButtonIsEnabled(bool active)
    {
        DownEntryLVL.interactable = active;
    }

    public void TimePlusButtonIsEnabled(bool active)
    {
        PlusTime.interactable = active;
    }

    public void TutorialButtonIsEnabledbool(bool active)
    {
        TutuorialMode.interactable = active;
    }

    public void playCashSound()
    {
        TotalWins.GetComponentInParent<AudioSource>().Play(); ;
    }

    // End Button State Methods

    // Game State Methods
    public void setLifeLevelZero()
    {
        LifeIndicator1.SetActive(false);
        LifeIndicator2.SetActive(false);
        LifeIndicator3.SetActive(false);
    }

    public void setLifeLevelFull()
    {
        LifeIndicator1.SetActive(true);
        LifeIndicator2.SetActive(true);
        LifeIndicator3.SetActive(true);
    }

    public void setWarpAnimationActive(bool state)
    {
        warpAnimation.SetActive(state);
    }

    public void setGameOverviewPanelActive(bool state)
    {
        gameOverviewPanel.SetActive(state);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setGameOverviewPanel2Active(bool state)
    {
        gameOverviewPanel2.SetActive(state);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setGameOverPanelActive(bool state)
    {
        gameOverPanel.SetActive(state);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void outOfEntriesPanelIsActive(bool state)
    {
        OutOfEntriesPanel.SetActive(state);
    }

    public void setNewGamePanelActive(bool state)
    {
        newGamePanel.SetActive(state);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setTutorialPanelActive(bool state)
    {
        TutorialPanel.SetActive(state);
    }

    public void setLevelTextPanelActive(bool state)
    {
        LevelText.text = "Moving to\nSector " + Convert.ToString(managerGameObject.getCurrentLevel());

        LevelTextPanel.SetActive(state);
    }

    public void updateHealthIndicator(int numMissedObjects)
    {
        if(numMissedObjects == 1)
        {
            LifeIndicator3.SetActive(false);
        }
        if(numMissedObjects == 2)
        {
            LifeIndicator2.SetActive(false);
        }
        if(numMissedObjects == 3)
        {
            LifeIndicator1.SetActive(false);
        }
    }
    // End Game State Methods

}
