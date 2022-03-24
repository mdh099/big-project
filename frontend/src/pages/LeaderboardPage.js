import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import LeaderboardPageUI from '../components/LeaderboardPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const LeaderboardPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <LeaderboardPageUI />
        </div>
    );
}
export default LeaderboardPage;