import React, { useState, useCallback } from 'react';
import '../Login.css';
import axios from 'axios';
import jwt_decode from "jwt-decode";

function Login()
{
    let bp = require('./Path.js');
    var storage = require('../tokenStorage.js');

    var loginName;
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

    const doLogin = async event =>
    {
        event.preventDefault();
        var obj = {login:loginName.value,password:loginPassword.value};
        var js = JSON.stringify(obj);

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
            var res = response.data;
            if (res.error)
            {
                setMessage('Error: ' + res.error);
            }
            else
            {
                storage.storeToken(res);
                var token = storage.retrieveToken();

                var ud = jwt_decode(token, {header:true});

                var username = ud.payload.username;
                var email = ud.payload.email;
                var userID = ud.payload.userId;

                setMessage('Logged in as ' + ud.payload.userId);

                var user = {username:username, email:email, userID:userID};
                localStorage.setItem('user_data', JSON.stringify(user));
                window.location.href = '/account';
            }
        })
        .catch(function (error)
        {
            console.log(error);
            setMessage('Error: ' + error);
        });

//         try
//         {
//             console.log("here1");
//             const response = await fetch(bp.buildPath('api/login'),
//                 {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
//             var res = JSON.parse(await response.text());
//
//             console.log(res);
//             //---------------------------------------------------------------------------
//             if(res.userID <= 0 || !res.IsVerified)
//             {
//                 setMessage('Login Failed -- Is your account verified? Check your email!');
//             }
//             else
//             {                                             // Changed res.id to res.userID
//                 storage.storeToken(res); // JWT
//
//                 let userID = res.userID;
//                 let Username = res.Username;
//                 let email = res.email;
//
//                 var user = {username:res.Username,id:res.userID, email:res.email}
//                 // added next line for testing
//                 console.log(user);
//                 localStorage.setItem('user_data', JSON.stringify(user));
//                 setMessage('');
//                 window.location.href = '/account';
//             }
//             //------------------------------------------------------------------------
//         }
//         catch(e)
//         {
//             console.log(e.toString());
//             return;
//         }
    };

    return(
      <div id="loginDiv">

        <form onSubmit={doLogin}>

        <div class="grid-container">
            <div class="userG">
                <input type="text" id="loginName" placeholder="Username" ref={(c) => loginName = c} />
            </div>
            <div class="passG">
                <input type="password" id="loginPassword" placeholder="Password" ref={(c) => loginPassword = c} />
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
        <createLnk id="inner-title" onClick={gotoRegister}>Create Account</createLnk>
        <br/>
        <br/>
        <resetLnk id="inner-title">Reset Password</resetLnk>
     </div>
    );
};
export default Login;
