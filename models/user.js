const mongoose = require("mongoose");
const Schema = mongoose.Schema;

//Create Schema
const UserSchema = new Schema({
    userID: {
        type: Number
    },
    Username: {
        type: String,
        required: true
    },
    Password: {
        type: String,
        required: true
    },
    email: {
        type: String,
        required: true
    },
    Friends: {
        type: Array,
        required: true
    },
    Scores: {
        type: Array,
        required: true
    }
});

const user = mongoose.model("Users", UserSchema, "Users");
module.exports = user;