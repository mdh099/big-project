const jwt = require("jsonwebtoken");
require("dotenv").config();
exports.createToken = function ( Username, email, userID )
{
    return _createToken( Username, email, userID );
}
_createToken = function ( Username, email, userID )
{
    try
    {
      const expiration = new Date();
      const user = {userID:userID,Username:Username,email:email};
      const accessToken =  jwt.sign( user, process.env.ACCESS_TOKEN_SECRET);
      // In order to exoire with a value other than the default, use the
       // following
      /*
      const accessToken= jwt.sign(user,process.env.ACCESS_TOKEN_SECRET,
         { expiresIn: '30m'} );
                       '24h'
                      '365d'
      */
      var ret = {accessToken:accessToken};
    }
    catch(e)
    {
      var ret = {error:e.message};
    }
    return ret;
}
exports.isExpired = function( token )
{
   var isError = jwt.verify( token, process.env.ACCESS_TOKEN_SECRET,
     (err, verifiedJwt) =>
   {
     if( err )
     {
       return true;
     }
     else
     {
       return false;
     }
   });
   return isError;
}
exports.refresh = function( token )
{
  var ud = jwt.decode(token,{complete:true});
  var userID = ud.userID;
  var Username = ud.Username;
  var email = ud.email;
  return _createToken( Username, email, userID );
}
