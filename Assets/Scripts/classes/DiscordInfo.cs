using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using TMPro;

public class DiscordInfo : MonoBehaviour
{
    [HideInInspector] public Discord.Discord discord;
    [SerializeField] TextMeshProUGUI tetx;
    [SerializeField] TextMeshProUGUI friends;
    public static bool isConnectedDiscord;
    public static string username;

    void Start()
    {
        discord = new Discord.Discord(894426281205198919, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        var userManager = discord.GetUserManager();
        var relationshipManager = discord.GetRelationshipManager();

        userManager.OnCurrentUserUpdate += () =>{
            var currentUser = userManager.GetCurrentUser();
            username = currentUser.Username;
            tetx.text = (username);
        };

        relationshipManager.OnRefresh += () =>
        {
            relationshipManager.Filter((ref Discord.Relationship relationship) =>
            {
                return relationship.Type == Discord.RelationshipType.Friend;
            });

            // Loop over all friends a user has.
            Debug.Log($"relationships updated: {relationshipManager.Count()}");

            for (var i = 0; i < relationshipManager.Count(); i++)
            {
                // Get an individual relationship from the list
                var r = relationshipManager.GetAt((uint)i);
                friends.text += "\n" + r.User.Username;
                // Save r off to a list of user's relationships
            }
            Debug.LogError("Tis should be done!");
        };
    }

    void Update()
    {
        if(discord != null) 
        {
            isConnectedDiscord = true;
            discord.RunCallbacks();
        }
        else isConnectedDiscord = false;
    }
}
