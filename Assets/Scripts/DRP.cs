using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Discord;
using Steamworks;

public class DRP : MonoBehaviour
{
    public Discord.Discord discord;
    [SerializeField] string topText = "Playing Pleh!";
    public string bottomText = "Main Menu";
    [SerializeField] string topTextEditor = "Programming Pleh!";
    string bottomTextEditor = "(very fun(don't need help please))";
    [SerializeField] string[] editorTexts = new string[] {"I liek sitting on chairs", "I could be playing beat saber now", "mommy sorry mommy sorry mommy sorry mommy sorry", "c# my beloved", "c# > java :)", "where acc?", "System.Linq <3", "monika nendoroid monika nendoroid monika nendoroid", "{playerName}, that's a nice name.", "monka"};
    [SerializeField] string image = "pleh";
    [SerializeField] string miniImage;

    [Header("Player info")]
    [SerializeField] bool shouldCheckForDiscordInfo = false;
    public string playerName = ""; // Set this by default to equal the pc name
    public string discriminator;
    public string premiumString;
    public string flagString;
    public string[] friendList;
    [HideInInspector] public long[] friendListAsId;
    public string[] blockedList;
    public bool isConnectedToUser, hasCheckedRelationships;
    DemoDataHolder _data;
    [HideInInspector] public string friendName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        DRP[] drpim = FindObjectsOfType<DRP>();
        if (drpim.Length > 1) Destroy(gameObject);
    }

    void Start()
    {
        DoShit();
    }

    public IEnumerator GetFriendNameReady()
    {
        string steam = SteamFriend();
        if (steam != "")
        {
            friendName = steam;
        }
        else
        {
            yield return new WaitUntil(() => friendList.Length > 1);
            friendName = DiscordFriend();
        }
    }

    public void RP(string BottomText, string TopText = "Playing Pleh!")
    {
        topText = TopText;
        bottomText = BottomText;
        try { DoShit(); } catch { }
    }

    public void DoShit()
    {
        if (PlayerPrefs.GetInt("Streamer Mode") == 2)
        {
            PlayerPrefs.SetString("playerName", "Player");
            return;
        }

        playerName = PlayerPrefs.GetString("playerName", "");
        _data = FindObjectOfType<DemoDataHolder>();

        if (PlayerPrefs.GetString("playerName") == "" && playerName == "" || PlayerPrefs.GetString("playerName") == "Player")
        {
            if (SteamManager.Initialized) PlayerPrefs.SetString("playerName", SteamFriends.GetPersonaName());
            else if (PlayerPrefs.GetInt("Streamer Mode") == 0) PlayerPrefs.SetString("playerName", Environment.UserName);
            playerName = PlayerPrefs.GetString("playerName");
        }

        discord = new Discord.Discord(894426281205198919, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        var activityManager = discord.GetActivityManager();
        var userManager = discord.GetUserManager();
        var relationshipManager = discord.GetRelationshipManager();

        if (Application.isEditor)
        {
            //topText = topTextEditor;
            //bottomText = bottomTextEditor;
        }
        // Change discord rich presence
        var activity = new Discord.Activity
        {
            Details = topText,
            State = bottomText,
            Assets = {
               LargeImage = image,
               SmallImage = miniImage
           }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Discord.Result.Ok)
            {
                Debug.LogError("Couldn't update status!");
            }
        });

        // Get the username
        userManager.OnCurrentUserUpdate += () =>
        {
            isConnectedToUser = true;

            var currentUser = userManager.GetCurrentUser();
            if (currentUser.Username != "")
            {
                PlayerPrefs.SetString("playerName", currentUser.Username);
                playerName = PlayerPrefs.GetString("playerName");
            }
            discriminator = currentUser.Discriminator;

            var premiumType = userManager.GetCurrentUserPremiumType();

            switch (premiumType)
            {
                case PremiumType.None:
                    premiumString = playerName + " is not subscribed to nitro";
                    break;
                case PremiumType.Tier1:
                    premiumString = playerName + " is subscribed to nitro clasic";
                    break;
                case PremiumType.Tier2:
                    premiumString = playerName + " is subscribed to nitro";
                    break;
            }

            if (userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse1)) flagString = "Bravery";
            else if (userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse2)) flagString = "Brilliance";
            else if (userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse3)) flagString = "Balance";
        };
        relationshipManager.OnRefresh += () =>
        {
            relationshipManager.Filter((ref Discord.Relationship relationship) =>
            {
                return relationship.Type == Discord.RelationshipType.Friend;
            });

            List<string> friends = new List<string>();
            List<long> friendsId = new List<long>();

            for (var i = 0; i < relationshipManager.Count(); i++)
            {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                friends.Add(r.User.Username);
                friendsId.Add(r.User.Id);
            }

            friendList = friends.ToArray();
            friendListAsId = friendsId.ToArray();

            relationshipManager.Filter((ref Discord.Relationship relationship) =>
            {
                return relationship.Type == Discord.RelationshipType.Blocked;
            });

            List<string> blocked = new List<string>();

            for (var i = 0; i < relationshipManager.Count(); i++)
            {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                blocked.Add(r.User.Username);
            }

            blockedList = blocked.ToArray();

            hasCheckedRelationships = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(discord != null) discord.RunCallbacks();
    }

    public string DiscordFriend()
    {
        return friendList[UnityEngine.Random.Range(0, friendList.Length - 1)];
    }

    public string SteamFriend()
    {
        if (!SteamManager.Initialized) return "";
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        if (friendCount == 0) return "";
        return SteamFriends.GetFriendPersonaName(SteamFriends.GetFriendByIndex(UnityEngine.Random.Range(0, friendCount-1), EFriendFlags.k_EFriendFlagImmediate));
    }

    private void NewMethod()
    {
        var userManager = discord.GetUserManager();
        var relationshipManager = discord.GetRelationshipManager();

        relationshipManager.OnRefresh += () =>
        {
            relationshipManager.Filter((ref Relationship relationship) =>
            {
                return relationship.Type == RelationshipType.Friend;
            });

            List<string> friends = new List<string>();

            for (var i = 0; i < relationshipManager.Count(); i++)
            {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                Debug.Log("one");
                friends.Add(r.User.Username);
            }

            //List<long> friendsId = new List<long>();

            friendList = friends.ToArray();
            //friendListAsId = friendsId.ToArray();

            //relationshipManager.Filter((ref Relationship relationship) =>
            //{
            //    return relationship.Type == RelationshipType.Blocked;
            //});

            //List<string> blocked = new List<string>();

            //for (var i = 0; i < relationshipManager.Count(); i++)
            //{
            //    // Get an individual relationship from the list
            //    var r = relationshipManager.GetAt((uint)i);
            //    blocked.Add(r.User.Username);
            //}

            //blockedList = blocked.ToArray();

            //hasCheckedRelationships = true;
        };
    }
}
