import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import DeleteAccountPageUI from '../components/DeleteAccountPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const DeleteAccountPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <DeleteAccountPageUI />
        </div>
    );
}
export default DeleteAccountPage;
