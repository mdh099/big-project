import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import FriendsPageUI from '../components/LeaderboardPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const FriendsPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <FriendsPageUI />
        </div>
    );
}
export default FriendsPage;