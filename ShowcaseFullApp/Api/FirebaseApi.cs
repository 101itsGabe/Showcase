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

        _userService = new UserService();
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

    public async Task AddTvShow(string showName)
    {
        Console.WriteLine("INSIDE TV SHOW");
        refresh();
        if (_userService.email != "")
        {
            Console.WriteLine(_userService.email);
            var emailquery = _collection.WhereEqualTo("email", _userService.email);
            QuerySnapshot snapshot = await emailquery.GetSnapshotAsync();
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
                }
            }
        }
    }


    public async Task<string?> Signup(string email, string password)
    {
        Console.WriteLine($"Email:{email}, Password: {password}");
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
        Console.WriteLine("inside add user");
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