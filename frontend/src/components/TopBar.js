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
        <button><a href="/download" class="download">Download</a></button>
        <div class="account">
          <button>Account</button>
          <ul>
            <li><a href="/account">View Account</a></li>
            <li><a href="/changeemail">Change Email</a></li>
            <li><a href="/changepassword">Change Password</a></li>
            <li><a href="/deleteaccount">Delete Account</a></li>
          </ul>
        </div>
        <div class="leaderboard">
          <button>Leaderboards</button>
          <ul>
            <li><a href="/globalleaderboard">All Users</a></li>
            <li><a href="/friendleaderboard">Friends</a></li>
            <li><a href="/personalleaderboard">Personal Best</a></li>
          </ul>
        </div>
        <button><a href="/friends" class="friends">Friends</a></button>
        <button><a href="/" class="logout">Log Out</a></button>
      </div>
    </nav>
    );
}
export default TopBar;
