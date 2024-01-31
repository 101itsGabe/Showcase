using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;

namespace ShowcaseFullApp.Api
{
    public class MongodbApi
    {
        private MongoClient _dbclient;
        private IMongoDatabase db;
        private IMongoCollection<BsonDocument> _collection;
        public MongodbApi()
        {
            //_dbclient = new MongoClient("mongodb+srv://manngabe:3872810Gabe$$@cluster0.i5baniq.mongodb.net/?retryWrites=true&w=majority");
            //db = _dbclient.GetDatabase("showcase_db");
            //_collection = db.GetCollection<BsonDocument>("users");

            Debug.WriteLine("Starting up Mongodb");
            const string connectionUri = "testcluster-shard-00-00-abcd.mongodb.net:27017,testcluster-shard-00-01-abcd.mongodb.net:27017,testcluster-shard-00-02-abcd.mongodb.net:27017/test?replicaSet=TestCluster-shard-0";
            var mongoUrl = new MongoUrl(connectionUri);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the clien
            // Create a new client and connect to the server
            var client = new MongoClient(mongoUrl);
            // Send a ping to confirm a successful connection
            try
            {
                var databse = client.GetDatabase("showcasedb");
                var result = client.GetDatabase("showcasedb").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Debug.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
