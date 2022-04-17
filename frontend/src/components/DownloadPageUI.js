import React, { useState } from 'react';
import './DownloadPageUI.css';
const axios = require('axios');

function AccountPageUI()
{

    const gotoRegister = async event =>
    {
        event.preventDefault();
        try
        {
            window.location.href = '/register';

        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    };

    return(

        <div id="AccountPageBody">

        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
            <a id="pls" href="https://drive.google.com/uc?export=download&id=1Wu19waSwREKtmxC_1lSTm_Rl0PtjYElx">
                Download APK for Android
            </a>

        </div>
    );
}
export default AccountPageUI;
