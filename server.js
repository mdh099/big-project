const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

const app = express();

//app.use(express.json());
//app.use(express.urlencoded());

const PORT = process.env.PORT || 5000; 
app.set('port', (process.env.PORT || 5000));

const path = require('path');  
 
app.use(cors());
app.use(bodyParser.json());


//const MongoClient = require('mongodb').MongoClient;
//const url = 'mongodb+srv://jm:cop4331@cluster0.k1bed.mongodb.net/COP4331?retryWrites=true&w=majority';
require('dotenv').config();
const url = process.env.MONGODB_URI;
const mongoose = require("mongoose");
mongoose.connect(url)
  .then(() => console.log("Mongo DB connected"))
  .catch(err => console.log(err));

var api = require('./api.js');
api.setApp( app, mongoose );

if (process.env.NODE_ENV === 'production') 
    {
      // Set static folder
      app.use(express.static('frontend/build'));
      app.get('*', (req, res) => 
         {
            res.sendFile(path.resolve(__dirname, 'frontend', 'build', 'index.html'));
          });
    }

app.use((req, res, next) => 
{
  res.setHeader('Access-Control-Allow-Origin', '*');
  res.setHeader(
    'Access-Control-Allow-Headers',
    'Origin, X-Requested-With, Content-Type, Accept, Authorization'
  );
  res.setHeader(
    'Access-Control-Allow-Methods',
    'GET, POST, PATCH, DELETE, OPTIONS'
  );
  next();
});
//app.listen(5000); // start Node + Express server on port 5000

app.listen(PORT, () => 
{
  console.log('Server listening on port ' + PORT);
});
