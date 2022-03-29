using GameUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{

    private ManagerHUD ManagerHudObject; // Object that allows use of the HUD's methods and objects
    private GameObject NetworkIssuePanel;
    private GameObject Player;

    ConnectionDriver.UserInfo thisUser; // Stores UserID and store number
    ConnectionDriver.ResultInfo3 entriesAndGameInfo; // Used the store all the results from the server, including store setting and the active user's entries
    List<Tuple<int, ulong>> ResultsList = new List<Tuple<int, ulong>>(); // Stores all entries recieved from the server (entryID and Value)

    private bool gameIsInProgress = false; // Used to keep track of whether or not the ship should move and asteroids be spawned.
    private bool gameIsOver = false; // Used to determine if the Game Over object is displayed and if the level needs to be reloaded.
    private bool gameIsPaused = false;
    private bool isTutorialMode = false;
    private bool isWarping = false;

    private int currentLevel = 1;
    private int COST_PER_PLAY = 75;
    private double asteroidValue = .01; // Stores the base value of an asteroid before entry level is factored in
    private float asteroidSpeed = -3.5f;
    private float asteroidDelta = 5f;
    private int numAsteroidsHit = 0;
    private int numMissedObjects = 0;
    private int NUM_MISSES_ALLOWED = 3; // This is the number of misses permitted TODO Find a way to manipulate this automatically
    private int LEVEL_REQUIREMENT = 10; // This is the number of asteroids required to be hit in order to move to the next level
    private string[] arguments;

    private void Awake()
    {
        ManagerHudObject = GameObject.FindGameObjectWithTag("ManagerHUD").GetComponent<ManagerHUD>();
        NetworkIssuePanel = GameObject.FindGameObjectWithTag("NetworkIssuePanel");
        Player = GameObject.FindGameObjectWithTag("Player");
        NetworkIssuePanel.SetActive(false);
        arguments = Environment.GetCommandLineArgs();
        if (arguments.Length > 3) ConnectionDriver.SetUserInfo(arguments);
#if UNITY_EDITOR
#else
        else ConnectionIssueHandler(true);
#endif
    }

    // Use this for initialization
    void Start()
    {
        thisUser = ConnectionDriver.DoLogin();
        entriesAndGameInfo = ConnectionDriver.GetResults3(thisUser.StoreID, thisUser.ID);
        createEntriesList();

        UpdateHud();

        ManagerHudObject.SetEntryLevel(1);
        ManagerHudObject.ChangeBuyTime();
        //ManagerHudObject.ChangeEntryLevel(-1); User can no longer control entry level
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHud()
    {
        thisUser = ConnectionDriver.DoLogin();
        entriesAndGameInfo = ConnectionDriver.GetResults3(thisUser.StoreID, thisUser.ID);
        createEntriesList();

        ManagerHudObject.SetTotalWins(thisUser.Winnings / 100.0);
        ManagerHudObject.SetEntries(ResultsList.Count);
        ManagerHudObject.SetEntriesPerPlay(COST_PER_PLAY);
    }

    private void createEntriesList()
    {
        ResultsList.Clear();
        for (int i = 0; i < entriesAndGameInfo.Multiplier.Length; i++)
        {
            ResultsList.Add(Tuple.Create<int, ulong>(entriesAndGameInfo.Multiplier[i], entriesAndGameInfo.ID[i]));
        }
    }

    public void buyMoreTime(int purchaseAmount)
    {
        if (purchaseAmount == -1) purchaseAmount = (int)(ManagerHudObject.GetTotalWins() * 100);
        else purchaseAmount *= 100;
        ConnectionDriver.GetBoughtResultsResponse3 newEntries = ConnectionDriver.BuyResults3(thisUser.StoreID, thisUser.ID, (purchaseAmount));
        for (int i = 0; i < newEntries.Multiplier.Length; i++)
        {
            ResultsList.Add(Tuple.Create<int, ulong>(newEntries.Multiplier[i], newEntries.ID[i]));
        }
        UpdateHud();
        ManagerHudObject.PlayButtonIsEnabled(true);
        ManagerHudObject.outOfEntriesPanelIsActive(false);
    }

    // Game State Methods
    public void ResetTheGame()
    {
        Time.timeScale = 0;

        currentLevel = 1;
        ManagerHudObject.SetEntryLevel(1);
        gameIsInProgress = false;
        gameIsOver = false;
        gameIsPaused = false;
        isTutorialMode = false;
        numAsteroidsHit = 0;
        numMissedObjects = 0;
        asteroidSpeed = -3.5f;
        asteroidDelta = 5f;

        ManagerHudObject.setGameOverPanelActive(false);
        ManagerHudObject.setNewGamePanelActive(true);
        ManagerHudObject.setLifeLevelFull();
        ManagerHudObject.ResetThisTurnsWinnings();
        ManagerHudObject.setTutorialPanelActive(false);
        ManagerHudObject.setGameOverviewPanelActive(false);
        ManagerHudObject.setGameOverviewPanel2Active(false);
        ManagerHudObject.setWarpAnimationActive(false);
        ManagerHudObject.activateAllButtons();

        GameObject[] leftOvers = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in leftOvers)
        {
            Destroy(asteroid);
        }

        leftOvers = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject drone in leftOvers)
        {
            Destroy(drone);
        }

        leftOvers = GameObject.FindGameObjectsWithTag("Satellite");
        foreach (GameObject satellite in leftOvers)
        {
            Destroy(satellite);
        }

        Player.SetActive(true);
        Player.transform.position = new Vector3(0, 0, 2.5f);
    }

    public void levelUp()
    {
        currentLevel++;
        ManagerHudObject.setWarpAnimationActive(true);
        ManagerHudObject.setLevelTextPanelActive(true);
        ManagerHudObject.SetEntryLevel(currentLevel);
        setGameIsPaused(true);
        setGameIsInProgress(false);
        isWarping = true;

        GameObject[] leftOvers = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in leftOvers)
        {
            Destroy(asteroid);
        }

        leftOvers = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject drone in leftOvers)
        {
            Destroy(drone);
        }

        leftOvers = GameObject.FindGameObjectsWithTag("Satellite");
        foreach (GameObject satellite in leftOvers)
        {
            Destroy(satellite);
        }

        StartCoroutine(sleepFunction());

    }

    private IEnumerator sleepFunction()
    {
        yield return new WaitForSeconds(3);
        ManagerHudObject.setWarpAnimationActive(false);
        ManagerHudObject.setLevelTextPanelActive(false);
        setGameIsPaused(false);
        setGameIsInProgress(true);
        isWarping = false;
    }

    public bool getGameIsInProgress()
    {
        return gameIsInProgress;
    }

    public void setGameIsInProgress(bool state)
    {
        gameIsInProgress = state;
    }

    public bool getGameIsOver()
    {
        return gameIsOver;
    }

    public void setGameIsOver(bool state)
    {
        gameIsOver = state;
    }

    public bool getGameIsPaused()
    {
        return gameIsPaused;
    }

    public void setGameIsPaused(bool state)
    {
        gameIsPaused = state;
    }

    public bool getTutorialMode()
    {
        return isTutorialMode;
    }

    public void setTutorialMode(bool state)
    {
        isTutorialMode = state;
        ManagerHudObject.TutorialButtonIsEnabledbool(!state);
    }
    // End Game State Methods

    // Getters and Setters
    public int getCostPerPlay()
    {
        return COST_PER_PLAY;
    }

    public bool getIsWarping()
    {
        return isWarping;
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public float getAsteroidDelta()
    {
        return asteroidDelta;
    }

    public void incrementNumAsteroidsHit()
    {
        numAsteroidsHit++;
        if (numAsteroidsHit == LEVEL_REQUIREMENT * currentLevel)
        {
            levelUp();
        }
    }

    public int getNumAsteroidsHit()
    {
        return numAsteroidsHit;
    }

    public void resetNumAsteroidsHit()
    {
        numAsteroidsHit = 0;
    }

    public void addToWinnings(double winnings)
    {
        StartCoroutine(ManagerHudObject.WinsIncrementer(ManagerHudObject.GetTotalWins() + winnings));
        winnings *= 100;
        int entryLevel = ManagerHudObject.GetEntryLevel();
        short[] mults = new short[COST_PER_PLAY];
        ulong[] IDs = new ulong[COST_PER_PLAY];
        Debug.Log(ManagerHudObject.GetEntries());
        for (int i = 0; i < (COST_PER_PLAY); i++)
        {
            mults[i] = (short)ResultsList[0].Item1;
            IDs[i] = ResultsList[0].Item2;
            ResultsList.RemoveAt(0);
            Debug.Log(i);
        }
        short[] potmultsfake = {  };
        ulong[] potidsfake = {  };
        ConnectionDriver.SetResults3(thisUser.StoreID, thisUser.ID, mults, IDs, potmultsfake, potidsfake, false);
        ConnectionDriver.addToWinnings(Convert.ToInt32(winnings));
        UpdateHud();
    }

    public double getAstroidValue()
    {
        return asteroidValue;
    }

    public void setAsteroidValue(double value)
    {
        asteroidValue = value;
    }

    public float getAsteroidSpeed()
    {
        return asteroidSpeed;
    }

    public void setAsteroidSpeed(float multiplier)
    {
        asteroidSpeed *= multiplier;
    }

    public void incrementNumberOfMisses()
    {
        numMissedObjects++;
        ManagerHudObject.updateHealthIndicator(numMissedObjects);
        if (numMissedObjects >= NUM_MISSES_ALLOWED)
        {
            Time.timeScale = 0;
            setGameIsInProgress(false);
            setGameIsOver(true);
            addToWinnings(ManagerHudObject.GetComponent<ManagerHUD>().GetThisTurnsWinnings());
            ManagerHudObject.setGameOverPanelActive(true);
            ManagerHudObject.ExitButtonIsEnabled(true);
            //ManagerHudObject.EntryLVLDownButtonIsEnabled(true);
            //ManagerHudObject.EntryLVLUpButtonIsEnabled(true);
            ManagerHudObject.PlayButtonIsEnabled(true);
            ManagerHudObject.BuyTimeButtonIsEnabled(true);
            ManagerHudObject.TimePlusButtonIsEnabled(true);
        }
    }

    public void resetNumberOfMisses()
    {
        numMissedObjects = 0;
    }

    // End Getters and Setters

    // Triggers when not in unity editor and there is a connection issue
    // Activates a panel that prevents play and gives the user the option to
    // return to the main horseshoe menu
    public void ConnectionIssueHandler(bool isIssue)
    {
        if (isIssue)
        {
            NetworkIssuePanel.SetActive(true);
        }
        else
        {
            NetworkIssuePanel.SetActive(false);
        }
    }

    // Logs the user out and passes, one last time, user data back to server account.
    public void Logout()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ErrorHandler.ErrorMessage(7, "GameManager-IsNoLongerEligibile", ex.ToString(), 1);
        }
    }
}
