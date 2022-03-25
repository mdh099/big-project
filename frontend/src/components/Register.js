import React, { useState } from 'react';
import '../Register.css';

function Register()
{

    var RegisterName;
    var RegisterPassword;
    var RegisterEmail;

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

     const doRegister = async event => 
    {
        event.preventDefault();
        var obj = {login:RegisterName.value, password:RegisterPassword.value, email: RegisterEmail.value};
        var js = JSON.stringify(obj);
        try
        {    
            const response = await fetch(buildPath('api/registration'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});

            var res = JSON.parse( await response.text());

            if( res.id <= 0 )
            {
                setMessage('Incorrect User/Password Combination');
            }
            else
            {
                var user = {firstName:res.firstName,lastName:res.lastName,id:res.id}
                localStorage.setItem('user_data', JSON.stringify(user));
                setMessage('');
                window.location.href = '/';
            }
        }
        catch(e)
        {
            console.log(e.toString());
            return;
        }    

    };
    return(
      <div id="RegisterDiv">
        <form onSubmit={doRegister}>
            <registey id="inner-title">REGISTER</registey><br />

            <br/>
            <div class="grid-container">
                <div class="userG">
                    <input type="text" id="RegisterName" placeholder="Username" ref={(c) => RegisterName = c} />
                </div>
                <div class="userG">
                    <input type="text" id="RegisterEmail" placeholder="Email" ref={(c) => RegisterEmail = c} />
                </div>
                <div class="userG">
                    <input type="password" id="RegisterPassword" placeholder="Password" ref={(c) => RegisterPassword = c} />
                </div>
            </div>
            <br/>

                <input type="submit" id="RegisterButton" className="buttons" value = "▷" onClick={doRegister} />

        </form>
        <span id="RegisterResult">{message}</span>
        <br/>
        <br/>
        <br/>
        <br/>
        <loginLink id="gotoLoginLink" onClick={gotoLogin} >Have an account?</loginLink>
     </div>
    );
};
export default Register;
