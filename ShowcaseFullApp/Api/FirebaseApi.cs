using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Amazon.SecurityToken.Model;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FireSharp;
using Google.Apis.Auth.OAuth2;
using ShowcaseFullApp.Services;
using ShowcaseFullApp.ViewModels;

namespace ShowcaseFullApp.Api;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Firestore;
using Grpc.Core;

public class FirebaseApi
{
    private FirestoreDb _db;

    private string apiKey = "AIzaSyDixyNjSzDT1dcYNqD2NYCAaOstvbrS-7o";
    private CollectionReference _collection;
    private readonly FirebaseAuthClient _firebaseAuth;
    private UserService _userService;

    private FirebaseAuthConfig config;
    //string projectId = "showcase-ebfee";

    public FirebaseApi()
    {

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "showcase-ebfee-firebase-adminsdk-14ql0-d0b9240d95.json");

        _userService = UserService.Current;
        _db = FirestoreDb.Create("showcase-ebfee");
        _collection = _db.Collection("Users");
        config = new FirebaseAuthConfig
        {
            ApiKey = apiKey,
            AuthDomain = "showcase-ebfee.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider(),
                new GoogleProvider()
            }
        };
        _firebaseAuth = new FirebaseAuthClient(config);
        //var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);


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
            CollectionReference showRef = docRef2.Collection("tvshows");
            QuerySnapshot snapshot2 = await showRef.GetSnapshotAsync();
        }
    }

    public async Task AddTvShow(string showName, string epName, int id)
    {
        refresh();
        if (_userService.email != "")
        {
            //Console.WriteLine(_userService.email);
            var emailquery = _collection.WhereEqualTo("email", _userService.email);
            QuerySnapshot snapshot = await emailquery.GetSnapshotAsync();
            //Console.WriteLine(snapshot.Documents.Count);
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
                    //Console.WriteLine(qs.Documents.Count);
                    if (qs.Documents.Count > 0)
                    {
                        isIn = true;
                    }
                    Console.WriteLine(isIn);
                    if (isIn == false)
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>
                        {
                            { "curepname", epName },
                            { "curepnum", 1 },
                            { "tvshowname", showName },
                            { "curseason", 1 },
                            { "showId", id}
                        };

                        await showRef.AddAsync(data);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("HAHA");
        }
        
    }


    public async Task<string?> Signup(string email, string password)
    {
        //Console.WriteLine($"Email:{email}, Password: {password}");
        try
        {
            var userCredentials = await _firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            if (userCredentials != null)
            { 
                addUserToFirebase(email);
            }
            
            return userCredentials is null ? null : await userCredentials.User.GetIdTokenAsync();
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine($"Error creating user: {ex.Reason}");
            // Handle the exception appropriately
        }
        
        

        return null;
    }

    private async void addUserToFirebase(string email)
    {
        refresh();
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        if (snapshot.Count <= 0)
        {
            
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "email", email }
            };

            DocumentReference docref = await _collection.AddAsync(data);
            CollectionReference showRef = docref.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            
            Dictionary<string, object> subcollectionData = new Dictionary<string, object>
            {
                // Add any data for the subcollection if needed
            };

            await showRef.AddAsync(subcollectionData);
            
        }
        else
        {
            Console.WriteLine($"{email} is already in here");
        }
        

    }

    public async Task<string?> LoginToFirebase(string email, string password)
    {
        try
        {
            var userCredentials = await _firebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            if (userCredentials != null)
            {
                return await userCredentials.User.GetIdTokenAsync();
            }
            
        }
        catch (FirebaseAuthException ex)
        {
            Console.WriteLine($"Login failed: {ex.Reason}");
        }

        return null;
    }

    public async Task<List<idShow>> getShowList(string email)
    {
        List<idShow> showList = new List<idShow>();
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (var doc in snapshot.Documents)
        {
            DocumentReference curdoc = _collection.Document(doc.Id);
            CollectionReference showRef = curdoc.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            foreach (var showDoc in query2.Documents)
            {
                // Assuming "showName" is a field in your TV show documents
                string showName = showDoc.GetValue<string>("tvshowname");
                string curEp = showDoc.GetValue<string>("curepname");
                string epNum = showDoc.GetValue<int>("curepnum").ToString();
                string id = showDoc.GetValue<int>("showId").ToString();
                string showValue = $"{showName}, Episode {epNum}: {curEp}";
                Console.WriteLine(showValue);
                if (int.TryParse(id, out int curID))
                {
                    idShow curShow = new idShow();
                    curShow.name = showValue;
                    curShow.id = curID;
                    showList.Add(curShow);
                }
            }
        }

        return showList;
    }

    public async Task<bool> isAdded(string email, string showName)
    {
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (var doc in snapshot.Documents)
        {
            DocumentReference docref = _collection.Document(doc.Id);
            CollectionReference showRef = docref.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            foreach (var showDoc in query2.Documents)
            {
                var curshowname = showDoc.GetValue<string>("tvshowname");
                if (curshowname == showName)
                {
                    return true;
                }
            }

            return false;
        }
        return false;
    }

    public async Task<int> getCurEp(string email, string showName)
    {
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (var doc in snapshot.Documents)
        {
            DocumentReference docref = _collection.Document(doc.Id);
            CollectionReference showRef = docref.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            foreach (var showDoc in query2.Documents)
            {
                var curshowname = showDoc.GetValue<string>("tvshowname");
                if (curshowname == showName)
                {
                    return showDoc.GetValue<int>("curepnum");
                }
            }
        }

        return 0;
    }
    
    public async Task<int> getCurSeason(string email, string showName)
    {
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (var doc in snapshot.Documents)
        {
            DocumentReference docref = _collection.Document(doc.Id);
            CollectionReference showRef = docref.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            foreach (var showDoc in query2.Documents)
            {
                var curshowname = showDoc.GetValue<string>("tvshowname");
                if (curshowname == showName)
                {
                    return showDoc.GetValue<int>("curseason");
                }
            }
        }

        return 0;
    }


    public async void updateEp(string email, string showName, int lastEp, int curSeason, string epName)
    {
        var query = _collection.WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        foreach (var doc in snapshot.Documents)
        {
            DocumentReference docref = _collection.Document(doc.Id);
            CollectionReference showRef = docref.Collection("tvshows");
            QuerySnapshot query2 = await showRef.GetSnapshotAsync();
            foreach (var showDoc in query2.Documents)
            {
                var curshowname = showDoc.GetValue<string>("tvshowname");
                if (curshowname == showName)
                {
                    var epNum = showDoc.GetValue<int>("curepnum");
                    if (lastEp >= epNum)
                    {
                        var updateData = new Dictionary<string, object>
                        {
                            { "curepnum", epNum + 1 },
                            { "curepname", epName}
                        };
                        await showDoc.Reference.UpdateAsync(updateData);
                    }
                    else
                    {
                        var updateData = new Dictionary<string, object>
                        {
                            { "curepnum", 1 },
                            {"curepname", epName},
                            {"curseason", curSeason + 1}
                        };
                        await showDoc.Reference.UpdateAsync(updateData);
                    }
                }
            }
        }
    }





    /*
    public async Task<string?> LoginGoogle()
    {
        try
        {
            var userCredentials = await _firebaseAuth.Login
        }
    }
    */
    
    
    
    
}