using UnityEngine;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Threading;

// How the game communicates with the local server
public static class ConnectionDriver
{


#if UNITY_EDITOR
    static string strDomain = "ivtsweepstakes.com";
    static string MachineID = "5";
    static string ID = "1023132136";
    static int TheStoreID = 22;
    static int gameId;
#else
    static string strDomain;
    static string MachineID;
    static string ID;
    static int TheStoreID;
    static int gameId;
#endif

    static int retries = 10, retrySleep = 10;

    public static string GetDomain()
    {
        return strDomain;
    }

    public static int GetStoreId()
    {
        return TheStoreID;
    }

    public static void SetUserInfo(string[] arguments)
    {
        ID = arguments[1];
        MachineID = arguments[2];
        strDomain = arguments[3];
        TheStoreID = Int32.Parse(arguments[4]);
        gameId = Convert.ToInt32(arguments[5]);
    }

    public struct LoginInfo
    {
        public string ID;
        public int StoreID;
    };

    public struct UserInfo
    {
        public int ID;
        public int StoreID;
        public string Name;
        public int Winnings;
        public string Error;
        public bool isJackpotContributable;
        public int jackpotContributionEntryAmount;
    };

    public struct ResultRequestInfo3
    {
        public int StoreID;
        public int UserID;
    }

    public struct ResultInfo3
    {
        public Int16[] Multiplier;
        public UInt64[] ID;
        public bool IsSpinSkillGame; // In some states, every spin needs a test of skill. True toggles game on, False off.
        public bool IsEndSkillGame; // In some states, end of every game needs a test of skill. True toggles game on, False off.
        public bool IsPrerevealActive; // In some state, preveal is required.
        public bool IsAlwaysWinSomething; // In some states, every spin needs to win at least a penny. True toggles penny-win-on-loss on, False off.
        public int FreeSpinMultiplier; // For every entry that contains free spins, this is the max winnable ammount.
        public double CostPerLine; // Every store charges a different price per line.
        public double CostAutoWin; // Every store charges a different auto-win amount for states that require it.
        public double Cost5MinBuy; // Every store charges an amount to purchase 5 minutes of internet time.
        public int Entries5MinBuy;
        public string Error;
    }

    public struct GetBoughtResultsRequest3
    {
        public int StoreID;
        public int UserID;
        public int WinningsToSpend;
    }

    public struct GetBoughtResultsResponse3
    {
        public string Error;
        public int[] Multiplier;
        public UInt64[] ID;
        public int Winnings;
    }

    public struct GetFreeResultsRequest3
    {
        public int StoreID;
        public int UserID;
        public int RequestCount;
    }

    public struct GetFreeResultsResponse3
    {
        public string Error;
        public int[] Multiplier;
        public UInt64[] ID;
    }

    public struct CheckResultRequestInfo3
    {
        public int StoreID;
        public int UserID;
        public Int16[] Multiplier;
        public UInt64[] ID;
    }

    public struct CheckResultInfo3
    {
        public bool IsOK;
        public string Error;
    }

    public struct RegisterBadEntriesRequest3
    {
        public int StoreID;
        public int UserID;
        public UInt64[] IDs;
    }

    public struct RegisterBadEntriesResponse3
    {
        public int[] Multiplier;
        public UInt64[] ID;
        public string Error;
    }

    public struct SetResultRequestInfo3
    {
        public int StoreID;
        public int UserID;
        public Int16[] Multiplier;
        public UInt64[] ID;
        public bool FailedSkillGame;
        public Int16[] PotMultiplier;
        public UInt64[] PotID;
        public int GameId;
    }

    public struct SetResultInfo3
    {
        public int Winnings;
        public string Error;
    }

    public struct SetMachineResponse
    {
        public int storeID, machineID, status;
        public string error;
    }

    public struct StatusInfo
    {
        public int UserID, StoreID;
        public string Name;
        public int Status;
        public DateTime LastUpdated;
        public bool didBetAtLeastADollar;
        public string Error;
    }

    public struct StatusRequest
    {
        public int userID, storeID;
    }

    public struct StatusResponse
    {
        public bool isLoggedIn;
        public string error;
    }

    public struct ErrorInfo
    {
        public int StoreID;
        public string GameName;
        public string[] ErrorList;
    }

    public struct ErrorRet
    {
        public string Error;
    }

    public struct GetPotRequest
    {
        public int StoreID;
    }

    public struct GetPotResponse
    {
        public double SmallPot;
        public double MediumPot;
        public double LargePot;
        public double HotSeat;
        public bool NoResultsReturned;
        public string Error;
    }

