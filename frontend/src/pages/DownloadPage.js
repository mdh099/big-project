import React from 'react';
import TitleLoggedIn from '../components/TitleLoggedIn';
import LoggedInName from '../components/LoggedInName';
import DownloadPageUI from '../components/DownloadPageUI';
import TopBar from '../components/TopBar';

/*
    <PageTitle />
    <LoggedInName />
    <AccountPageUI />
*/

const DownloadPage = () =>
{
    return(
        <div>
            <TitleLoggedIn />
            <TopBar />
            <DownloadPageUI />
        </div>
    );
}
export default DownloadPage;
