require('express');
require('mongodb');
require('dotenv').config();

var token = require('./createJWT.js');

const { findOneAndReplace } = require('./models/user.js');
//Load models
const User = require("./models/user.js");
const Score = require("./models/score.js");
const Reset = require("./models/reset.js");
const sgMail = require('@sendgrid/mail');

sgMail.setApiKey(process.env.SENDGRID_API_KEY);

const twilioClient = require("twilio")(
  process.env.TWILIO_ACCOUNT_SID,
  process.env.TWILIO_AUTH_TOKEN
);

const sendEmail = (reciever, id) => {
  try {
    const link = `https://cop4331-123.herokuapp.com/resetpassword/${id}`; 
    const data = {
      to: reciever,
      from: 'ARAsteroids@gmail.com', 
      subject: 'AR Asteroids Password Reset Request',
      html:       
      `
      <div>Reset your password by clicking the time-sensitive link below. Don't worry, it's valid for 24 hours! </div><br/>
      <div>${link}</div>
      <div>
          - From the team at AR Asteroids
      `
    };
    return sgMail.send(data); 
  } catch (e) {
      console.log(e);
      return new Error(e);
  }
};

const verificationSID = process.env.TWILIO_VERIFY_SID;

exports.setApp = function(app, client){

  app.post('/api/forgotpassword', async function(req, res, next){
    var error = '';

    const { email } = req.body;

    const results = await User.findOne({ email: email });

    if(!results){
      console.log("User not found!");
      return res.status(404).json(ret = { error: "user not found"});
    } else {
      console.log("valid email address"); 
      console.log("Username: " + results.Username); 
    }

    const token = require("./createJWT.js");

    const test = token.createToken(results.Username, results.email, results.userID);
    const id = test.accessToken; 

    // Insert ResetTokens into the DB, along with userID of the reqester. 
    const newReset = { userID: results.userID, ResetToken: id};

    try
    {
      const result = await Reset.create(newReset);
      console.log(result); 
    }
    catch(e)
    {
      error = e.toString();
      console.log(error); 
    }

    await sendEmail(email, id)
    
    return res.status(200).send({ message: "Email Successfully sent to your inbox."});
  });

  app.post('/api/verifyreset', async function(req, res, next)
  {
    var error = '';

    const { resetToken, password } = req.body;

    try
    {
      if( token.isExpired(resetToken))
      {
        var r = {error:'The JWT is no longer valid', resetToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }

    const results = await Reset.findOne({ ResetToken: resetToken});

    var userID = -1;
    var isVerified = false;

    if(results)
    {
        userID = results.userID;
        console.log("UserID: " + userID); 
    }
    else
    {
      var ret = {error:"Cannot not determine the validity of link, please request new link."};
      res.status(200).json(ret);
      return;
    } 

    const userResults = await User.findOne({ userID: userID});

    if(!userResults){
      console.log("account not found"); 
      var r = {error:'Account not located in DB, please try again'};
      res.status(200).json(r);
      return;
    }

    // Now that we have confirmed the user, reset the new password they provided.
    const filter = { userID: userID };
    const update = {Password: password};

    let doc = await User.findOneAndUpdate(filter, update);
    doc.Password;
  
    doc = await User.findOne(filter);
    doc.Password; 

    Reset.findOneAndDelete({ResetToken: resetToken}, function (err, docReset) {
      if (err){
        console.log(err)
      } else {
        console.log("Deleted old request in DB"); 
      }
    });

    ret = { error: error}; 
    res.status(200).json(ret);
  });

  // Login
  // incoming: login, password
  // outgoing: id, firstName, lastName, error
  app.post('/api/login', async function(req, res, next)
  {
    var error = '';

    const { login, password } = req.body;

    const results = await User.findOne({ Username: login, Password: password });

    var userID = -1;
    var username = '';
    var email = '';
    var isVerified = false;

    if(results)
    {
      if(results.IsVerified){
        userID = results.userID;
        username = results.Username;
        email = results.email;
        isVerified = results.IsVerified;

        try
        {
          const token = require("./createJWT.js");
          ret = token.createToken( username, email, userID );
        }
        catch(e)
        {
          ret = {error:e.message};
        }

        res.status(200).json(ret);
      }
      else{
        var ret = { userID:userID, username:username, email: email, IsVerified: isVerified, error:'Account is not verified.'};
        res.status(200).json(ret);
      }
    }
    else
    {
      ret = {error:"Login/Password Incorrect"};
      res.status(200).json(ret);
    } 
  });

  // Registration
  // Change so, it returns an error if that username already exists
  // incoming: Username, Password, email, 
  // (maybe not) outgoing: id, error 
  app.post('/api/registration', async function(req, res, next)
  {
    var error = '';

    const { login, password, email } = req.body;

    const newUser = {Username:login, Password:password, email:email, IsVerified: false, EmailToken: null};

    // Checks if provided email is already used in an account. 
    //--------------------------------------------------
    const emailResults = await User.findOne({ email: email}); 

    if(emailResults){
      error = "Email in use"; 
    }
    //------------------------------------------------------

    if(!error){
      // Changed catch to return an error if it gets the duplicate account message
      try
      {
        const result = await User.create(newUser);
      }
      catch(e)
      {
        error = e.toString();
        console.log(error);
      }
    }

    if(!error){
      console.log("Will now create verification code");
      // Create the verification code
      twilioClient.verify
            .services(verificationSID)
            .verifications
            .create({channelConfiguration: {
                    template_id: 'd-db4824d7f6cd4adcab81570907256a41',
                    subject: 'Verification code',
                    from: 'ARAsteroids@gmail.com',
                    from_name: 'Team at AR-Asteroids'
                    }, to: email, channel: "email" })
            .then(verification => { console.log(verification.sid);
            })
            .catch(error => {
              console.log("Caught error, uncomment next line to see it.");
              console.log(error);
            });
    }

    var ret = { error: error};
    res.status(200).json(ret);
  });

  async function setIsVerified(login){
    console.log(login);
    var error = '';

    const filter = { Username: login };
    const update = {IsVerified: true};

    // doc is the document before update was applied
    let doc = await User.findOneAndUpdate(filter, update);
    //doc.IsVerified;

    doc = await User.findOne(filter);
    //doc.IsVerified; 

    var ret = { error: '' };
  }

  app.post("/api/verify", (req, res) => {
    // incoming: code, email, login
    // outgoing: error
    var error = ""; 
    var ret;

    const userCode = req.body.code;
    const email = req.body.email;
    // const Username = req.body.login; 

    console.log(`Code: ${userCode}`);
    console.log(`Email: ${email}`);

    // DO NOT MESS WITH
    // RE-comment this whole block of code if something stops working again. 
    twilioClient.verify
      .services(verificationSID)
      .verificationChecks.create({ to: email, code: userCode })
      .then(verification_check => {
          if (verification_check.status === "approved") {
            console.log("Verification succeeded");
            setIsVerified(req.body.login); 

            ret = {error: ''};
            res.status(200).json(ret);
          }
          else{            
            ret = {error: 'Invalid verification code.'};
            res.status(200).json(ret);
          }
      })
      .catch(error => {
        console.log("Caught error");
        ret = {error: 'Something went wrong, please try again shortly.'};
        res.status(200).json(ret);
      }); 
  });

  app.post('/api/editaccount', async (req, res, next) => 
  {
    var error = '';

    const { login, password, email, jwtToken } = req.body;

    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }
  
    const filter = { Username: login };
    const update = {Password: password, email: email};
  
    // // doc is the document before update was applied
    let doc = await User.findOneAndUpdate(filter, update);
    doc.Username; 
    doc.Password;
    doc.email;
  
    doc = await User.findOne(filter);
    doc.Password; 
    doc.email;

    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }
  
    var ret = { error: '', jwtToken: refreshedToken  };
    res.status(200).json(ret);
  });
  
  app.post('/api/deleteaccount', async function(req, res, next)
    {
      // incoming: userID
      // outgoing: error
  
      var error = '';
  
      const { userID } = req.body;
  
      User.findOneAndDelete({userID: userID}, function (err, doc) {
        if (err){
          console.log(err)
        }
        else{
          //console.log("Deleted account"); 
        }
      });
  
      var ret = { error: '' };
      res.status(200).json(ret);
    });

// Display Highscore
// Incoming: userID(current user) 
// Outgoing: highscore, error message
app.post('/api/displayhighscore', async function(req, res, next)  
{
  // Gets input JSON
  const { userID } = req.body;

  // Initializes highscore and error
  var highscore = 0;
  var error = '';

  // Attempts to find the current user
  const currentUser = await User.findOne({ userID:userID });

  // If-else statement to catch errors
  if(currentUser){ // Error if current user doesn't exist
    highscore = currentUser.Highscore;
  }else{
    error = 'Error. Current user does not exist.';
  }

  // Return with error message
  var ret = { highscore: highscore, error: error };
  res.status(200).json(ret);
});

// Add Friend
// Incoming: userID(current user), friendID(friend to add), jwtToken  
// Outgoing: error message
app.post('/api/addfriend', async function(req, res, next)  
{
  // Gets input JSON
  const { userID, friendID, jwtToken } = req.body;

  // Checks if JWT is expired
  try
  {
    if( token.isExpired(jwtToken))
    {
      var r = {error:'The JWT is no longer valid', jwtToken: ''};
      res.status(200).json(r);
      return;
    }
  }
  catch(e)
  {
    console.log(e.message);
  }

  // Initializes error
  var error = '';
  
  // Attempts to find the current user and the friend to be added
  const currentUser = await User.findOne({ userID:userID });
  const friendToAdd = await User.findOne({ userID:friendID });

  // If-else statement to catch errors
  if(currentUser){ // Error if current user doesn't exist
    if(friendToAdd){ // Error if friend to add doesn't exist
      if(currentUser.Friends.includes(friendID)){ // Error if user is already friends with friend to be added
        error = 'Error. Already friends.';
      }else{
        currentUser.Friends.push(friendID);
        currentUser.save();  
      }
    }else{
      error = 'Error. Friend does not exist.';
    }
  }else{
    error = 'Error. Current user does not exist.';
  }

  // Refreshes JWT
  var refreshedToken = null;
  try
  {
    refreshedToken = token.refresh(jwtToken);
  }
    catch(e)
  {
    console.log(e.message);
  }

  // Return with error message
  var ret = { error: error, jwtToken: refreshedToken };
  res.status(200).json(ret);
});

  // Delete Friend
  // Incoming: userID(current user logged in), friendID(friend to delete), jwtToken 
  // Outgoing: error message
  app.post('/api/deletefriend', async function(req, res, next)
  {
    // Gets input JSON
    const { userID, friendID, jwtToken } = req.body;

    // Checks if JWT is expired
    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
    console.log(e.message);
    }

    // Initializes error
    var error = '';

    // Attempts to find the current user
    const currentUser = await User.findOne({ userID:userID });

    // If-else statement to catch errors
    if(currentUser){ // Error if current user doesn't exist
      if(currentUser.Friends.includes(friendID)){ // Error if user isn't friends with the on trying to be deleted
        await User.findOneAndUpdate({userID:userID}, {$pull: {Friends:friendID}});
      }else{
        error = 'Error. Not friends with user trying to be deleted.';
      }
    }else{
      error = 'Error. Current user does not exist.';
    }

    // Refreshes JWT
    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }

    // Return with error message
    var ret = { error: error, jwtToken: refreshedToken };
    res.status(200).json(ret);
  });

  // Search for friends to add
  // Incoming: userID(current user logged in), jwtToken 
  // Outgoing: users(list of all possible accounts for user to befriend), error message
  app.post('/api/searchnewfriends', async function(req, res, next)
  {
    // Gets input JSON
    const { userID, jwtToken } = req.body;

    // Checks if JWT is expired
    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }

    // Initializes error
    var error = '';

    // Finds the current user
    const currentUser = await User.findOne( {userID: userID} );

    // Creates an array of the user and their friends
    const friendsPlusUser = currentUser.Friends;
    friendsPlusUser.push(userID);

    // Finds all the users except for the current user and their friends
    const users = await User.find({ userID: { $nin: friendsPlusUser} });

    // Refreshes JWT
    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }

    // Return with error message
    var ret = {users: users, error: error, jwtToken: refreshedToken};
    res.status(200).json(ret);
  });

  // Search current friends
  // incoming: userID(current user logged in), jwtToken 
  // outgoing: users(current user's friends), error message
  app.post('/api/searchcurrentfriends', async function(req, res, next)
  {
    // Gets input JSON
    const { userID, jwtToken } = req.body;

    // Checks if JWT is expired
    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }

    // Initializes error
    var error = '';

    // Finds the current users
    const currentUser = await User.findOne( {userID: userID} );

    // Finds all of the current user's friends
    const currentFriends = await User.find( {userID: {$in: currentUser.Friends} } );

    // Refreshes JWT
    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }

    // Return with error message
    var ret = {currentFriends: currentFriends, error: error, jwtToken: refreshedToken};
    res.status(200).json(ret);
  });

  // Add new score
  // Incoming: userID, score
  // Outgoing: error
  app.post('/api/addscore', async function(req, res, next)
  {
    // Gets input JSON
    const { userID, score } = req.body;

    // Initializes error
    var error = '';

    // Finds the current users
    const currentUser = await User.findOne( {userID: userID} );

    // Gets the current user's username
    const username = currentUser.Username;

    // Creates a new score for the user
    const newScore = {userID, Username:username, Score:score};

    // Attempts to add the new score to the table
    try
    {
      const result = await Score.create(newScore);
    }
    catch(e)
    {
      error = 'Error adding new score';
    }

    // Return with error message
    var ret = {error: error};
    res.status(200).json(ret);
  });

  // Show local leaderboard
  // Incoming: userID(current user logged in), jwtToken 
  // Outgoing: username and scores of the current user every user that the current user is friends with
  app.post('/api/showlocalleaderboard', async function(req, res, next)  
  {
    // Gets input JSON
    const { userID, jwtToken } = req.body;

    // Checks if JWT is expired
    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }

    // Initializes error
    var error = '';

    // Finds the current user
    const currentUser = await User.findOne( {userID: userID} );

    // Creates an array of the user and their friends
    const friendsPlusUser = currentUser.Friends;
    friendsPlusUser.push(userID);

    // Finds the scores of the current user and their friends
    const localScores = await Score.find( {userID: {$in: friendsPlusUser}} ).sort( {Score: -1 } );

    // Refreshes JWT
    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }

    // Return with error message
    var ret = {localScores: localScores, error: error, jwtToken: refreshedToken};
    res.status(200).json(ret);
  });

  // Show global leaderboard
  // Incoming: jwtToken 
  // Outgoing: username and scores of every user
  app.post('/api/showgloballeaderboard', async function(req, res, next)
  {
    // Gets input JSON
    const { jwtToken } = req.body;

    // Checks if JWT is expired
    try
    {
      if( token.isExpired(jwtToken))
      {
        var r = {error:'The JWT is no longer valid', jwtToken: ''};
        res.status(200).json(r);
        return;
      }
    }
    catch(e)
    {
      console.log(e.message);
    }

    // Initializes error
    var error = '';

    // Finds every score
    const globalScores = await Score.find().sort( {Score: -1 } );

    // Refreshes JWT
    var refreshedToken = null;
    try
    {
      refreshedToken = token.refresh(jwtToken);
    }
      catch(e)
    {
      console.log(e.message);
    }

    // Return with error message
    var ret = {globalScores: globalScores, error: error, jwtToken: refreshedToken};
    res.status(200).json(ret);
  });
}
