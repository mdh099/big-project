using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class DatabaseAccess : MonoBehaviour
{
    /*
    MongoClient client = new MongoClient("mongodb + srv://asteroidRoot:ztyXeYpqyD65IUId@asteroidscluster.q4aw1.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    int testScore = 420;
    string userName = "Radir";
    string password = "7f943921724d63dc0ac9c6febf99fa88";


    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("Asteroids");
        collection = database.GetCollection<BsonDocument>("NameOfThing");

        //temp for testing Watch 13:50
        GetScoresFromDataBase();
    }

    public async void testLogin(string userName, string password)
    {
        var 
    }

    public async void SaveScoreToDataBase(string userName, int score)
    {
        var document = new BsonDocument { { userName, score } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;

        List<HighScore> highScores = new List<HighScore>();
        foreach(var score in scoresAwaited.ToList())
        {
            highScores.Add(Deserialize(score.ToString()));
        }

        return highScores;
    }

    private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();

        return highScore;
    }
    */
}

/*
//inline class
public class HighScore
{
    public string UserName { get; set; }
    public int Score { get; set; }
}*/
