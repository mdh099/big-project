import React, { useState } from 'react';
import '../Login.css';
import '../emailConfirm.css';
import Register from './Register.js';

function Login()
{

    let bp = require('./Path.js');

    var Name;
    var Email;
    var Code;
    const [message,setMessage] = useState('');
    const app_name = 'cop4331-123';

    var _ud = localStorage.getItem('user_data');
    var ud = JSON.parse(_ud);

    var verificationCode;

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

    const goToLogin = async event =>
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

    const doVerify = async event => 
    {

        event.preventDefault();
        var obj = {code:Code.value, email:Email.value};

        var js = JSON.stringify(obj);
        try
        {    
            const response = await fetch(buildPath('api/verify'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});

            var res = JSON.parse(await response.text());

            if( res.error !== "" || obj.code == "" || obj.password == "" || obj.email == "")
            {
                if (obj.code == "" || obj.password == "" || obj.email == "")
                {
                    setMessage('Please Fill out All Fields');
                }
                else 
                    setMessage('Error: ' + res.error);
            }
            else
            {
                setMessage('');
                window.location.href = '/login';
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    

    };

//     const resendCode = async event =>
//     {
//         event.preventDefault();
//         var obj = {login:Name.value, email: Email.value};
//
//         var js = JSON.stringify(obj);
//         try
//         {
//             const response = await fetch(buildPath('api/registration'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});
//
//             var res = JSON.parse(await response.text());
//
//             if (obj.login == "" || obj.email == "" || )
//             {
//                 setMessage('Please Fill out All Fields');
//             }
//
//             else
//             {
//                 setMessage('');
//                 window.location.href = '/emailConfirm';
//             }
//         }
//         catch(e)
//         {
//             console.log(e.toString());
//             return;
//         }
//     };

//             <div class="verifyEmailBtnDiv">
//               <button id="resendCodeBtn" type="button" onClick={resendCode}>Resend Code</button>
//             </div>

//                 <div class="emailNameDiv">
//                 <input type="input" id="codeInput"  placeholder="Enter Username"
//                     ref={(c) => Name = c} />
//              </div>

    return(
      <div id="emailConfirmDiv">

        <div id="enterCodeText">
            <label>Please Verify Your Account</label>
        </div>

        <form id="emailVerifyForm">
            
             <div class="emailMailDiv">
                <input type="input" id="codeInput"  placeholder="Enter Email" 
                    ref={(c) => Email = c} />
             </div>
             
             <br/>

             <div class="emailCodeDiv">
                <input type="input" id="codeInput"  placeholder="Enter Code" 
                    ref={(c) => Code = c} />
             </div>

         </ form>

         <span id="loginResult">{message}</span>

         <div id="leavesomespacepls">
            <div class="verifyEmailBtnDiv">
              <button id="verifyEmailBtnID" type="button" onClick={doVerify}>Verify</button>
            </div>

            <div class="verifyEmailBtnDiv">
              <button id="goToRegisterBtn" type="button" onClick={gotoRegister}> Back to Register</button>
            </div>

            <div class="verifyEmailBtnDiv">
              <button id="goToLoginBtn" type="button" onClick={goToLogin}> Go to Login</button>
            </div>

        </div>
        
     </div>
    );
};
export default Login;
