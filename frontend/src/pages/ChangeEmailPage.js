import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import ChangeEmailPageUI from '../components/ChangeEmailPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const ChangeEmailPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <ChangeEmailPageUI />
        </div>
    );
}
export default ChangeEmailPage;
