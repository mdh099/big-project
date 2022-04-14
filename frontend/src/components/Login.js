import React, { useState } from 'react';
import '../Login.css';
import axios from 'axios';                        // JWT

function Login()
{

    let bp = require('./Path.js');
    var storage = require('../tokenStorage.js');  // JWT

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
        console.log("IN DOLOGIN");

        event.preventDefault();
        var obj = {login:loginName.value,password:loginPassword.value};
        var js = JSON.stringify(obj);

         var config =
        {
            method: 'post',
            url: bp.buildPath('api/login'),    // JWT
            headers:
            {
                'Content-Type': 'application/json'
            },
            data: js
        };

        axios(config)
            .then(function (response)
        {
            var res = response.data;
            if (res.error)
            {
                setMessage('User/Password combination incorrect');
            }
            else
            {
                storage.storeToken(res);
                var jwt = require('jsonwebtoken');

                var ud = jwt.decode(storage.retrieveToken(),{complete:true});
                var userId = ud.payload.userId;
                var firstName = ud.payload.firstName;
                var lastName = ud.payload.lastName;

                var user = {firstName:firstName,lastName:lastName,id:userId}
                localStorage.setItem('user_data', JSON.stringify(user));
                window.location.href = '/cards';
            }
        })
        .catch(function (error)
        {
            console.log(error);
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
//                 setMessage('Login Failed. Ensure that Your Account is Verified');
//             }
//             else
//             {                                             // Changed res.id to res.userID
//                 var user = {username:res.username,id:res.userID, email:res.email}
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