    public struct WinningsRequest
    {
        public int storeID;
        public int userID;
    };

    public struct WinningsResponse
    {
        public string error;
        public int winnings;
        public int deferredWinnings;
    };

 
    public struct AddToWinningsRequest
    {
        public int storeId, userId, amountToAdd;
    }


    public struct AddToWinningsResponse
    {
        public string error;
    }

    public struct AccessStoredResultsRequest
    {
        public int storeId;
        public short[] Multiplier;
        public int requestCount;
    }

    public struct AccessStoredResultsResponse
    {
        public short[] Multiplier;
        public int returned;
        public string error;
    }

    public struct GetCommunityPotsRequest
    {
        public int StoreID;
        public bool IsGameMachine;
    }

    public struct GetCommunityPotsResponse
    {
        public DateTime DateCreated, FireDate, Current;
        public TimeSpan RemainingTime;
        public double Amount, RealAmount;
        public int Level;
        public string Name;//gold, bronze..
        public bool Fired;
        public int ID;
        public string Error;
        public int WinnerID;
        public string WinnerName;//single name or how many winners
        public string eligibleIDs;
    }

    public static GetCommunityPotsResponse GetCommunityPotsResponseFromServer2()
    {
        string domain = strDomain;
        int storeId = TheStoreID;
        GetCommunityPotsRequest gpreq = new GetCommunityPotsRequest();
        GetCommunityPotsResponse gpres = new GetCommunityPotsResponse();
        gpres.Error = String.Empty;
        gpreq.StoreID = storeId;
        gpreq.IsGameMachine = true;

        string strURL = string.Format("http://{0}/JSONServices/GetCommunityPotEvents.aspx", domain);
        string strJsonInput = JsonConvert.SerializeObject(gpreq);

        WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
		{
			try
			{
				wc.Headers[HttpRequestHeader.ContentType] = "application/json";
				string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
				gpres = JsonConvert.DeserializeObject<GetCommunityPotsResponse>(strJsonResult);
				return (gpres);
			}
			catch (System.Exception ex)
			{
				gpres.Error = ex.Message.ToString();
			}

			Thread.Sleep(retrySleep);
		}

        return (gpres);
    }

