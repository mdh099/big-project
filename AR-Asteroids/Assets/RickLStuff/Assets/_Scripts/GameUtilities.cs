using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameUtilities
{
    // The end all, be all, for what constitutes the valid win lines.
    public static class WinLines
    {
        /************************ Members ************************/

        //  Hardcoded lines that for getting, but not setting
        private static readonly int[,] lines = new int[,]
        {
            { 1, 4, 7, 10, 13 },    // Line 1
            { 0, 3, 6, 9,  12 },    // Line 2
            { 2, 5, 8, 11, 14 },    // Line 3
            { 0, 3, 8, 9,  12 },    // Line 4
            { 0, 4, 6, 10, 12 },    // Line 5
            { 0, 4, 8, 10, 12 },    // Line 6
            { 1, 4, 8, 10, 13 },    // Line 7
            { 1, 5, 6, 11, 13 },    // Line 8
            { 1, 5, 7, 11, 13 },    // Line 9
            { 1, 5, 8, 11, 13 },    // Line 10
            { 2, 3, 8, 9,  14 },    // Line 11
            { 2, 4, 6, 10, 14 },    // Line 12
            { 2, 5, 6, 11, 14 },    // Line 13
            { 0, 3, 7, 11, 14 },    // Line 14
            { 0, 4, 7, 10, 14 },    // Line 15
            { 2, 5, 7, 9,  12 },    // Line 16
            { 0, 5, 6, 11, 12 },    // Line 17
            { 0, 5, 6, 10, 14 },    // Line 18
            { 0, 5, 7, 11, 12 },    // Line 19
            { 0, 5, 7, 9,  14 },    // Line 20
            { 0, 5, 8, 11, 12 },    // Line 21
            { 0, 5, 8, 9,  14 },    // Line 22
            { 1, 3, 6, 9,  13 },    // Line 23
            { 1, 3, 6, 10, 14 },    // Line 24
            { 1, 3, 7, 9,  13 },    // Line 25
            { 1, 3, 7, 11, 13 },    // Line 26
            { 1, 3, 8, 9,  13 },    // Line 27
            { 1, 3, 8, 11, 13 },    // Line 28
            { 1, 4, 6, 10, 13 },    // Line 29
            { 1, 4, 6, 11, 14 },    // Line 30
            { 2, 3, 6, 9,  14 },    // Line 31
            { 2, 3, 6, 11, 12 },    // Line 32
            { 2, 4, 8, 10, 14 },    // Line 33
            { 2, 4, 8, 9,  13 },    // Line 34
            { 2, 4, 7, 10, 14 },    // Line 35
            { 2, 4, 7, 11, 14 },    // Line 36
            { 2, 4, 7, 9,  14 },    // Line 37
            { 2, 3, 7, 11, 13 },    // Line 38
            { 2, 3, 7, 9,  14 },    // Line 39
            { 2, 3, 7, 10, 12 }     // Line 40
        };
        // Below are three "buckets" for uniqueness.
        // Singlularly unique. Two lines sharing first three nodes.
        // Three lines sharing first three nodes.
        private static readonly int[] singleLines = new int[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        };
        private static readonly int[,] doubleLines = new int[,]
        {
            { 16, 17 }, { 18, 19 }, { 20, 21 }, { 22, 23 }, { 24, 25 },
            { 26, 27 }, { 28, 29 }, { 30, 31 }, { 32, 33 }
        };
        private static readonly int[,] tripleLines = new int[,]
        {
            { 34, 35, 36 }, { 37, 38, 39 }
        };
        //  Constant used to ensure we never exceed this number
        //  during verification checks. Must match # of rows in lines[]   
        public static readonly int NUMBER_OF_WIN_LINES = 40;

        /************************ Methods ************************/

        // Getter for all lines
        public static int[,] GetLines() { return lines; }
        // Getter for all singularly unique lines
        public static int[] GetSingleLines() { return singleLines; }
        // Getter for all lines grouped in double that share first three nodes.
        public static int[,] GetDoubleLines() { return doubleLines; }
        // Getter for all lines grouped in triple that share first three nodes.
        public static int[,] GetTripleLines() { return tripleLines; }
        // Getter for specific line
        public static int[] GetALine(int li)
        {
            int[] newLine = new int[5];

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    newLine[i] = lines[li, i];
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-GetALine", ex.ToString(), 1);
            }
            return newLine;
        }
        // Getter for specific double line combo
        public static int[] GetADoubleLine(int li)
        {
            int[] newCombo = new int[2];
            try
            {
                newCombo[0] = doubleLines[li, 0];
                newCombo[1] = doubleLines[li, 1];
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-GetADoubleLine", ex.ToString(), 1);
            }
            return newCombo;
        }
        // Getter for specific triple line combo
        public static int[] GetATripleLine(int li)
        {
            int[] newCombo = new int[3];
            try
            {
                newCombo[0] = tripleLines[li, 0];
                newCombo[1] = tripleLines[li, 1];
                newCombo[2] = tripleLines[li, 2];
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-GetATripleLine", ex.ToString(), 1);
            }
            return newCombo;
        }
    }
    // All things paytable
    public static class PayTable
    {
        public static int freeSpinMultiplier;
        /************************ Members ************************/
        public static readonly int[] MULTIPLIERS =
        {
            5, 10, 20, 25,
            50, 75, 100, 200,
            250, 500, 1000, 2000,
            3000, -2
        };
        // Lookup table to find all card combinations that give the desired payout.
        // index 0 = 0 combos, 1 = 1 combo, 2 = a pair, 3 = triple, 4 = quadruple, 5 = 5 combo.
        public static readonly int[,] cardValues = new int[,]
        {   //  combinations of card values:
            //  0   1       2       3       4       5
            {   0,  0,      0,      5,      20,     100     },  // Card 1   (Normal Payout)
            {   0,  0,      0,      5,      25,     200     },  // Card 2   (Normal Payout)
            {   0,  0,      0,      5,      50,     200     },  // Card 3   (Normal Payout)
            {   0,  0,      0,      10,     50,     250     },  // Card 4   (Normal Payout)
            {   0,  0,      0,      10,     75,     500     },  // Card 5   (Normal Payout)
            {   0,  0,      0,      20,     75,     500     },  // Card 6   (Normal Payout)
            {   0,  0,      0,      50,     250,    1000    },  // Card 7   (Normal Payout)
            {   0,  0,      0,      100,    2000,   3000    },  // Card 8   (Normal Payout)
            {   0,  0,      0,      -2,     0,      0       },  // Card 9   (Bonus Card: Free Spins)
            {   0,  0,      0,      0,      0,      0       }   // Card 10  (Wild Card: must always be last index)
        };

        /************************ Methods ************************/

        // Finds all card value combos that match the given payout.
        // Used by board configuration handler to match card combinations
        // to desired pay outcome.
        public static Tuple<int, int> PickPayoutCombos(int payout)
        {
            System.Random rando = new System.Random();
            // Makes sure the payout is a valid payout
            bool validPayout = false;
            Tuple<int, int> choice = null;
            try
            {
                for (int k = 0; k < MULTIPLIERS.Length; k++)
                {
                    if (MULTIPLIERS[k] == payout)
                    {
                        validPayout = true;
                    }
                }
                if (!validPayout)
                {
                    return Tuple.Create(-1, -1);
                }
                // Picks a winning combo matching the payout
                List<Tuple<int, int>> keyReferences = new List<Tuple<int, int>>();
                for (int i = 0; i < cardValues.GetLength(0); i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (cardValues[i, j] == payout)
                        {
                            keyReferences.Add(Tuple.Create(i, j));
                        }
                    }
                }
                choice = keyReferences.ElementAt(rando.Next(keyReferences.Count));
                if (Config.DEBUG_FULL) UnityEngine.Debug.Log("Payout = " + payout + "   CardValue = " + choice.Item1 + "   NumOfCards = " + choice.Item2);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-PickPayoutCombos", ex.ToString(), 1);
            }
            return choice;
        }
        // Returns whether that combination produces value greater than zero (nothing).
        public static bool IsPayout(int cardVal, int combinations)
        {
            if (Config.DEBUG || Config.DEBUG_FULL) UnityEngine.Debug.Log("IsPayout-cardValue: " + cardVal + " --- NumOfCards: " + combinations);
            if (cardValues[cardVal, combinations] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int[] values = new int[4];

        // Returns a tuple of the total payout schema (Total Payout, Max Free Spins, mult-id list of entries).
        public static Tuple3<int, int, List<Tuple<int, ulong>>> GetPayoutPackage(int[] winValues, List<Tuple<int, ulong>> totalValues)
        {
            Tuple3<int, int, List<Tuple<int, ulong>>> payoutPackage = null;
            try
            {
                if (winValues.Length <= 0)
                {
                    payoutPackage = new Tuple3<int, int, List<Tuple<int, ulong>>>(0, 0, totalValues);
                }
                else
                {
                    int totalCashPayout = 0;
                    int maxFreeSpins = 0;
                    for (int i = 0; i < winValues.Length; i++)
                    {
                        if (winValues[i] >= 0)
                        {
                            totalCashPayout += winValues[i];
                        }
                        else if (winValues[i] < 0)
                        {
                            switch (winValues[i])
                            {
                                case (-2):
                                    {
                                        if (maxFreeSpins == 0)//Prereveal;preveal;bonus spins-random values for mini; removes multiplier -Mike
                                        {
                                            //************If you edit the lines here, you must edit GetSpins() also***************-Mike
                                            int[] miniGameValues = new int[5]; //fifth value if for the extra spins that may be earned in the bonus spins
                                            //random between 1 and 2 where 1 saves spins for later(during bonus spins) and 2 does not
                                            System.Random randoCalrissian = new System.Random();
                                            int addSpins = randoCalrissian.Next(1, 3);
                                            int extraSpins = 0;
                                            if (addSpins == 1)
                                            {
                                                for (int k = 0; k < 4; k++)
                                                {
                                                    miniGameValues[k] = randoCalrissian.Next(1, 4);//get random value between 1 and 3 for each minigame selection
                                                    maxFreeSpins += miniGameValues[k];

                                                    if (miniGameValues[k] > 1) //pull 1 entry if able for extra spins
                                                    {
                                                        miniGameValues[k]--;
                                                        extraSpins++;
                                                    }

                                                    if (k == 3) miniGameValues[4] = extraSpins;//set the extra spins value to the total of spins pulled

                                                }
                                            }
                                            else
                                            {
                                                for (int k = 0; k < 4; k++)
                                                {
                                                    miniGameValues[k] = randoCalrissian.Next(1, 4);
                                                    maxFreeSpins += miniGameValues[k];

                                                }
                                            }
                                            values = miniGameValues;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        ErrorHandler.ErrorMessage(2, "GetPayoutPackage(Entry Value = " + winValues[i] + ")", 3);
                                        break;
                                    }
                            }
                        }
                    }
                    payoutPackage = new Tuple3<int, int, List<Tuple<int, ulong>>>(totalCashPayout, maxFreeSpins, totalValues);//Paypack explanation
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-GetPayoutPackage", ex.ToString(), 1);
            }
            return payoutPackage;
        }

        //For bonus spins
        public static int GetSpins()
        {
            //create random bonus spins between 1 <= x < 4
            int[] miniGameValues = new int[5]; //fifth value if for the extra spins that may be earned in the bonus spins
            int spins = 0;
            System.Random randoCalrissian = new System.Random();

            //random between 1 and 2 where 1 saves spins for later(during bonus spins) and 2 does not
            int addSpins = randoCalrissian.Next(1, 3);
            int extraSpins = 0;
            if(addSpins == 1)
            {
                for (int k = 0; k < 4; k++)
                {
                    miniGameValues[k] = randoCalrissian.Next(1, 4);//get random value between 1 and 3 for each minigame selection

                    if (miniGameValues[k] > 1) //pull 1 entry if able for extra spins
                    {
                        miniGameValues[k]--;
                        extraSpins++;
                    }

                    if (k == 3) miniGameValues[4] = extraSpins;//set the extra spins value to the total of spins pulled

                    spins += miniGameValues[k];//add to the total spin count
                }
            }
            else
            {
                for (int k = 0; k < 4; k++)
                {
                    miniGameValues[k] = randoCalrissian.Next(1, 4);
                    spins += miniGameValues[k];
                }
            }

            values = miniGameValues;

            return spins;
        }
    }



    // Various Tuple constructs
    public class Tuple<T, U>
    {
        public T Item1 { get; private set; }
        public U Item2 { get; private set; }

        public Tuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    public static class Tuple
    {
        public static Tuple<T, U> Create<T, U>(T item1, U item2)
        {
            return new Tuple<T, U>(item1, item2);
        }
    }
    public class Tuple3<T, U, V>
    {
        public T Item1 { get; private set; }
        public U Item2 { get; private set; }
        public V Item3 { get; private set; }

        public Tuple3(T item1, U item2, V item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }
    public static class Tuple3
    {
        public static Tuple3<T, U, V> Create<T, U, V>(T item1, U item2, V item3)
        {
            return new Tuple3<T, U, V>(item1, item2, item3);
        }
    }
    public class Tuple4<T, U, V, W>
    {
        public T Item1 { get; private set; }
        public U Item2 { get; private set; }
        public V Item3 { get; private set; }
        public W Item4 { get; private set; }

        public Tuple4(T item1, U item2, V item3, W item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
    }
    public static class Tuple4
    {
        public static Tuple4<T, U, V, W> Create<T, U, V, W>(T item1, U item2, V item3, W item4)
        {
            return new Tuple4<T, U, V, W>(item1, item2, item3, item4);
        }
    }
    public class Tuple5<T, U, V, W, X>
    {
        public T Item1 { get; private set; }
        public U Item2 { get; private set; }
        public V Item3 { get; private set; }
        public W Item4 { get; private set; }
        public X Item5 { get; private set; }

        public Tuple5(T item1, U item2, V item3, W item4, X item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }
    }
    public static class Tuple5
    {
        public static Tuple5<T, U, V, W, X> Create<T, U, V, W, X>(T item1, U item2, V item3, W item4, X item5)
        {
            return new Tuple5<T, U, V, W, X>(item1, item2, item3, item4, item5);
        }
    }
    // Used to assist SoundManager and it's subsidiary classes.
    public static class SoundManagement
    {
        public enum Sounds
        {
            BUTTON_GROUP_1, BUTTON_GROUP_2, SKILL_GAME_SUCCESS, JACKPOT,
            SKILL_GAME_FAILED, SPINNING, STOPPED, WINNING_LINE, YOU_WIN,
            MINIGAME_MATCH, MINIGAME_MISMATCH, MINIGAME_CLICK
        };
    }
    // Used to assist ButtonManagement and it's subsidiary classes.
    public static class ButtonManagement
    {
        public enum Buttons
        {
            REVEAL, AUTOPLAY, SHOW_LINES, OVERVIEW, SKILL_GAME, EXIT, INCREMENT_TIME, BUY_TIME, DECREASE_ENTRY, INCREASE_ENTRY
        };
    }
    // Used by game to manage state.
    public static class GameManagement
    {
        /****************** Descriptions of Main Game States. ********************\
        //
        //  LOADING:            Display splash screen. Gives machine a chance to
        //                      load elements of the game. Can only transition to
        //                      the IDLE state. Board display should show a preset
        //                      board layout before transition.
        //
        //  IDLE:               Splash screen is set to disabled. All buttons are
        //                      active (except "Skill"). Autoplay launches from
        //                      here. Transitions on press of reveal button.
        //
        //  IDLE_JACKPOT        Someone is winning a a jackpot, but we don't know
        //                      yey home. Chasing animations play randomly. When
        //                      Jackpot is awarded, show graphic, and return to IDLE.
        //
        //  PAY_TABLE:          Pay table is showing. All buttons (except return
        //                      to game) are disabled. Turn off animations in
        //                      board. Transitions to IDLE only.
        //
        //  RULES_SHOWING:      Rules are showing. All buttons (except return to
        //                      game, and change rules page) are disabled.
        //                      Transitions to IDLE only.
        //
        //  SPINNING:           Starts the spin cycle. Persists for 3-4 seconds.
        //                      "Pre-reveal" is on (if selected) Show Lines is
        //                      deactivated. "Reveal", "Pay Table", "Rules", and
        //                      "Exit" are disabled. Transitions to STOPPING only.
        //
        //  STOPPING:           Board configuration exists, reels start slowing
        //                      down. They stop in position and this transitions to
        //                      SHOWING_LINES. "Skill", "Reveal", "Exit",
        //                      "Pay Table", and rules "Rules" disabled. Only the
        //                      toggle for activating or deactivating autoplay is
        //                      possible, and "Pre-Reveal." Transitions only to
        //                      SHOWING_LINES.
        //
        //  SHOWING_LINES:      Board configuration is still and showing. Win
        //                      lines, borders, and card combos are animated.
        //                      Minimum of one display cycle is mandatory.
        //                      Transitions to IDLE.
        //
        //  START_MINIGAME:     After initial display in SHOWING_LINES, if free
        //                      spins were awarded, minigame is populated and
        //                      launched. All non-minigame buttons (Except "Exit")
        //                      are disabled. Transitions to MINIGAME_ACTIVE.
        //
        //  MINIGAME_ACTIVE:    MiniGameManager has total control during this, and
        //                      same buttons are disabled as in START_MINIGAME.
        //                      Transitions to CLOSE_MINIGAME after win or loss.
        //
        //  CLOSE_MINIGAME:     GameManager received percentage of possible free
        //                      spins user has one. Minigame window is closed.
        //                      Transitions to IDLE.
        //
        //  OUT_OF_ENTRIES:     All of user's entries are gone. Autoplay turns off.
        //                      Reveal is disabled. User must exit and purchase
        //                      more entries. Also used for timer running out.
        //
        //  EXITING:            Displays the Main Menu display screen as game is
        //                      dismantled responsibly. Skill-based pay collection?
        //                      GameManager passes all user data to MenuManager.
        //                      
        **************************************************************************/
        public enum States
        {
            LOADING, IDLE, OVERVIEW_SHOWING, SPINNING, STOPPING,
            SHOWING_LINES, PLAYING_SKILL, START_MINIGAME, MINIGAME_ACTIVE,
            CLOSE_MINIGAME, OUT_OF_ENTRIES, EXITING, IDLE_JACKPOT
        };
        private static List<Tuple<int[], int[]>> winLineMemoization = new List<Tuple<int[], int[]>>();
        private static bool somethingToCommit = false;
        public static void AddMemo(Tuple<int[], int[]> memo)
        {
            somethingToCommit = true;
            winLineMemoization.Add(memo);
        }
        public static void BuildMemo()
        {
            return;

        }
        // Only gets used in the editor version.
        public static void CommitMemo()
        {
            try
            {
                if (somethingToCommit)
                {
#if crap
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("Assets/_Data/memoData.txt", true))
                    {
                        int[] wl = winLineMemoization.Last().Item1;
                        int[] po = winLineMemoization.Last().Item2;
                        string memoEntry = "";
                        for (int i = 0; i < wl.Length; i++)
                        {
                            memoEntry += wl[i] + "#";
                        }
                        memoEntry += "*";
                        for (int j = 0; j < po.Length; j++)
                        {
                            memoEntry += po[j] + "#";
                        }
                        file.WriteLine(memoEntry);
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-CommitMemo", ex.ToString(), 1);
            }
        }
        public static List<Tuple<int[], int[]>> GetMemo()
        {
            return winLineMemoization;
        }
        public static void RemoveMemo()
        {
            try
            {
                if (somethingToCommit)
                {
                    somethingToCommit = false;
                    winLineMemoization.Remove(winLineMemoization.Last());
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-RemoveMemo", ex.ToString(), 1);
            }
        }
    }
    public static class MiniGameManagement
    {
        public enum State { OUT_OF_TURNS, CARD_PICKED_ANIMATING, IDLE, WAIT };
    }
    // Used by game to manage state.
    public static class ErrorManagement
    {
        public static string repeatChar(string character, int number)
        {
            string template = "";
            for (int i = 0; i < number; i++)
            {
                template += character;
            }
            return template;
        }
        public static void ClearErrorFile()
        {
            try
            {
                string filePath;
                if (System.IO.Directory.Exists("Assets/_Data"))
                {
                    filePath = "Assets/_Data/errorLog.txt";
                }
                else
                {
                    filePath = UnityEngine.Application.dataPath + "/errorLog.txt";
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false))
                {
                    file.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(14, "GameUtilities-ClearErrorFile", ex.ToString(), 0);
            }
        }
        public static void SendError(string msg)
        {
            try
            {
                string filePath;
                if (System.IO.Directory.Exists("Assets/_Data"))
                {
                    filePath = "Assets/_Data/errorLog.txt";
                }
                else
                {
                    filePath = UnityEngine.Application.dataPath + "/errorLog.txt";
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    string errorLine = "";
                    string remoteError = "";
                    errorLine += repeatChar("*", 80);
                    remoteError = errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = "" + DateTime.Now;
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = repeatChar("-", 20);
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = msg;
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    string[] errors = new string[1];
                    errors[0] = remoteError;
                    ConnectionDriver.SendError(ConnectionDriver.GetStoreId(), errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-SendError", ex.ToString(), 1);
            }
        }
        public static void SendError(string msg, string exception)
        {
            try
            {
                string filePath;
                if (System.IO.Directory.Exists("Assets/_Data"))
                {
                    filePath = "Assets/_Data/errorLog.txt";
                }
                else
                {
                    filePath = UnityEngine.Application.dataPath + "/errorLog.txt";
                }
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    string errorLine = "";
                    string remoteError = "";
                    errorLine += repeatChar("*", 80);
                    remoteError = errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = "" + DateTime.Now;
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = repeatChar("-", 20);
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = msg;
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = repeatChar("-", 20);
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    errorLine = exception;
                    remoteError += errorLine + "\n";
                    file.WriteLine(errorLine);
                    string[] errors = new string[1];
                    errors[0] = remoteError;
                    ConnectionDriver.SendError(ConnectionDriver.GetStoreId(), errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-SendError", ex.ToString(), 1);
            }
        }
    }
    // Miscellaneous functions used across the game.
    public static class Tools
    {
        private static System.Random rando = new System.Random();
        public static int GetRandomNum(int number)
        {
            return rando.Next(number);
        }
        // Creates a sequential list, and randomizes it.
        public static List<int> Randomize(int desiredLength)
        {
            List<int> stack = null;
            try
            {
                if (desiredLength <= 0)
                {
                    ErrorHandler.ErrorMessage(2, "Randomize(Length = " + desiredLength + ")", 1);
                }
                List<int> pureStack = new List<int>();
                for (int i = 0; i < desiredLength; i++)
                {
                    pureStack.Add(i);
                }
                // Shake them up.
                stack = new List<int>();
                do
                {
                    int index = rando.Next(desiredLength);
                    int elem = pureStack.ElementAt(index);
                    pureStack.RemoveAt(index);
                    desiredLength--;
                    stack.Add(elem);
                } while (desiredLength > 0);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-Randomize", ex.ToString(), 1);
            }
            return stack;
        }
        // Overloaded version of previous. Randomizes inputted list of ints.
        public static List<int> Randomize(List<int> values)
        {
            int totalItems = values.Count;
            List<int> stack = null;

            try
            {
                if (values.Count <= 0)
                {
                    ErrorHandler.ErrorMessage(2, "Randomize(List.Count <= 0)", 1);
                }

                // Shake them up.
                stack = new List<int>();
                do
                {
                    int index = rando.Next(totalItems);
                    int elem = values.ElementAt(index);
                    values.RemoveAt(index);
                    totalItems--;
                    stack.Add(elem);
                } while (totalItems > 0);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-Randomize", ex.ToString(), 1);
            }
            return stack;
        }
        // Overloaded version of previous. Randomizes inputted array of strings.
        public static List<string> Randomize(List<string> values)
        {
            int totalItems = values.Count;
            List<string> stack = null;

            try
            {
                if (values.Count <= 0)
                {
                    ErrorHandler.ErrorMessage(2, "Randomize(List.Count <= 0)", 1);
                }

                // Shake them up.
                stack = new List<string>();
                do
                {
                    int index = rando.Next(totalItems);
                    string elem = values.ElementAt(index);
                    values.RemoveAt(index);
                    totalItems--;
                    stack.Add(elem);
                } while (totalItems > 0);
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorMessage(7, "GameUtilities-Randomize", ex.ToString(), 1);
            }
            return stack;
        }
    }
    // Custom Unity builds that transfer the memoization file into the build folder.

}