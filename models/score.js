const mongoose = require("mongoose");
const Schema = mongoose.Schema;

//Create Schema
const ScoreSchema = new Schema({
    userID: {
        type: Number
    },
    Username: {
        type: String,
        required: true
    },
    Score: {
        type: Number,
        required: true
    }
});

const score = mongoose.model("Scores", ScoreSchema, "Scores");
module.exports = score;