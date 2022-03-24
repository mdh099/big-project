import React from 'react';
import PageTitle from '../components/PageTitle';
import '../HomePage.css';

const HomePage = () =>
{

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

    const goToRegister = async event => 
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

    return(
      <div>
        <PageTitle />
        
        <div/>
        <input type="button" id="GoToLoginPageBtn" className="buttons" value = "Login" onClick= {goToLogin} />

        <div/>
        <br/>
        <br/>

        <input type="button" id="GoToRegisterPageBtn" className="buttons" value = "Register" onClick= {goToRegister} />

      </div>
    );
};

export default HomePage;
