import React from 'react';
import './TopBar.css';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const TopBar = () =>
{

    const doLogout = event => 
    {
        event.preventDefault();
        localStorage.removeItem("user_data")
        window.location.href = '/';
    };  

    return(
        <nav>
      <div class="dropdown">
        <button id="download"><a href="/download" class="download">Download</a></button>

        <div class="account">
          <button id="btnforAccountDrop">Account</button>

          <ul>
            <li><a href="/account">View Account</a></li>
            <li><a href="/changeemail">Change Email</a></li>
            <li><a href="/changepassword">Change Password</a></li>
            <li><a href="/deleteaccount">Delete Account</a></li>
          </ul>

        </div>

        <button id="btnforboards"><a href="/leaderboard" class="leaderboard">Leaderboards</a></button>



        <button id="friends"><a href="/friends" class="friends">Friends</a></button>

        <button id="logout" onClick={doLogout}><a href="/" class="logout">Log Out</a></button>
      </div>

    </nav>
    );
}
export default TopBar;
