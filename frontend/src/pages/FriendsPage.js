import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import FriendsPageUI from '../components/FriendsPageUI';
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