import React, { useState } from 'react';
import './AccountPage.css';

function AccountPageUI()
{

    // Retrieve User Data
    var storage = require('../tokenStorage.js');
    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);
    var username = ud.username;
    var email = ud.email;

    let bp = require('./Path.js');

    var card = '';
    var search = '';
    const [message,setMessage] = useState('');
    const [searchResults,setResults] = useState('');
    const [cardList,setCardList] = useState('');

    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);
    var userId = ud.id;
    var firstName = ud.firstName;
    var lastName = ud.lastName;



    const viewFriends = async event => 
    {
        event.preventDefault();
        var obj = {userId:userId,card:card.value};
        var js = JSON.stringify(obj);
        try
        {
            const response = await fetch(bp.buildPath('api/addcard'),
            {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var txt = await response.text();
            var res = JSON.parse(txt);
            if( res.error.length > 0 )
            {
                setMessage( "API Error:" + res.error );
            }
            else
            {
                setMessage('Card has been added');
            }
        }
        catch(e)
        {
            setMessage(e.toString());
        }
    };

    const viewAccount = async event => 
    {

        var tok = storage.retrieveToken();
        //var obj = {userID:userID,username:username,email:email};

        var accTab = document.getElementById("accountAccBtn");
        var passTab = document.getElementById("accountchangePassBtn");
        var mailTab = document.getElementById("accountchangeEmailBtn");
        var mainBoxText = document.getElementById("mainBoxContent");

        passTab.style.background = "white";
        accTab.style.background = "#E5E5E5";
        mailTab.style.background = "white";

        mainBoxText.innerHTML  = "<br />Username: " + username + " <br /><br />" +
                "Email: "+ email +" <br /><br />" +
                "High Score: 99999 <br /><br />";
    };

    const changePass = async event => 
    {
        var accTab = document.getElementById("accountAccBtn");
        var passTab = document.getElementById("accountchangePassBtn");
        var mailTab = document.getElementById("accountchangeEmailBtn");
        var mainBoxText = document.getElementById("mainBoxContent");

        passTab.style.background = "#E5E5E5";
        accTab.style.background = "white";
        mailTab.style.background = "white";

        mainBoxText.innerHTML  = "Here we will change password";
    };

    const changeEmail = async event => 
    {
        var accTab = document.getElementById("accountAccBtn");
        var passTab = document.getElementById("accountchangePassBtn");
        var mailTab = document.getElementById("accountchangeEmailBtn");
        var mainBoxText = document.getElementById("mainBoxContent");

        mailTab.style.background = "#E5E5E5";
        accTab.style.background = "white";
        passTab.style.background = "white";

        mainBoxText.innerHTML  = "Here we will change email";
    };


    /*
        <div id="topGrayRect">
        <a href="/account">Account</a>
        <a href="/account">Friends</a>
        <a href="/leaderboard">Leaderboard</a>
        <button type="button" id="logoutButton" class="buttons" onClick={doLogout}> Log Out </button>
        </div>
    */

    var username = ud.Username;
    var email = ud.email; 

/* i am so sorry for the divs, TODO CLEAN THEM UP*/
    return(

        <div id="AccountPageBody">

            <div id = "mainBoxContent">
                <br />
                <br />
                <br />
                <br />
                Username: {username} <br /><br />
                Email: {email} <br /><br />
                Friends: /*TODO*/ <br /><br />
                High Score: /*TODO*/ <br /><br />
            </div>

        </div>
    );
}
export default AccountPageUI;
