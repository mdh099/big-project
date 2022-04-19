using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

public class Login : MonoBehaviour
{
    int retries = 10;
    int retrySleep = 10;
    int ID = 0;
    int Score = -1;

    public struct LoginInfo
    {
        public string login;
        //public int userID;
        public string password;
    };

    public struct UserInfo
    {
        public int userID;
        public string error;
    };

    public struct StoreScore
    {
        public int score;
        public int userID;
        public string username;
    };

    public struct ScoreError
    {
        public string error;
    };







    public UserInfo DoLogin(string user, string password)
    {
        LoginInfo li = new LoginInfo();
        Debug.Log("----Inside DoLogin----");
        //Debug.Log(user + " " + password);

        MD5 pass = MD5.Create();
        byte[] bytes = Encoding.ASCII.GetBytes(password);
        byte[] hash = pass.ComputeHash(bytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        string hashed = sb.ToString();

        //Debug.Log("from database: 7f943921724d63dc0ac9c6febf99fa88");
        //Debug.Log("from code: " + hashed);

        li.login = user;
        li.password = hashed;
        //if (Config.DEBUG_FULL) Debug.Log(ID);
        //I think we put the website API here?
        string strURL = string.Format("https://cop4331-123.herokuapp.com/api/gamelogin");
        string strJsonInput = JsonConvert.SerializeObject(li);

        Debug.Log(strJsonInput);

        UserInfo ui = new UserInfo();
        ui.error = string.Empty;


        try
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
            ui = JsonConvert.DeserializeObject<UserInfo>(strJsonResult);
            //Debug.Log(ui.error);
            if (ui.error.Length == 0)
            {
                Debug.Log(ui.userID);
                return (ui);
            }
        }
        catch (System.Exception ex)
        {
            ui.error = ex.Message.ToString();
        }

        return (ui);
    }

        //ErrorHandler.ErrorMessage(6, ui.Error, 1); // Removed because you cant send an error through the network when there is no network to send it through


    public ScoreError addScore()
    {
        StoreScore ss = new StoreScore();

        ss.userID = 206;
        ss.username = "Radir";
        ss.score = 420;

        //I think we put the website API here?
        string strURL = String.Format("https://cop4331-123.herokuapp.com/api/addscore");
        string strJsonInput = JsonConvert.SerializeObject(ss);
        ScoreError se = new ScoreError();
        try
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
            se = JsonConvert.DeserializeObject<ScoreError>(strJsonResult);
            Debug.Log(se.error);
            if (se.error.Length == 0)
            {
                Debug.Log(se.error);
                return (se);
            }
        }
        catch (System.Exception ex)
        {
            se.error = ex.Message.ToString();
        }

        return(se);

    }
}

/*
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

    }*/
