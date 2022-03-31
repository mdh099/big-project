import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import AccountPageUI from '../components/AccountPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const AccountPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <AccountPageUI />
        </div>
    );
}
export default AccountPage;