    public static void storeResults(short[] Multiplier)
    {
        AccessStoredResultsRequest req = new AccessStoredResultsRequest();
        req.storeId = TheStoreID;
        req.requestCount = 0;
        req.Multiplier = Multiplier;

        AccessStoredResultsResponse res = new AccessStoredResultsResponse();
        res.error = String.Empty;

        string strURL = String.Format("http://{0}/jsonservices/AccessStoredResults.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(req);

		WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
		{
			try
			{
				wc.Headers[HttpRequestHeader.ContentType] = "application/json";
				string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
				res = JsonConvert.DeserializeObject<AccessStoredResultsResponse>(strJsonResult);
				return;
			}
			catch (System.Exception ex)
			{
			}
			Thread.Sleep(retrySleep);
		}
    }

    public static short[] getStoredResults(int requestCount)
    {
        AccessStoredResultsRequest req = new AccessStoredResultsRequest();
        req.storeId = TheStoreID;
        req.requestCount = requestCount;
        req.Multiplier = new short[0];

        AccessStoredResultsResponse res = new AccessStoredResultsResponse();
        res.error = String.Empty;

        string strURL = String.Format("http://{0}/jsonservices/AccessStoredResults.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(req);

		WebClient wc = new WebClient();
		for (int i = 0; i < retries; i++)
		{
			try
			{
				wc.Headers[HttpRequestHeader.ContentType] = "application/json";
				string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
				res = JsonConvert.DeserializeObject<AccessStoredResultsResponse>(strJsonResult);
				break;
			}
			catch (System.Exception ex)
			{
				Thread.Sleep(retrySleep);
			}
		}

        Debug.Log("Before rando: " + res.returned);

        System.Random randoCalrissian = new System.Random();
        int tempRand = 0, lastWin = -1;
        //if not enough results returned
        for (int i = res.returned; i < requestCount; i++)
        {
            if (lastWin == (-1) || (i - lastWin) > 60) //if its the first added or if the last added mult was over 4 entries ago then add a win
            {
                tempRand = randoCalrissian.Next(1, 6);
                switch (tempRand)
                {
                    case 1:
                        res.Multiplier[i] = 5;
                        break;
                    case 2:
                        res.Multiplier[i] = 10;
                        break;
                    case 3:
                        res.Multiplier[i] = 20;
                        break;
                    case 4:
                        res.Multiplier[i] = 50;
                        break;
                    case 5:
                        res.Multiplier[i] = 100;
                        break;
                }
                lastWin = i;
            }
            else
            {
                res.Multiplier[i] = 0;
            }
            res.returned++;
        }
        Debug.Log("After rando: " + res.returned);

        string debug = "";
        for (int j = 0; j < res.returned; j++)
        {
            debug += "--" + res.Multiplier[j] + "--";
        }
        Debug.Log(debug);
        return (res.Multiplier);
    }


    public static WinningsResponse RetrieveWinnings()
    {
        WinningsRequest req = new WinningsRequest();
        WinningsResponse res = new WinningsResponse();
        res.error = string.Empty;
        req.storeID = TheStoreID;

		for (int i = 0; i < retries; i++)
        {
            try
            {
                req.userID = Int32.Parse(ID);

                string strURL = string.Format("http://{0}/JSONServices/GetWinnings.aspx", strDomain);
                string strJsonInput = JsonConvert.SerializeObject(req);

                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                res = JsonConvert.DeserializeObject<WinningsResponse>(strJsonResult);
                return res;
            }
            catch (System.Exception ex)
            {
                res.error = ex.Message.ToString();
				Thread.Sleep(retrySleep);
            }
        }

        ErrorHandler.ErrorMessage(29, "ConnectionDriver-RetrieveWinnings", res.error, 1);

        return res;
    }

    public static GetPotResponse GetJackpotInfo()
    {
        GetPotRequest gpr = new GetPotRequest();
        gpr.StoreID = TheStoreID;
        GetPotResponse gprs = new GetPotResponse();
        gprs.Error = string.Empty;

        string strURL = String.Format("http://{0}/JSONServices/GetPots.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(gpr);

        WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
        {
            try
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                gprs = JsonConvert.DeserializeObject<GetPotResponse>(strJsonResult);

                return gprs;
            }
            catch (System.Exception ex)
            {
                gprs.Error = ex.Message.ToString();
				Thread.Sleep(retrySleep);
            }
        }
        ErrorHandler.ErrorMessage(16, "ConnectionDriver-GetJackpotInfo", gprs.Error, 0);
        return gprs;
    }

    public static StatusResponse CheckLoginStatus(int uId)
    {
        StatusRequest rri = new StatusRequest();
        rri.storeID = TheStoreID;
        rri.userID = uId;

        StatusResponse ri = new StatusResponse();
        ri.error = String.Empty;

        // 1. Pull the store data for this store ID
        string strURL = string.Format("http://{0}/JSONServices/CheckLoginStatus.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

		for (int i = 0; i < retries; i++)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                ri = JsonConvert.DeserializeObject<StatusResponse>(strJsonResult);
                return ri;
            }
            catch (System.Exception ex)
            {
                ri.error = ex.Message.ToString();
				Thread.Sleep(retrySleep);
            }
        }
        ErrorHandler.ErrorMessage(16, "ConnectionDriver-CheckLoginStatus", ri.error, 1);
        return ri;
    }

    public static void SendError(int StoreID, string[] errors)
    {
        return;
#if CRAP
        ErrorInfo ei = new ErrorInfo();
        ei.StoreID = StoreID;
        ei.GameName = Config.GAME_NAME;
        ei.ErrorList = errors;

        string strURL = string.Format("http://{0}/jsonservices/senderrors.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(ei);

        ErrorRet re = new ErrorRet();
        re.Error = string.Empty;

        try
        {
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
            re = JsonConvert.DeserializeObject<ErrorRet>(strJsonResult);
            if (re.Error != string.Empty)
            {
                ErrorHandler.ErrorMessage(6, "ConnectionDriver-SendError", re.Error, 0);
            }
        }
        catch (System.Exception ex)
        {
            re.Error = ex.Message.ToString();
        }
#endif
    }

    public static UserInfo DoLogin()
    {
        LoginInfo li = new LoginInfo();
        li.ID = ID;
        li.StoreID = TheStoreID;
        if (Config.DEBUG_FULL) Debug.Log(ID);
        string strURL = string.Format("http://{0}/jsonservices/userlogin.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(li);

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
                if (ui.Error.Length == 0 )
                {
					TheStoreID = ui.StoreID;
                    return (ui);
                }
            }
            catch (System.Exception ex)
            {
                ui.Error = ex.Message.ToString();
            }
			Thread.Sleep(retrySleep);
        }

        ErrorHandler.ErrorMessage(6, ui.Error, 1);

        return (ui);
    }

    public static void SetJackpotEligibility(int userID, string name, int storeID, int status, bool isMinDollarBet)
    {
        string strURL = String.Format("http://{0}/jsonservices/setusereligibility.aspx?StoreID=" + storeID, strDomain);
        StatusInfo si = new StatusInfo();
        si.Error = string.Empty;

        si.StoreID = storeID;
        si.UserID = userID;
        si.Name = name;
        si.Status = status;
        si.didBetAtLeastADollar = isMinDollarBet;

        string strJsonInput = JsonConvert.SerializeObject(si);
        WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
        {
            try
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                si = JsonConvert.DeserializeObject<StatusInfo>(strJsonResult);
                if (si.Error.Length == 0 )
                {
					return;
                }
            }
            catch (System.Exception ex)
            {
                si.Error = ex.Message.ToString();
				Thread.Sleep(retrySleep);
            }
        }

        ErrorHandler.ErrorMessage(6, si.Error, 0);
    }

