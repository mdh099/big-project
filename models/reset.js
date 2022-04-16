
const mongoose = require("mongoose");
const Schema = mongoose.Schema;

//Create Schema
const ResetSchema = new Schema({
    userID: {
        type: Number,
        required: true
    },
    ResetToken: {
        type: String,
        required: true
    }
});

const reset = mongoose.model("Resets", ResetSchema, "Resets");
module.exports = reset;