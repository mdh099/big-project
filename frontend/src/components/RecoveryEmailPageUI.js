import React, { useState } from 'react';
import './PasswordResetPageUI.css';
var md5 = require('md5');

function RecoveryEmailPageUI()
{
    var newPassword;
    var response2;
    const [message,setMessage] = useState('');

    var url;
    var token;

    const app_name = 'cop4331-123'

    function buildPath(route)
    {
        if (process.env.NODE_ENV === 'production')
        {
            return 'https://' + app_name +  '.herokuapp.com/' + route;
        }
        else
        {
            return 'http://localhost:5000/' + route;
        }
    }

    const gotoLogin = async event =>
    {
        event.preventDefault();
        try
        {
            window.location.href = '/login';

        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    };

    const passwordReset = async event =>
    {
        url = window.location.href;

        token = url.substring(url.lastIndexOf('/') + 1);
        console.log(token);

        event.preventDefault();
        var obj = {"resetToken":token, password:md5(newPassword.value)};

        var js = JSON.stringify(obj);
        try
        {
            const response = await fetch(buildPath('api/verifyreset'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});

            var res = JSON.parse(await response.text());

            console.log(res.error);
            console.log(res.message);

            if( res.error !== "" )
            {
                if (obj.newPassword == "")
                {
                    setMessage('Enter a new password');
                }
                else
                {
                    setMessage('Error: ' + res.error);
                }
            }

            if (res.message !== "")
            {
                setMessage('Success!');
                //window.location.href = '/login';
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    };

    return (
        <div id="changePasswordDiv">

        <form onSubmit={passwordReset}>
        <br/>
        <br/>
        <div id = "accInfo">
                Please enter a new password:
        </div>
        <br/>

        <div class="grid-container">
            <div class="passG">
                <input type="password" id="loginPassword" placeholder="New Password" ref={(c) => newPassword = c} />
            </div>
            <div class="emailG">
                <input type="submit" id="loginButton" class="buttons" value = "▷"
                onClick={passwordReset} />
            </div>
        </div>

        </form>

        <br/>
        <br/>
        <span id="loginResult">{message}</span>
        <br/>
        <br/>
        <loginLink id="gotoVerifyLink" onClick={gotoLogin} >Log In</loginLink>

        </div>
    );
};

export default RecoveryEmailPageUI;