    public static void SetMachineState(int StoreID, int Status)
    {
        string strURL = String.Format("http://{0}/JSONServices/SetMachineState.aspx?StoreID=" + StoreID + "&MachineID=" + MachineID + "&Status=" + Status, strDomain);

        SetMachineResponse smr = new SetMachineResponse();
        smr.error = string.Empty;
		for (int i = 0; i < retries; i++)
        {

            try
            {
                WebClient wc = new WebClient();
                string strJsonResult = wc.DownloadString(strURL);
                smr = JsonConvert.DeserializeObject<SetMachineResponse>(strJsonResult);
                if (smr.error.Length == 0 )
                {
					return;
                }
            }
            catch (System.Exception ex)
            {
                smr.error = ex.Message.ToString();
				Thread.Sleep(retrySleep);
            }
        }

        ErrorHandler.ErrorMessage(6, smr.error, 0);
    }

    public static ResultInfo3 GetResults3(int StoreID, int UserID)
    {
        ResultInfo3 ri = new ResultInfo3();
        ri.Error = string.Empty;

        ResultRequestInfo3 rri = new ResultRequestInfo3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;

        // 1. Pull the store data for this store ID
        //string strURL = string.Format("http://{0}/JSONServices/GetResults3.aspx", strDomain);
        string strURL = string.Format("http://{0}/JSONServices/GetResults4.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

        WebClient wc = new WebClient();
		for (int i = 0; i < retries; i++)
        {
            try
            {
                //wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.ContentType] = "text/plain";

                //string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                string strJsonResult = Horseshoe.StringCipher.Decrypt((string)wc.UploadString(strURL, "POST", Horseshoe.StringCipher.Encrypt(strJsonInput)));

                ri = JsonConvert.DeserializeObject<ResultInfo3>(strJsonResult);
                if (ri.Error.Length == 0 )
                {
					return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
			Thread.Sleep(retrySleep);
        }

        ErrorHandler.ErrorMessage(6, ri.Error, 1);
        return (ri);
    }

    public static GetBoughtResultsResponse3 BuyResults3(int StoreID, int UserID, int WinningsToSpend)
    {
        GetBoughtResultsResponse3 ri = new GetBoughtResultsResponse3();
        ri.Error = string.Empty;

        GetBoughtResultsRequest3 rri = new GetBoughtResultsRequest3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;
        rri.WinningsToSpend = WinningsToSpend;

        // 1. Pull the store data for this store ID
        string strURL = string.Format("http://{0}/JSONServices/BuyResults3.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

		for (int i = 0; i < retries; i++)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                ri = JsonConvert.DeserializeObject<GetBoughtResultsResponse3>(strJsonResult);
                if (ri.Error.Length == 0 )
                {
					return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
            Thread.Sleep(retrySleep);
        }

		return (ri);
    }

    public static GetFreeResultsResponse3 GetFreeResults3(int StoreID, int UserID, int RequestCount)
    {
        GetFreeResultsResponse3 ri = new GetFreeResultsResponse3();
        ri.Error = string.Empty;

        GetFreeResultsRequest3 rri = new GetFreeResultsRequest3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;
        rri.RequestCount = RequestCount;

        // 1. Pull the store data for this store ID
        string strURL = string.Format("http://{0}/JSONServices/getFreeResults3.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

		for (int i = 0; i < retries; i++)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                ri = JsonConvert.DeserializeObject<GetFreeResultsResponse3>(strJsonResult);
                if (ri.Error.Length == 0)
                {
					return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
			Thread.Sleep(retrySleep);
        }

        ErrorHandler.ErrorMessage(6, ri.Error, 1);

        return (ri);
    }

    public static CheckResultInfo3 CheckResults3(int StoreID, int UserID, Int16[] Multiplier, UInt64[] ID)
    {
        CheckResultInfo3 ri = new CheckResultInfo3();
        ri.Error = string.Empty;

        CheckResultRequestInfo3 rri = new CheckResultRequestInfo3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;
        rri.Multiplier = Multiplier;
        rri.ID = ID;

        // 1. Pull the store data for this store ID
        string strURL = string.Format("http://{0}/JSONServices/CheckResults3.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

		for (int i = 0; i < retries; i++)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                if (Config.DEBUG_FULL) Debug.Log(strJsonResult);
                ri = JsonConvert.DeserializeObject<CheckResultInfo3>(strJsonResult);
                if (ri.Error.Length == 0)
                {
                    return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
			Thread.Sleep(retrySleep);
        }

		ErrorHandler.ErrorMessage(6, ri.Error, 1);
        return (ri);
    }

    public static RegisterBadEntriesResponse3 RegisterBadEntries3(int StoreID, int UserID, UInt64[] IDs)
    {
        RegisterBadEntriesResponse3 ri = new RegisterBadEntriesResponse3();
        ri.Error = string.Empty;

        RegisterBadEntriesRequest3 rri = new RegisterBadEntriesRequest3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;
        rri.IDs = IDs;

        // 1. Pull the store data for this store ID
        string strURL = string.Format("http://{0}/JSONServices/RegisterBadEntries3.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);
        WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
        {
            try
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                if (Config.DEBUG_FULL) Debug.Log(strJsonResult);
                ri = JsonConvert.DeserializeObject<RegisterBadEntriesResponse3>(strJsonResult);
                if (ri.Error.Length == 0)
                {
                    return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
			Thread.Sleep(retrySleep);
        }
	
		ErrorHandler.ErrorMessage(6, ri.Error, 1);
        return (ri);
    }

    public static void addToWinnings(int amount)
    {
        int userId = Convert.ToInt32(ID);
        int StoreID = TheStoreID;
        AddToWinningsRequest req = new AddToWinningsRequest();
        req.storeId = StoreID;
        req.amountToAdd = amount;
        req.userId = userId;

        AddToWinningsResponse res = new AddToWinningsResponse();
        res.error = String.Empty;

        string strURL = String.Format("http://{0}/jsonservices/AddToWinnings4.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(req);

		WebClient wc = new WebClient();

		for (int i = 0; i < retries; i++)
		{
			try
			{
				wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                //string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                wc.Headers[HttpRequestHeader.ContentType] = "text/plain";

               // string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                string strJsonResult = Horseshoe.StringCipher.Decrypt((string)wc.UploadString(strURL, "POST", Horseshoe.StringCipher.Encrypt(strJsonInput)));

                res = JsonConvert.DeserializeObject<AddToWinningsResponse>(strJsonResult);
				if(res.error.Length == 0) return;
			}
			catch (System.Exception ex)
			{
			}
			Thread.Sleep(retrySleep);
		}

	}

    public static SetResultInfo3 SetResults3(int StoreID, int UserID, Int16[] Multiplier, UInt64[] ID, Int16[] PotMultiplier, UInt64[] PotID, bool Success)
    {
        SetResultInfo3 ri = new SetResultInfo3();
        ri.Error = string.Empty;

        SetResultRequestInfo3 rri = new SetResultRequestInfo3();
        rri.StoreID = StoreID;
        rri.UserID = UserID;
        rri.Multiplier = Multiplier;
        rri.ID = ID;
        rri.PotMultiplier = PotMultiplier;
        rri.PotID = PotID;
        rri.FailedSkillGame = Success;
        rri.GameId = gameId;

        // 1. Pull the store data for this store ID
       // string strURL = string.Format("http://{0}/JSONServices/SetResults3.aspx", strDomain);
        string strURL = string.Format("http://{0}/JSONServices/SetResults4.aspx", strDomain);
        string strJsonInput = JsonConvert.SerializeObject(rri);

        WebClient wc = new WebClient();

        for (int i = 0; i < retries; i++)
        {
            try
            {
               // wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Headers[HttpRequestHeader.ContentType] = "text/plain";

               // string strJsonResult = (string)wc.UploadString(strURL, "POST", strJsonInput);
                string strJsonResult = Horseshoe.StringCipher.Decrypt((string)wc.UploadString(strURL, "POST", Horseshoe.StringCipher.Encrypt(strJsonInput)));

                ri = JsonConvert.DeserializeObject<SetResultInfo3>(strJsonResult);
                if (ri.Error.Length == 0)
                {
					return (ri);
                }
            }
            catch (System.Exception ex)
            {
                ri.Error = ex.Message.ToString();
            }
            Thread.Sleep(retrySleep);
        }

        ErrorHandler.ErrorMessage(6, ri.Error, 1);
        return (ri);
    }
}
