using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using FireSharp;
using Google.Apis.Auth.OAuth2;

namespace ShowcaseFullApp.Api;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Firestore;

public class FirebaseApi
{
    private readonly IFirebaseConfig _config;
    private readonly IFirebaseClient _client;
    private FirestoreDb _db;
    string projectId = "showcase-ebfee";

    public FirebaseApi()
    {
        FirestoreDbBuilder builder = new FirestoreDbBuilder
        {
            ProjectId = projectId
        };

        GoogleCredential cred;
        using (var stream =
               new FileStream("/ShowcaseFullApp/JsonFile/showcase-ebfee-firebase-adminsdk-14ql0-38e00d703f.json", FileMode.Open, FileAccess.Read))
        {
            cred = GoogleCredential.FromStream(stream);
        }

        _db = builder.Build();

        Console.WriteLine("dum Diddly dee");
        //_client = new FirebaseClient(_config);
    }


    public async Task<DocumentSnapshot> GetDocumentAsync()
    {
        DocumentReference collection = _db.Collection("users").Document();
        DocumentSnapshot snapshot = await collection.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Console.WriteLine("Data");
            Console.WriteLine(snapshot.ToDictionary());
        }
        else
        {
            Console.WriteLine("Hehe hoohoo your code did a poopoo");
        }
        
        
        return snapshot;
    }
}