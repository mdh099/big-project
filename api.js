require('express');
require('mongodb');

exports.setApp = function(app, client){
    // Adding Login API
// Complete, returns userID and email. 
app.post('/api/login', async (req, res, next) => 
{
  // incoming: login, password
  // outgoing: id, firstName, lastName, error
	
 var error = '';

  const { login, password } = req.body;

  const db = client.db();
  const results = await db.collection('Users').find({Username:login,Password:password}).toArray();

  var userID = -1;
  var username = '';
  var email = '';

  if( results.length > 0 )
  {
    userID = results[0].userID;
    username = results[0].Username;
    email = results[0].email;
    // used to be userID = results[0]._id;
  }

  var ret = { userID:userID, username:username, email: email, error:''};
  res.status(200).json(ret);
});

// Adding registration API
app.post('/api/registration', async (req, res, next) => 
{
  // incoming: Username, Password, email, 
  // (maybe not) outgoing: id, error 
  const { login, password, email } = req.body;

  const newUser = {Username:login, Password:password, email:email};
  
  // var userID = -1;
  var error = '';

  try
  {
    const db = client.db();
    const result = db.collection('Users').insertOne(newUser);
  }
  catch(e)
  {
    error = e.toString();
  }

  var ret = { error: error/*, userID:userID*/};
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

// Not complete. 
// app.post('/api/editaccount', async (req, res, next) => 
// {
//   // incoming: Username, Password, email, 
//   // (maybe not) outgoing: id, error 
//   const { login, password, email } = req.body;
  
//   // Currently does not allow you to change your username. Only email, and password. 
//   // var username = '';
//   var newEmail = email;
//   var newPassword = password;

//   // const newUser = {Username:login, Password:password, email:email};
//   const updatedUser = {Username:login, Password:password, email:email}; 
  
//   // var userID = -1;
//   var error = '';

//   try 
//   {
//     const db = client.db();
//     const result = await db.collection('Users').find({Username:login,Password:password}).toArray();
//     result = updatedUser; 
//   }
//   catch(e)
//   {
//     error = e.toString();
//   }

//   if( results.length > 0 )
//   {
//     userID = results[0].userID;
//     username = results[0].Username;
//     email = results[0].email;
//     // used to be userID = results[0]._id;
//   }

//   var ret = { userID:userID, username:username, email: email, error:''};
//   res.status(200).json(ret);

//   var ret = { error: error/*, userID:userID*/};

//   // Now it neads to 
//   res.status(200).json(ret);
// });
//

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
}