import React, { useState } from 'react';
import './PasswordResetPageUI.css';

function PasswordResetPageUI()
{
    var accountemail;
    var response2;
    const [message,setMessage] = useState('');

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
        event.preventDefault();
        var obj = {email:accountemail.value};

        var js = JSON.stringify(obj);
        try
        {
            const response = await fetch(buildPath('api/forgotpassword'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});

            var res = JSON.parse(await response.text());

            console.log(res.error);
            console.log(res.message);

            if( res.error !== "" )
            {
                if (obj.email == "")
                {
                    setMessage('Please enter account Email');
                }
                else
                {
                    setMessage('No account with that email has been found');
                }
            }

            if (res.message !== "")
            {
                setMessage('Success! Check your email.');
                //window.location.href = '/';
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    };

    return(
      <div id="changePasswordDiv">

        <form onSubmit={passwordReset}>
        <br/>
        <br/>
        <div id = "accInfo">
                Enter your account email:
        </div>
        <br/>

        <div class="grid-container">
            <div class="passG">
                <input type="text" id="loginPassword" placeholder="Account Email" ref={(c) => accountemail = c} />
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
        <loginLink id="gotoVerifyLink" onClick={gotoLogin} >Remember your password?</loginLink>

     </div>
    );
};

export default PasswordResetPageUI;
