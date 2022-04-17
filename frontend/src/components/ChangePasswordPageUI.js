import React, { useState } from 'react';
import './ChangePasswordPageUI.css';
import axios from 'axios';

function ChangePasswordPageUI()
{

    // Retrieve User Data
    var storage = require('../tokenStorage.js');
    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);
    var username = ud.Username;
    var email = ud.email;
    var res;
    var token;

    let bp = require('./Path.js');

    var newPassword;
    var loginPassword;
    const [message,setMessage] = useState('');

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

    const gotoPasswordReset = async event =>
    {
        event.preventDefault();
        try
        {
            window.location.href = '/passwordreset';

        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    };

    const updateAccount = async event =>
    {
        var Username = ud.Username;
        var email = ud.email;
        var userID = ud.userID;

        console.log(res);

        var obj2 = {login:username, password:newPassword.value, email:email, jwtToken:token};

        var js2 = JSON.stringify(obj2);
        console.log(obj2);

        try
        {
            const response2 = await fetch(bp.buildPath('api/editaccount'), {method:'POST', body:js2, headers:{'Content-Type': 'application/json'}}); // await fetch

            console.log(response2);

            var res2 = JSON.parse(await response2.text()); // await response2

            console.log(res2);
            console.log(res2.error);

            if (res2.error !== "")
            {
                setMessage('Error Updating Password: ' + res2.error);
            }
            else
            {
                var user = {Username:Username, email:email, userID:userID};
                localStorage.setItem('user_data', JSON.stringify(user));
                setMessage('Password Updated');
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }
    }

    const doLogin = async event =>
    {
        event.preventDefault();
        var obj = {login:username,password:loginPassword.value};
        var js = JSON.stringify(obj);

        console.log(username.value);
        console.log(loginPassword.value);
        console.log(JSON.stringify(obj));

        axios({
        method: 'post',
        url: bp.buildPath('api/login'),
        headers:
        {
            'Content-Type': 'application/json'
        },
        data: js
        })
        .then(function (response)
        {
            res = response.data;
            if (res.error)
            {
                setMessage('Error: ' + res.error);
            }
            else if (res) // Successful Login
            {
                console.log(res);
                storage.storeToken(res);
                token = storage.retrieveToken();

                //var ud = jwt_decode(token, {header:true});
                var ud = JSON.parse(window.atob(token.split('.')[1]));

                console.log(ud);

                updateAccount();
            }
        })
        .catch(function (error)
        {
            console.log(res);
            console.log(error);
            setMessage('Critical Error: ' + error);
        });


//         try
//         {
//             const response = await fetch(bp.buildPath('api/login'),
//                 {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
//             var res = JSON.parse(await response.text());
//
//             if(res.userID <= 0)
//             {
//                 setMessage('Old Password for ' + username + ' Incorrect');
//             }
//             else
//             {
//                 var obj2 = {login:username, password:newPassword.value, email:email};
//
//                 var js2 = JSON.stringify(obj2);
//
//                 try
//                 {
//                     const response2 = await fetch(bp.buildPath('api/editaccount'), {method:'POST', body:js2, headers:{'Content-Type': 'application/json'}});
//
//                     var res2 = JSON.parse(await response2.text());
//
//                     if (res2.error !== "")
//                     {
//                         setMessage('Error Updating Password');
//                     }
//                     else
//                     {
//                         setMessage('Password Updated');
//                     }
//                 }
//                 catch(e)
//                 {
//                     console.log(e.toString());
//                     return;
//                 }
//             }
//         }
//         catch(e)
//         {
//             console.log(e.toString());
//             return;
//         }
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
        <createLnk id="inner-title" onClick={gotoPasswordReset}>Forgot Password?</createLnk>
     </div>
    );
};
export default ChangePasswordPageUI;
