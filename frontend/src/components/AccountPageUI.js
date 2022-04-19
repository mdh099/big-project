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

    var username = ud.Username;
    var email = ud.email; 
    var highScore = 0;



   /*
    state = {
        pollingCount: 0,
        delay: 3000
    }

    componentDidMount()
    {
        this.interval = setInterval (this.tick, this.state.delay);
    }

    componentDidUpdate(prevProps, prevState)
    {
        if (prevState.delay != this.state.delay)
        {
            clearInterval(this.interval);
            this.interval = setInterval(this.tick, this.state.delay);
        }
    }

    componentWillUnmount()
    {
        clearInterval(this.interval);
    }

    tick = () => {
        this.setState({
            pollingCount: this.state.pollingCount+1
        });
    }

    render() 
    {
        return this.state.pollingCount;
    }
   */



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

    const updateAccount = async event =>
    {
        var obj = {userID: ud.userID};
        var js2 = JSON.stringify(obj);

        try
        {
            const response = await fetch(bp.buildPath('api/displayhighscore'), {method:'POST', body:js2, headers:{'Content-Type': 'application/json'}}); // await fetch


            var res = JSON.parse(await response.text());

            highScore = res.highscore;

            console.log("Score is "+ highScore);
            console.log(highScore);

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

    /*
        <div id="topGrayRect">
        <a href="/account">Account</a>
        <a href="/account">Friends</a>
        <a href="/leaderboard">Leaderboard</a>
        <button type="button" id="logoutButton" class="buttons" onClick={doLogout}> Log Out </button>
        </div>
    */

    //highScore = updateAccount();
    

    return(

        <div id="AccountPageBody">

            

            <div id = "mainBoxContent">
                Username: {username} <br /><br />
                Email: {email} <br /><br />
                High Score: <a id="btnToRevealScore" onClick= {updateAccount}>  </ a> <br /><br />
            </div>

        </div>
    );
}
export default AccountPageUI;
