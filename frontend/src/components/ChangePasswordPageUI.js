import React, { useState } from 'react';
import './ChangePasswordPageUI.css';

function ChangePasswordPageUI()
{

    let bp = require('./Path.js');

    var newPassword;
    var loginPassword;
    const [message,setMessage] = useState('');

    // Retrieve User Data
    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);
    var username = ud.username;
    var email = ud.email;

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

    const doLogin = async event =>
    {
        event.preventDefault();
        var obj = {login:username,password:loginPassword.value};
        var js = JSON.stringify(obj);
        try
        {
            const response = await fetch(bp.buildPath('api/login'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());

            if(res.userID <= 0)
            {
                setMessage('Old Password for ' + username + ' Incorrect');
            }
            else
            {
                var obj2 = {login:username, password:newPassword.value, email:email};

                var js2 = JSON.stringify(obj2);

                try
                {
                    const response2 = await fetch(bp.buildPath('api/editaccount'), {method:'POST', body:js2, headers:{'Content-Type': 'application/json'}});

                    var res2 = JSON.parse(await response2.text());

                    if (res2.error !== "")
                    {
                        setMessage('Error Updating Password');
                    }
                    else
                    {
                        setMessage('Password Updated');
                    }
                }
                catch(e)
                {
                    console.log(e.toString());
                    return;
                }
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

        <form onSubmit={doLogin}>

        <div id = "accInfo">
                Changing password for: {username}
        </div>
        <br/>

        <div class="grid-container">
            <div class="passG">
                <input type="password" id="loginPassword" placeholder="Old Password" ref={(c) => loginPassword = c} />
            </div>
            <div class="passG">
                <input type="password" id="loginPassword" placeholder="New Password" ref={(c) => newPassword = c} />
            </div>
            <div class="emailG">
                <input type="submit" id="loginButton" class="buttons" value = "▷"
                onClick={doLogin} />
            </div>
        </div>

        </form>

        <br/>
        <span id="loginResult">{message}</span>
        <br/>
        <br/>
        <createLnk id="inner-title" onClick={gotoRegister}>Forgot Password?</createLnk>
     </div>
    );
};
export default ChangePasswordPageUI;
