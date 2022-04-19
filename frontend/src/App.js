import React from 'react';
import { BrowserRouter as Router, Route, Redirect, Switch } from 'react-router-dom';
import './App.css';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import RegisterPage from './pages/RegisterPage';
import AccountPage from './pages/AccountPage';
import LeaderboardPage from './pages/LeaderboardPage';
import FriendsPage from './pages/FriendsPage';
import EmailConfirmPage from './pages/EmailConfirmPage';
import ChangePasswordPage from './pages/ChangePasswordPage';
import ChangeEmailPage from './pages/ChangeEmailPage';
import PasswordResetPage from './pages/PasswordResetPage';
import RecoveryEmailPage from './pages/RecoveryEmailPage';
import DeleteAccountPage from './pages/DeleteAccountPage';
import DownloadPage from './pages/DownloadPage';

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

        <Route path="/passwordreset" exact>
          <PasswordResetPage />
        </Route>

        <Route path="/resetpassword">
          <RecoveryEmailPage />
        </Route>

        <Route path="/deleteaccount">
          <DeleteAccountPage />
        </Route>

        <Route path="/download">
          <DownloadPage />
        </Route>

        <Redirect to="/" />

      </Switch>  
    </Router>
  );
}
export default App;
