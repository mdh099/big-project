require('express');
require('mongodb');

const { findOneAndReplace } = require('./models/user.js');
//Load user model
const User = require("./models/user.js");

exports.setApp = function(app, client){

  // Login
  // Complete, returns userID and email. 
  app.post('/api/login', async function(req, res, next)  
  {
    // incoming: login, password
    // outgoing: id, firstName, lastName, error
	
    var error = '';

    const { login, password } = req.body;
    
    // Old mongodb query
    //const db = client.db();
    //const results = await db.collection('Users').find({Username:login,Password:password}).toArray();

    const results = await User.findOne({ Username: login, Password: password });

    var userID = -1;
    var username = '';
    var email = '';

    if(results)
    {
      userID = results.userID;
      username = results.Username;
      email = results.email;
      // used to be userID = results[0]._id;
    }
    else
    {
      ret = {error:"Login/Password incorrect"};
    }

    var ret = { userID:userID, username:username, email: email, error:''};
    res.status(200).json(ret);
  });

  // Registration
  app.post('/api/registration', async function(req, res, next)  
  {
    // incoming: Username, Password, email, 
    // (maybe not) outgoing: id, error 
    const { login, password, email } = req.body;

    const newUser = {Username:login, Password:password, email:email};
  
    // var userID = -1;
    var error = '';

    try
    {
      // Old mongodb query
      //const db = client.db();
      //const result = db.collection('Users').insertOne(newUser);

      const result = await User.create(newUser);
    }
    catch(e)
    {
      error = e.toString();
    }

    var ret = { error: error/*, userID:userID*/};
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
app.post('/api/addfriend', async function(req, res, next)  
{
  // incoming: userID(current user), friendID(friend to add)
  // outgoing: friendID(added friend), error message
  const { userID, friendID } = req.body;

  var error = '';

  const currentUser = await User.findOne({ userID:userID });
  const friendToAdd = await User.findOne({ userID:friendID });

  currentUser.Friends.push(friendID);
  currentUser.save();

  var ret = { friendID:friendID, error:''};
  res.status(200).json(ret);
});

// Delete Friend
app.post('/api/deletefriend', async function(req, res, next)
{
  // incoming: userID(current user logged in), friendID(friend to delete)
  // outgoing: friendID(deleted friend), error message
  
  const { userID, friendID } = req.body;

  var error = '';

  await User.findOneAndUpdate({userID:userID}, {$pull: {Friends:friendID}});

  var ret = { friendID:friendID, error:''};
  res.status(200).json(ret);
});

// Search for friends to add
app.post('/api/searchnewfriends', async function(req, res, next)
{
  // incoming: userID(current user logged in)
  // outgoing: users(list of all possible accounts for user to befriend), error message

  const { userID } = req.body;

  var error = '';

  const currentUser = await User.findOne( {userID: userID} );

  const friendsPlusUser = currentUser.Friends;
  friendsPlusUser.push(userID);

  const users = await User.find({ userID: { $nin: friendsPlusUser} });

  var ret = {users, error:''};
  res.status(200).json(ret);
});

// Search current friends
app.post('/api/searchcurrentfriends', async function(req, res, next)
{
  // incoming: userID(current user logged in)
  // outgoing currentFriends(user's friends), error message
  const { userID } = req.body;

  var error = '';

  const currentUser = await User.findOne( {userID: userID} );

  const currentFriends = await User.find( {userID: {$in: currentUser.Friends} } );

  var ret = {currentFriends, error:''};
  res.status(200).json(ret);
});






// Olds cards apis

app.post('/api/searchcards', async (req, res, next) => 
{
  // incoming: userId, search
  // outgoing: results[], error
  var error = '';
  const { userId, search } = req.body;
  var _search = search.trim();
  
  const db = client.db();
  const results = await db.collection('Cards').find({"Card":{$regex:_search+'.*', $options:'r'}}).toArray();
  
  

  var _ret = [];
  for( var i=0; i<results.length; i++ )
  {
    _ret.push( results[i].Card );
  }
  
  var ret = {results:_ret, error:error};
  res.status(200).json(ret);
  });
app.post('/api/addcard', async (req, res, next) =>
  {
    // incoming: userId, color
    // outgoing: error
    const { userId, card } = req.body;
    const newCard = {Card:card,UserId:userId};
    var error = '';
    try
    {
      const db = client.db();
      const result = db.collection('Cards').insertOne(newCard);
    }
    catch(e)
    {
      error = e.toString();
    }
    var ret = { error: error };
    res.status(200).json(ret);
  });
}