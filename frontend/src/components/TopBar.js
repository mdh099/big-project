import React from 'react';

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
        <div id="topGrayRect">
               <a href="/account" title="Account">Account</a>
               <a href="/friends" title="Friends">Friends</a>
               <a href="/leaderboard" title="Leaderboard">Leaderboard</a>
               <button type="button" id="logoutButton" class="buttons" onClick={doLogout} 
               title="Log Out"> Log Out </button>
          </div>
    );
}
export default TopBar;