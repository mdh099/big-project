import React, { useState } from 'react';

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

     const doRegister = async event => 
    {
        console.log("ATTEMPTING TO REGISTER");
        event.preventDefault();
        var obj = {login:RegisterName.value, password:RegisterPassword.value, email: RegisterEmail.value};
        var js = JSON.stringify(obj);
        console.log("ATTEMPTING TO GO IN TRY");
        try
        {    
            console.log("IN TRY");
            const response = await fetch(buildPath('api/registration'), {method:'POST',body:js,headers:{'Content-Type': 'application/json'}});

            console.log("AWAITING RESPONSE");

            var res = JSON.parse( await response.text());

            console.log("PAST JSON STUFF");
            if( res.id <= 0 )
            {
                setMessage('User/Password combination incorrect');
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
            <span id="inner-title">PLEASE FILL OUT ALL BOXES</span><br />
            <input type="text" id="RegisterName" placeholder="Username" ref={(c) => RegisterName = c} /><br />
            <input type="text" id="RegisterEmail" placeholder="Email" ref={(c) => RegisterEmail = c} /><br />
            <input type="password" id="RegisterPassword" placeholder="Password" ref={(c) => RegisterPassword = c} />

            <input type="submit" id="RegisterButton" className="buttons" value = "Sign Up" onClick={doRegister} />
        </form>
        <span id="RegisterResult">{message}</span>
     </div>
    );
};
export default Register;