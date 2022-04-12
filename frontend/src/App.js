import React from 'react';
import { BrowserRouter as Router, Route, Redirect, Switch } from 'react-router-dom';
import './App.css';
import LoginPage from './pages/LoginPage';
import CardPage from './pages/CardPage';
import HomePage from './pages/HomePage';
import RegisterPage from './pages/RegisterPage';
import AccountPage from './pages/AccountPage';
import LeaderboardPage from './pages/LeaderboardPage';
import FriendsPage from './pages/FriendsPage';
import EmailConfirmPage from './pages/EmailConfirmPage';
import ChangePasswordPage from './pages/ChangePasswordPage';
import ChangeEmailPage from './pages/ChangeEmailPage';

function App() {
  return (
    <Router >
      <Switch>

        <Route path="/" exact>
          <HomePage />
        </Route>

        <Route path="/account" exact>
          <AccountPage />
        </Route>

        <Route path="/login" exact>
          <LoginPage />
        </Route>

        <Route path="/register" exact>
          <RegisterPage />
        </Route>

        <Route path="/cards" exact>
          <CardPage />
        </Route>

        <Route path="/leaderboard" exact>
          <LeaderboardPage />
        </Route>

	<Route path="/changepassword" exact>
          <ChangePasswordPage />
        </Route>

        <Route path="/changeemail" exact>
          <ChangeEmailPage />
        </Route>

        <Route path="/friends" exact>
          <FriendsPage />
        </Route>

        <Route path="/emailConfirm" exact>
          <EmailConfirmPage />
        </Route>

        <Redirect to="/" />

      </Switch>  
    </Router>
  );
}
export default App;