import React, { useState } from 'react';
import '../Login.css';

function Login()
{

    let bp = require('./Path.js');

    var loginName;
    var loginPassword;
    const [message,setMessage] = useState('');

    const doLogin = async event => 
    {
        event.preventDefault();
        var obj = {login:loginName.value,password:loginPassword.value};
        var js = JSON.stringify(obj);
        try
        {    
            const response = await fetch(bp.buildPath('api/login'),
                {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
            var res = JSON.parse(await response.text());
            //---------------------------------------------------------------------------
            if(res.userID <= 0)
            {
                setMessage('Login Failed');
            }
            else
            {                                             // Changed res.id to res.userID
                var user = {username:res.username,id:res.userID, email:res.email}
                // added next line for testing
                console.log(user);
                localStorage.setItem('user_data', JSON.stringify(user));
                setMessage('');
                window.location.href = '/account';
            }
            //------------------------------------------------------------------------
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    
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
            <div class="submG">
                <input type="submit" id="loginButton" class="buttons" value = "▷"
                onClick={doLogin} />
            </div>
        </div>

        </form>

        <br/>
        <br/>
        <br/>
        <span id="loginResult">{message}</span>
        <br/>
        <br/>
        <br/>
        <createLnk id="inner-title">Create Account</createLnk>
        <br/>
        <br/>
        <resetLnk id="inner-title">Reset Password</resetLnk>
        <br/>
        <br/>
     </div>
    );
};
export default Login;
