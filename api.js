require('express');
require('mongodb');
require('dotenv').config();

const { findOneAndReplace } = require('./models/user.js');
//Load user model
const User = require("./models/user.js");
const Score = require("./models/score.js");

const twilioClient = require("twilio")(
  process.env.TWILIO_ACCOUNT_SID,
  process.env.TWILIO_AUTH_TOKEN
);

const verificationSID = process.env.TWILIO_VERIFY_SID;

exports.setApp = function(app, client){

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
      userID = results.userID;
      username = results.Username;
      email = results.email;
      isVerified = results.IsVerified; 
    }
    else
    {
      ret = {error:"Login/Password incorrect"};
    }

    var ret = { userID:userID, username:username, email: email, IsVerified: isVerified, error:''};
    res.status(200).json(ret);
  });

  // Registration
  // Change so, it returns an error if that username already exists
  // incoming: Username, Password, email, 
  // (maybe not) outgoing: id, error 
  app.post('/api/registration', async function(req, res, next)
  {
    const { login, password, email } = req.body;

    const newUser = {Username:login, Password:password, email:email, IsVerified: false, EmailToken: null};

    var error = '';

    try
    {
      const result = await User.create(newUser);
    }
    catch(e)
    {
      error = e.toString();
    }

    console.log("Will now create verification code");
    // Create the verification code
    twilioClient.verify
            .services(verificationSID)
            .verifications
            .create({channelConfiguration: {
                    template_id: 'd-db4824d7f6cd4adcab81570907256a41',
                    from: 'ARAsteroids@gmail.com',
                    from_name: 'Team at AR-Asteroids'
                    }, to: email, channel: "email" })
            .then(verification => { console.log(verification.sid);
              // app.get("/verify", (req, res) => {
              //     email: req.query.email; 
              // });
              // res.redirect(/verify?email=${email});
            })
            .catch(error => {
              console.log("Caught error, uncomment next line to see it.");
              console.log(error);
            });

    var ret = { error: error};
    res.status(200).json(ret);
  });

  async function setIsVerified(login){
    console.log("Made it to setIsVerified"); 
    console.log(login);
    var error = '';

    const filter = { Username: login };
    const update = {IsVerified: true};

    // doc is the document before update was applied
    let doc = await User.findOneAndUpdate(filter, update);
    doc.IsVerified;

    doc = await User.findOne(filter);
    doc.IsVerified; 

    var ret = { error: '' };

    console.log("Reach end of setIsVerified function!"); 
  }

  app.post("/api/verify", (req, res) => {
    // incoming: code, email, login
    // outgoing: error
    var error = ""; 
    var ret;

    const userCode = req.body.code;
    const email = req.body.email;
    const Username = req.body.login; 

    console.log(`Code: ${userCode}`);
    console.log(`Email: ${email}`);

    // DO NOT MESS WITH
    // RE-comment this whole block of code if something stops working again. 
    twilioClient.verify
      .services(verificationSID)
      .verificationChecks.create({ to: email, code: userCode })
      .then(verification_check => {
          if (verification_check.status === "approved") {
            console.log("Verification succeeded");   // For testing purposes 
            error = ""; 
            ret = {error: ""};
            setIsVerified(req.body.login); 
          }
          else{
              console.log("-Caught error-. Greetings from else statement.");
              console.log('Verification failed');
              ret = {error: "verification failed"};
              error = "verification failed";
          }
      })
      .catch(error => {
        console.log("Caught error");
        error = "verification failed";
      }); 

      ret = {error: error};
      res.status(200).json(ret);
  });

  app.post('/api/editaccount', async (req, res, next) => 
  {
    var error = '';
  
    const { login, password, email } = req.body;
  
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
  
    var ret = { error: '' };
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

// Add Friend
// Incoming: userID(current user), friendID(friend to add)  
// Outgoing: error message
app.post('/api/addfriend', async function(req, res, next)  
{
  // Gets input JSON
  const { userID, friendID } = req.body;

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

  // Return with error message
  var ret = { error };
  res.status(200).json(ret);
});

  // Delete Friend
  // Incoming: userID(current user logged in), friendID(friend to delete)
  // Outgoing: error message
  app.post('/api/deletefriend', async function(req, res, next)
  {
    // Gets input JSON
    const { userID, friendID } = req.body;

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

    // Return with error message
    var ret = { error };
    res.status(200).json(ret);
  });

  // Search for friends to add
  // Incoming: userID(current user logged in)
  // Outgoing: users(list of all possible accounts for user to befriend), error message
  app.post('/api/searchnewfriends', async function(req, res, next)
  {
    // Gets input JSON
    const { userID } = req.body;

    // Initializes error
    var error = '';

    // Finds the current user
    const currentUser = await User.findOne( {userID: userID} );

    // Creates an array of the user and their friends
    const friendsPlusUser = currentUser.Friends;
    friendsPlusUser.push(userID);

    // Finds all the users except for the current user and their friends
    const users = await User.find({ userID: { $nin: friendsPlusUser} });

    // Return with error message
    var ret = {users, error};
    res.status(200).json(ret);
  });

  // Search current friends
  // incoming: userID(current user logged in)
  // outgoing: users(current user's friends), error message
  app.post('/api/searchcurrentfriends', async function(req, res, next)
  {
    // Gets input JSON
    const { userID } = req.body;

    // Initializes error
    var error = '';

    // Finds the current users
    const currentUser = await User.findOne( {userID: userID} );

    // Finds all of the current user's friends
    const currentFriends = await User.find( {userID: {$in: currentUser.Friends} } );

    // Return with error message
    var ret = {currentFriends, error};
    res.status(200).json(ret);
  });

  // Add new score
  // Incoming: userID, score
  // Outgoing: N/A
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

    /*
    if(score > currentUser.Highscore){
        const filter = { userID:userID };
        const update = { Highscore:score };
        await User.findOneAndUpdate(filter, update);
    } 
    */

    // Return with error message
    var ret = {error};
    res.status(200).json(ret);
  });

  // Show local leaderboard
  // Incoming: userID(current user logged in)
  // Outgoing: username and scores of the current user every user that the current user is friends with
  app.post('/api/showlocalleaderboard', async function(req, res, next)  
  {
    // Gets input JSON
    const { userID } = req.body;

    // Initializes error
    var error = '';

    // Finds the current user
    const currentUser = await User.findOne( {userID: userID} );

    // Creates an array of the user and their friends
    const friendsPlusUser = currentUser.Friends;
    friendsPlusUser.push(userID);

    // Finds the scores of the current user and their friends
    const localScores = await Score.find( {userID: {$in: friendsPlusUser}} ).sort( {Score: -1 } );

    // Return with error message
    var ret = {localScores, error};
    res.status(200).json(ret);
  });

  // Show global leaderboard
  // Incoming: N/A
  // Outgoing: username and scores of every user
  app.post('/api/showgloballeaderboard', async function(req, res, next)
  {
    // Initializes error
    var error = '';

    // Finds every score
    const globalScores = await Score.find().sort( {Score: -1 } );

    // Return with error message
    var ret = {globalScores, error};
    res.status(200).json(ret);
  });
}