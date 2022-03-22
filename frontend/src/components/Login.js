import React, { useState } from 'react';
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
                setMessage('User/Password combination incorrect');
            }
            else
            {                                             // Changed res.id to res.userID
                var user = {username:res.username,id:res.userID, email:res.email}
                // added next line for testing
                console.log(user);
                localStorage.setItem('user_data', JSON.stringify(user));
                setMessage('');
                window.location.href = '/cards';
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
        <span id="inner-title">PLEASE SIGN IN</span><br />
        <input type="text" id="loginName" placeholder="Username" ref={(c) => loginName = c} /><br />
        <input type="password" id="loginPassword" placeholder="Password" ref={(c) => loginPassword = c} />

        <input type="submit" id="loginButton" class="buttons" value = "Do It"
          onClick={doLogin} />
        </form>
        <span id="loginResult">{message}</span>
     </div>
    );
};
export default Login;