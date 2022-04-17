import React, { useState } from 'react';
import './leaderboard.css';

function LeaderboardPageUI()
{

    let bp = require('./Path.js');

    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);

    var SearchingName;

    var currPage = 1;
    var totalPages = 1;

    var friendsArr;
    var Arr;
    var del;

    var storage = require('../tokenStorage.js');
    var username = ud.Username;
    var email = ud.email;

    const [message,setMessage] = useState('');

    var readyToRender = true;

    const showLocalLeaderboard = async event => 
    {

        event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/showlocalleaderboard'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('showlocalleaderboard Failed');
            }
            else
            {                                             
                friendsArr = res.localScores;

                
                
                // we get the table and clear it of previous rows
                var table = document.getElementById("leaderboardTable");


                if (friendsArr.length == 0)
                {
                    table.innerHTML = "Seems like none of your friends have any High Scores. Make some friends on the friends tab!"
                    return;
                }


                table.innerHTML = "<thead><tr><th id='boardTh1'>Rank</th><th id='boardTh2'>" +
                    "Name </th> <th id='boardTh3'>Score</th></tr></thead>" + "";

                for (var i = 0; i < friendsArr.length; i++)
                {
                    // we render the table rows individually, and fill each row
                    // with the rank, name, and score of each friend
                    // top 3 scorers are in red, while the rest are yellow
                    
                    var row = table.insertRow();

                    // cell for rank
                    var n = row.insertCell();
                    n.setAttribute("id", "tabledata");
                    if (i == 0) n.innerHTML = i+1 + "st";
                    else if (i == 1) n.innerHTML = i+1 + "nd";
                    else if (i == 2 )n.innerHTML = i+1 + "rd";
                    else n.innerHTML = i+1 + "th";
                    if (i < 3) n.style.color = "red";
                    n.style.borderBottom = "none";

                    // cell for name
                    n = row.insertCell();
                    n.innerHTML = friendsArr[i].Username;
                    n.style.borderBottom = "none";
                    n.setAttribute("id", "tabledata");
                    if (i < 3) n.style.color = "red";

                    // cell for score
                    n = row.insertCell();
                    n.innerHTML = friendsArr[i].Score;
                    n.style.borderBottom = "none";
                    n.setAttribute("id", "tabledata");
                    if (i < 3) n.style.color = "red";
                };

            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const showAllLeaderboard = async event => 
    {

        event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/showgloballeaderboard'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            if(res.userID <= 0)
            {
                setMessage('searchnewfriends Failed');
            }
            else
            {                                             

                Arr = res.globalScores;

                // we get the table and clear it of previous rows
                var table = document.getElementById("leaderboardTable");
                table.innerHTML = "<thead><tr><th id='boardTh1'>Rank</th><th id='boardTh2'>" +
                    "Name </th> <th id='boardTh3'>Score</th></tr></thead>" + "";

                for (var i = 0; i < Arr.length; i++)
                {
                    // we render the table rows individually, and fill each row
                    // with the rank, name, and score of each player
                    // top 3 scorers are in red, while the rest are yellow
                    
                    var row = table.insertRow();

                    // cell for rank
                    var n = row.insertCell();
                    n.setAttribute("id", "tabledata");
                    if (i == 0) n.innerHTML = i+1 + "st";
                    else if (i == 1) n.innerHTML = i+1 + "nd";
                    else if (i == 2 )n.innerHTML = i+1 + "rd";
                    else n.innerHTML = i+1 + "th";
                    if (i < 3) n.style.color = "red";
                    n.style.borderBottom = "none";

                    // cell for name
                    n = row.insertCell();
                    n.innerHTML = Arr[i].Username;
                    n.style.borderBottom = "none";
                    n.setAttribute("id", "tabledata");
                    if (i < 3) n.style.color = "red";

                    // cell for score
                    n = row.insertCell();
                    n.innerHTML = Arr[i].Score;
                    n.style.borderBottom = "none";
                    n.setAttribute("id", "tabledata");
                    if (i < 3) n.style.color = "red";
                };

            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    return(

        <div id="LeaderboardPageBody" >

            <input type="button" id="showLocalBtn" class="buttons" value = "Local Leaderboard" onClick={showLocalLeaderboard} />
            <input type="button" id="showAllBtn" class="buttons" value = "World Leaderboard" onClick={showAllLeaderboard} />


            <div id="divWithBoard" class="flexcroll">
                <table id="leaderboardTable">
                </table>
            </div>

            
          
        </div>
    );
}
export default LeaderboardPageUI;