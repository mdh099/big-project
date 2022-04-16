import React, { useState } from 'react';
import './AccountPage.css';

function AccountPageUI()
{

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

    var storage = require('../tokenStorage.js');

    /*
        const addCard = async event => 
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
        const searchCard = async event => 
        {
            event.preventDefault();
        
            var obj = {userId:userId,search:search.value};
            var js = JSON.stringify(obj);
            try
            {
                const response = await fetch(bp.buildPath('api/searchcards'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
                var txt = await response.text();
                var res = JSON.parse(txt);
                var _results = res.results;
                var resultText = '';
                for( var i=0; i<_results.length; i++ )
                {
                    resultText += _results[i];
                    if( i < _results.length - 1 )
                    {
                        resultText += ', ';
                    }
                }
                setResults('Card(s) have been retrieved');
                setCardList(resultText);
            }
            catch(e)
            {
                alert(e.toString());
                setResults(e.toString());
            }
        };
        /*
        <div id="AccountPageUI">
          <br />
          <input type="text" id="searchText" placeholder="Card To Search For" ref={(c) => search = c} />
          <button type="button" id="searchCardButton" class="buttons" onClick={searchCard}> Search Card</button><br />
          <span id="cardSearchResult">{searchResults}</span>

          <p id="cardList">{cardList}</p><br /><br />

          <input type="text" id="cardText" placeholder="Card To Add" ref={(c) => card = c} />
          <button type="button" id="addCardButton" class="buttons" onClick={addCard}> Add Card </button><br />
          <span id="cardAddResult">{message}</span>
        </div>
    */

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


    return(

        <div id="AccountPageBody">

          <button type="button" id="accountAccBtn" class="accButtons" title="Account"
          onClick={viewAccount}><c> Account</c> </button>
          <button type="button" id="accountchangePassBtn" class="accButtons" title="Change Password"
          onClick={changePass}><d> Change Password</d> </button>
          <button type="button" id="accountchangeEmailBtn" class="accButtons" title="Change Email"
          onClick={changeEmail} ><e> Change Email</e> </button>

          <div id="accountMainBox">
            <div id = "mainBoxContent">
                <br />
                Username: {username} <br /><br />
                Email: {email} <br /><br />
                Friends: 0 <br /><br />
                High Score: 99999 <br /><br />
            </div>
          </div>
        </div>
    );
}
export default AccountPageUI;
