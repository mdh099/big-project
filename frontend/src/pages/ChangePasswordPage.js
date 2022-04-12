import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import ChangePasswordPageUI from '../components/ChangePasswordPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const ChangePasswordPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <ChangePasswordPageUI />
        </div>
    );
}
export default ChangePasswordPage;
