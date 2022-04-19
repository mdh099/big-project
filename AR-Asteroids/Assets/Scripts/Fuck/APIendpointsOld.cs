using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

public class APIendpoints : MonoBehaviour
{
    /*#if UNITY_EDITOR
        string strDomain = "ivtsweepstakes.com";
        string MachineID = "5";
        string ID = "1023132136";
        int TheStoreID = 22;
        int gameId;
    #else
        static string strDomain;
        static string MachineID;
        static string ID;
        static int TheStoreID;
        static int gameId;
    #endif

        static int retries = 10, retrySleep = 10;*/

    int retries = 10;
    int retrySleep = 10;
    int ID = 0;
    int Score = -1;

    public struct LoginInfo
    {
        public string username;
        public int userID;
        public string password;
    };

    public struct UserInfo
    {
        public int userID;
        public string Error;
    };

    public struct ScoreRequest
    {
        public int storeID;
        public int userID;
    };

    public struct ScoreResponse
    {
        public string error;
        public int winnings;
        public int deferredWinnings;
    };


    public struct AddToScoreRequest
    {
        public int storeId, userId, amountToAdd;
    }


    public struct AddToScoreResponse
    {
        public string error;
    }






    public UserInfo DoLogin()
    {
        LoginInfo li = new LoginInfo();

        li.username = "Radir";
        li.password = "7f943921724d63dc0ac9c6febf99fa88";
        li.userID = 0;
        //if (Config.DEBUG_FULL) Debug.Log(ID);
        //I think we put the website API here?
        string strURL = string.Format("https://cop4331-123.herokuapp.com/api/gamelogin");
        string strJsonInput = JsonConvert.SerializeObject(li);

        Debug.Log(strJsonInput);

        UserInfo ui = new UserInfo();
        ui.Error = string.Empty;

        for (int i = 0; i < retries; i++)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                ui = JsonConvert.DeserializeObject<UserInfo>(strJsonResult);
                if (ui.Error.Length == 0)
                {
                    //TheStoreID = ui.StoreID;
                    return (ui);
                }
            }
            catch (System.Exception ex)
            {
                ui.Error = ex.Message.ToString();
            }
            Thread.Sleep(retrySleep);
        }

        //ErrorHandler.ErrorMessage(6, ui.Error, 1); // Removed because you cant send an error through the network when there is no network to send it through

        return (ui);
    }

    public void addToScore(int amount)
    {
        int userId = Convert.ToInt32(ID);
        int score;
        AddToScoreRequest req = new AddToScoreRequest();
        req.amountToAdd = amount;
        req.userId = userId;

        AddToScoreResponse res = new AddToScoreResponse();
        res.error = String.Empty;

        //I think we put the website API here?
        string strURL = String.Format("http://{0}/jsonservices/AddToWinnings4.aspx");
        string strJsonInput = JsonConvert.SerializeObject(req);

        WebClient wc = new WebClient();

        for (int i = 0; i < retries; i++)
        {
            try
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                //string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                wc.Headers[HttpRequestHeader.ContentType] = "text/plain";

                //string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                //string strJsonResult = Horseshoe.StringCipher.Decrypt((string)wc.UploadString(strURL, "POST", Horseshoe.StringCipher.Encrypt(strJsonInput)));
                string strJsonResult = wc.UploadString(strURL, "POST", strJsonInput); //I think this is what we need

                res = JsonConvert.DeserializeObject<AddToScoreResponse>(strJsonResult);
                if (res.error.Length == 0) return;
            }
            catch (System.Exception ex)
            {
            }
            Thread.Sleep(retrySleep);
        }

    }
}
