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

    var search = '';
    const [message,setMessage] = useState('');
    const [searchResults,setResults] = useState('');

    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);
    var userId = ud.id;
    var firstName = ud.firstName;
    var lastName = ud.lastName;

    var username = ud.Username;
    var email = ud.email; 
    var highScore = 0;

    const updateAccount = async event =>
    {
        // this does is gets highScore and displays it
        var obj = {userID: ud.userID};
        var js2 = JSON.stringify(obj);

        try
        {
            const response = await fetch(bp.buildPath('api/displayhighscore'), {method:'POST', body:js2, headers:{'Content-Type': 'application/json'}}); // await fetch


            var res = JSON.parse(await response.text());

            highScore = res.highscore;

            document.getElementById("btnToRevealScore").innerHTML= highScore;

            if (res.error !== "")
            {
                setMessage('Error Getting HighScore: ' + res.error);
            }
            else
            {
                setMessage('Got highScore');
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    }


    window.addEventListener('DOMContentLoaded', (event) => {
        updateAccount();
    });

    return(

        <div id="AccountPageBody">

            <div id = "mainBoxContent">
                Username: {username} <br /><br />
                Email: {email} <br /><br />
                High Score: <a id="btnToRevealScore">  </ a> <br /><br />
            </div>

        </div>
    );
}
export default AccountPageUI;
