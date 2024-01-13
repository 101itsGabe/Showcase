using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace ShowcaseFullApp.Api;

public class MongoDBApi
{
    public MongoDBApi()
    {
        const string connectionUri = "mongodb+srv://manngabe:<3872810Gabe$$>@cluster0.i5baniq.mongodb.net/?retryWrites=true&w=majority";
        var settings = MongoClientSettings.FromConnectionString(connectionUri);
        
        // Set the ServerApi field of the settings object to Stable API version 1
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        var client = new MongoClient(settings);
        try {
            Console.WriteLine("before ping");
            var result = client.GetDatabase("showcase_db").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
    }
    
}