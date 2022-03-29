using UnityEngine;
using GameUtilities;

public static class ErrorHandler
{
    // Will contain reference to GameManager when it's created.
    public static GameObject GM = null;

    private static readonly string[] errorMsgs =
    {
        "Invalid entry encountered", //0
        "Function called while out of state", //1
        "Invalid input for function", //2
        "Invalid return format", //3
        "Function failed", //4
        "Login failed", //5
        "Connection problems", //6
        "Critical error", //7
        "Missing scene component", //8
        "Null when shouldn't be", //9
        "Problems with instantiation and starting configuration", //10
        "Logic Error", //11
        "Index exceeds length of stack", //12
        "Entry amount contradiction", //13
        "No Error Log File", //14
        "Too many invalid entries", //15
        "User has been logged out externally" //16
    };

    public static void RegisterGM(GameObject gm)
    {
        GM = gm;
    }

    private static void Act(int action)
    {
        // action: 0  -->  Do Nothing
        // action: 1  -->  HALT APPLICATION
        // action: 2  -->  Repeating attempts
        // action: 3  -->  Report suspicious event
        switch (action)
        {
            case 0:
                {
                    // Stay calm and carry on.
                    break;
                }
            case 1:
                {
                    Logout();
                    break;
                }
            case 2:
                {
                    // TODO: Figure out how to relay this
                    break;
                }
            case 3:
                {
                    // TODO: Notify server of sketchy behavior
                    break;
                }
        }
    }

    private static void Logout()
    {
        //GM.GetComponent<ManagerGame>().Logout();
    }

    public static void ErrorMessage(int msg, int value, int action)
    {
        if (value == int.MinValue)
        {
            ErrorManagement.SendError(errorMsgs[msg]);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
        else
        {
            ErrorManagement.SendError(errorMsgs[msg] + ": " + value);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + ": " + value + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
    }
    public static void ErrorMessage(int msg, string value, int action)
    {
        if (value == "")
        {
            ErrorManagement.SendError(errorMsgs[msg]);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
        else
        {
            ErrorManagement.SendError(errorMsgs[msg] + ": " + value);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + ": " + value + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
    }
    public static void ErrorMessage(int msg, string value, string ex, int action)
    {
        if (value == "")
        {
            ErrorManagement.SendError(errorMsgs[msg]);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
        else
        {
            ErrorManagement.SendError(errorMsgs[msg] + ": " + value + "\n" + ex);
            if (Config.DEBUG || Config.DEBUG_FULL) Debug.Log("ERROR: " + errorMsgs[msg] + ": " + value + "\n" + ex + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Act(action);
        }
    }
}
