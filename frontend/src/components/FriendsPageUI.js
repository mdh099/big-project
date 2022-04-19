import React, { useState } from 'react';
import './FriendsPage.css';

function FriendsPageUI()
{

    let bp = require('./Path.js');

    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);

    var onNewFriends = false;
    var currPage = 1;

    var friendsArr;

    var storage = require('../tokenStorage.js');
    var username = ud.Username;
    var email = ud.email;

    // usersShown will hold ID's of the current friends that are shown on the table
    var usersShown = {
        0 : 0,
        1 : 0,
        2 : 0,
        3 : 0,
        4 : 0
    }

    const [message,setMessage] = useState('');

    const addFriend = async (event, ID) => 
    {
        event.preventDefault();

        var obj = {userID: ud.userID, friendID: usersShown[ID], jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/addfriend'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('AddFriend Failed');
            }
            else
            {                                             
                var user = {username:res.username,id:res.userID, email:res.email}                
            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const deleteFriend = async (event, ID) => 
    {
        event.preventDefault();

        var obj = {userID: ud.userID, friendID: usersShown[ID], jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/deletefriend'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());
            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('deleteFriend Failed');
            }
            else
            {                                             
                var user = {username:res.username,id:res.userID, email:res.email}                
            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const searchCurrentFriends = async event => 
    {

        //event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/searchcurrentfriends'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            console.log(res);

            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('searchCurrentFriend Failed');
            }
            else
            {                                             
                var user = {username:res.username,id:res.userID, email:res.email}

                friendsArr = res.currentFriends;


                // get all of the + and x buttons on left side of table
                var add = document.getElementsByClassName("addFriendButton");
                var del = document.getElementsByClassName("deleteFriendButton");

                for (var j = 0; j < 6; j++)
                {
                    // since we are searching for current friends, we want to be able to delete them
                    // not add them, so we only show X's, not +'s
                    add[j].style.visibility = "hidden";

                    if (j < friendsArr.length)
                        del[j].style.visibility = "visible";
                }

                onNewFriends = false;

                if (friendsArr.length == 0)
                {

                }
                
                // we get the table and clear it of previous rows
                var table = document.getElementById("usersTable");

                if (friendsArr.length == 0)
                {
                    table.innerHTML = "Looks like you don't have any friends yet. You can make some by clicking 'Find New Friends'";
                    return;
                }

                table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
                    "High Score </th></tr></thead>"

                for (var i = 0; i < 6 && i < friendsArr.length; i++)
                {
                    // we render the table rows individually, and fill usersShown object with the ID's of the friends that are now displayed
                    
                    usersShown[i] = friendsArr[i].userID;

                    var row = table.insertRow();

                    row.insertCell().innerHTML = friendsArr[i].Username;

                    row.insertCell().innerHTML = friendsArr[i].Highscore;
                };

                // we don't want to have the arrows if there is only one page, so we get rid of them if 
                // friendsArr has less than 6 users
                if (friendsArr.length > 6)
                {
                    document.getElementById("prevPageBtn").style.visibility = "visible";
                    document.getElementById("nextPageBtn").style.visibility = "visible";
                }
                else
                {
                    document.getElementById("prevPageBtn").style.visibility = "hidden";
                    document.getElementById("nextPageBtn").style.visibility = "hidden";
                }

                currPage = 1;

            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const refreshCurrentFriends = async event => 
    {

        sleep(500); // times out for half second while we wait for other command

        event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/searchcurrentfriends'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('searchCurrentFriend Failed');
            }
            else
            {                                             
                var user = {username:res.username,id:res.userID, email:res.email}

                friendsArr = res.currentFriends;


                // get all of the + and x buttons on left side of table
                var add = document.getElementsByClassName("addFriendButton");
                var del = document.getElementsByClassName("deleteFriendButton");

                for (var j = 0; j < 6; j++)
                {
                    // we will hide all buttons by default and only make them visible later as needed
                    del[j].style.visibility = "hidden";
                    add[j].style.visibility = "hidden";
                }

                onNewFriends = false;
                
                // we get the table and clear it of previous rows
                var table = document.getElementById("usersTable");
                table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
                    "High Score </th></tr></thead>"

                for (var i = currPage * 6 - 6; i < (currPage*6) && i < friendsArr.length; i++)
                {
                    // we render the table rows individually, and fill usersShown object with the ID's of the friends that are now displayed
                    
                    usersShown[i] = friendsArr[i].userID;

                    var row = table.insertRow();

                    row.insertCell().innerHTML = friendsArr[i].Username;

                    row.insertCell().innerHTML = friendsArr[i].Highscore;

                    if (i < friendsArr.length)
                        del[i%6].style.visibility = "visible";
                };

                if (friendsArr.length >= 6)
                {
                    document.getElementById("prevPageBtn").style.visibility = "visible";
                    document.getElementById("nextPageBtn").style.visibility = "visible";
                }
                else
                {
                    document.getElementById("prevPageBtn").style.visibility = "hidden";
                    document.getElementById("nextPageBtn").style.visibility = "hidden";
                }


            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    // pauses program for (milliseconds) amount of time
    function sleep(milliseconds) {
      const date = Date.now();
      let currentDate = null;
      do {
        currentDate = Date.now();
      } while (currentDate - date < milliseconds);
    }

    const searchNewFriends = async event => 
    {

        event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/searchnewfriends'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());
            
            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('searchNewFriend Failed');
            }
            else
            {                                            
                var user = {username:res.username,id:res.userID, email:res.email}

                friendsArr = res.users;

                // we get all x and + buttons
                var add = document.getElementsByClassName("addFriendButton");
                var del = document.getElementsByClassName("deleteFriendButton");

                for (var j = 0; j < 6; j++)
                {
                    // since we are searching for new friends, we want to be able to add them, not delete
                    // them, so we only have + buttons, not X's
                    del[j].style.visibility = "hidden";

                    if (j < friendsArr.length)
                        add[j].style.visibility = "visible";
                }

                onNewFriends = true;
                
                // we get the table and clear it of all previous rows
                var table = document.getElementById("usersTable");
                table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
                    "High Score </th></tr></thead>";

                for (var i = 0; i < 6 && i < friendsArr.length; i++)
                {
                    // we render rows of users indivisually and fill usersShown with
                    // the ID's of the users on the table
                    usersShown[i] = friendsArr[i].userID;

                    var row = table.insertRow();

                    var numRow = row;

                    row.insertCell().innerHTML = friendsArr[i].Username;
   
                    row.insertCell().innerHTML = friendsArr[i].Highscore;
                }

                // if the list is only one page, we do not want to have arrows to change page
                if (friendsArr.length > 6)
                {
                    document.getElementById("prevPageBtn").style.visibility = "visible";
                    document.getElementById("nextPageBtn").style.visibility = "visible";
                }
                else
                {
                    document.getElementById("prevPageBtn").style.visibility = "hidden";
                    document.getElementById("nextPageBtn").style.visibility = "hidden";
                }

            }

            currPage = 1;
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const refreshNewFriends = async event => 
    {
        sleep(500); // sleeps for half second while we wait for the other program

        event.preventDefault();

        var obj = {userID: ud.userID, jwtToken: storage.retrieveToken()};
        var js = JSON.stringify(obj);

        try
        {    
            const response = await fetch(bp.buildPath('api/searchnewfriends'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());
            
            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('searchNewFriend Failed');
            }
            else
            {                                            
                var user = {username:res.username,id:res.userID, email:res.email}

                friendsArr = res.users;

                // we get all x and + buttons
                var add = document.getElementsByClassName("addFriendButton");
                var del = document.getElementsByClassName("deleteFriendButton");

                for (var j = 0; j < 6; j++)
                {
                    // we will hide all buttons then make them visible late as needed
                    del[j].style.visibility = "hidden";
                    add[j].style.visibility = "hidden";
 
                }

                onNewFriends = true;
                
                // we get the table and clear it of all previous rows
                var table = document.getElementById("usersTable");
                table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
                    "High Score </th></tr></thead>";

                for (var i = currPage * 6 - 6; i < (currPage*6) && i < friendsArr.length; i++)
                {
                    // we render rows of users indivisually and fill usersShown with
                    // the ID's of the users on the table
                    usersShown[i] = friendsArr[i].userID;

                    var row = table.insertRow();

                    var numRow = row;

                    row.insertCell().innerHTML = friendsArr[i].Username;
   
                    row.insertCell().innerHTML = friendsArr[i].Highscore;

                    if (i < friendsArr.length)
                        add[i%6].style.visibility = "visible";
                }

                // if the list is only one page, we do not want to have arrows to change page
                if (friendsArr.length >= 6)
                {
                    document.getElementById("prevPageBtn").style.visibility = "visible";
                    document.getElementById("nextPageBtn").style.visibility = "visible";
                }
                else
                {
                    document.getElementById("prevPageBtn").style.visibility = "hidden";
                    document.getElementById("nextPageBtn").style.visibility = "hidden";
                }

            }

            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
    };

    const nextFriendPage = async event => 
    {
        var start = currPage * 6;

        if (currPage == 0 || start >= friendsArr.length) return; // we are on right-most page

        // change page buttons are the same for current and new friends, so we need to get both
        var add = document.getElementsByClassName("addFriendButton");
        var del = document.getElementsByClassName("deleteFriendButton");

        // get table and clear it of current users on the rows
        var table = document.getElementById("usersTable");
        table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
            "High Score </th></tr></thead>"

        for (var i = start; i <= start + 5; i++)
        {
            if (i >= friendsArr.length)
            {
                // users left does not fill up entire table, so we get rid of excess buttons
                if (onNewFriends) add[i % 6].style.visibility = "hidden"    
                else del[i % 6].style.visibility = "hidden";
                
                if (i % 6 == 0) break;
                continue;
            }

            // render users individually on its own row, and save ID in usersShown
            usersShown[i%6] = friendsArr[i].userID;

            var row = table.insertRow();

            row.insertCell().innerHTML = friendsArr[i].Username;
            row.insertCell().innerHTML = 0;//friendsArr[i].Scores[0].current_score;
        }

        // increment the current page we are on
        currPage++;
    };

    const prevFriendPage = async event => 
    {
        if (currPage == 1) return; // we are on the left-most page

        var start = (currPage-1) * 6 - 6;

        if (start >= friendsArr.length) return;

        // we get table and clear it of current rows
        var table = document.getElementById("usersTable");
        table.innerHTML = "<thead><tr><th id='nameTh'>Name</th><th id='secondTh'>" +
            "High Score </th></tr></thead>";

        // change page buttons are the same for current and new friends, so we need to get both
        var add = document.getElementsByClassName("addFriendButton");
        var del = document.getElementsByClassName("deleteFriendButton");

        for (var i = start; i < start + 6; i++)
        {
            // we go through each user and render them on a new row

            usersShown[i%6] = friendsArr[i].userID;

            var row = table.insertRow();

            row.insertCell().innerHTML = friendsArr[i].Username;
            row.insertCell().innerHTML = 0;//friendsArr[i].Scores[0].current_score;

            // if we are going back, these rows must be full, so all buttons are visible
            if (onNewFriends) add[i % 6].style.visibility = "visible"    
            else del[i % 6].style.visibility = "visible";
        }

        currPage--;
    };

    // CODE BELOW TO BE ADDED IF WE EVER IMPLEMENT A SEARCH FOR SPECIFIC FRIEND
    // <input type="text" id="usrSearchBar" placeholder="Search" ref={(c) => SearchingName = c} />
    //searchInput.addEventListener('keyup',function(){searchCurrentFriends();});

    window.addEventListener('DOMContentLoaded', (event) => {
        searchCurrentFriends();
    });

    return(

        <div id="friendsPageBody">

          <input type="button" id="searchCurrBtn" class="buttons" value = "Show My Friends" onClick={searchCurrentFriends} />
          <input type="button" id="searchNewBtn" class="buttons" value = "Find New Friends" onClick={searchNewFriends} />


          <input type="button" id="plus1" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 0), refreshNewFriends(event))} />
          <input type="button" id="plus2" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 1), refreshNewFriends(event))} />
          <input type="button" id="plus3" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 2), refreshNewFriends(event))} />
          <input type="button" id="plus4" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 3), refreshNewFriends(event))} />
          <input type="button" id="plus5" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 4), refreshNewFriends(event))} />
          <input type="button" id="plus6" class="addFriendButton" value="+" onClick={(event) => (addFriend(event, 5), refreshNewFriends(event))} />

          <input type="button" id="plus1" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 0), refreshCurrentFriends(event))} />
          <input type="button" id="plus2" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 1), refreshCurrentFriends(event))} />
          <input type="button" id="plus3" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 2), refreshCurrentFriends(event))} />
          <input type="button" id="plus4" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 3), refreshCurrentFriends(event))} />
          <input type="button" id="plus5" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 4), refreshCurrentFriends(event))} />
          <input type="button" id="plus6" class="deleteFriendButton" value="&#10005;" onClick={(event) => (deleteFriend(event, 5), refreshCurrentFriends(event))} />

          <div class="DivWithTable" >
            <table class="usrsTable" id="usersTable">
            </table>
           </div>

           <input type="button" id="prevPageBtn" class="buttons" value = "<" onClick={prevFriendPage} />

           <input type="button" id="nextPageBtn" class="buttons" value = ">" onClick={nextFriendPage} />
           
        </div>
    );

}
export default FriendsPageUI;