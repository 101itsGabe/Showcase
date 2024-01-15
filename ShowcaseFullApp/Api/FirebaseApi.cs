using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using FireSharp;
using Google.Apis.Auth.OAuth2;

namespace ShowcaseFullApp.Api;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Firestore;

public class FirebaseApi
{
    private FirestoreDb _db;

    private CollectionReference _collection;
    //string projectId = "showcase-ebfee";

    public FirebaseApi()
    {

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "showcase-ebfee-firebase-adminsdk-14ql0-d0b9240d95.json");

        _db = FirestoreDb.Create("showcase-ebfee");
        _collection = _db.Collection("Users");
        QuerySnapshot querySnapshot = _collection.GetSnapshotAsync().Result;
        DocumentReference docRef = _db.Collection("users").Document("document-id");
        
    }

    public async void refresh()
    {
        _collection = _db.Collection("Users");
    }
    


    public async Task GetDocumentAsync()
    {
        refresh();
        QuerySnapshot snapshot = _collection.GetSnapshotAsync().Result;
        string email = "";
        string docId = "";
        foreach (DocumentSnapshot docSnap in snapshot.Documents)
        {
            email = docSnap.GetValue<string>("email");
            docId = docSnap.Id;
            DocumentReference docRef2 = _db.Collection("Users").Document(docId);
            //Console.WriteLine(_db.Collection("users").Document(docId).Collection("tvshows").ToString());
            CollectionReference showRef = docRef2.Collection("tvshows");
            QuerySnapshot snapshot2 = await showRef.GetSnapshotAsync();
            /*
            foreach (var docsnap2 in snapshot2.Documents)
            {
                var epname = docsnap2.GetValue<string>("curepname");
                var epnum = docsnap2.GetValue<int>("curepnum").ToString();
                Console.WriteLine($"EpName: {epname}, EpNum: {epnum}");
            }
            */
            
            //Console.WriteLine($"email: {email}, docId: {docId}");
        }
    }

    public async Task AddTvShow(string showName)
    {
        Console.WriteLine("Not even");
        refresh();
        QuerySnapshot snapshot = _collection.GetSnapshotAsync().Result;
        foreach (DocumentSnapshot docSnap in snapshot.Documents)
        {
            var docId = docSnap.Id;
            if (docId != null)
            {
                var docRef = _collection.Document(docId);
                var showRef = docRef.Collection("tvshows");
                var snapshot2 = await showRef.GetSnapshotAsync();
                bool isIn = false;
                Query query = showRef.WhereEqualTo("tvshowname", showName);
                QuerySnapshot qs = await query.GetSnapshotAsync();
                Console.WriteLine(qs.Documents.Count());
                if (qs.Documents.Count() > 0)
                {
                    isIn = true;
                }

                if (isIn == false)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>
                    {
                        { "curepname", "lil kodak" },
                        { "curepnum", 1 },
                        { "tvshowname", showName }
                    };

                    await showRef.AddAsync(data);
                }
                else
                {
                    Console.WriteLine("IM NEKED");
                }
            }
        }
    }
}