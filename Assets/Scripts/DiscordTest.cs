using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using TMPro;
using UnityEngine.UI;

public class DiscordTest : MonoBehaviour
{

    public Discord.Discord discord;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] RawImage image;

    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(894426281205198919, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        var userManager = discord.GetUserManager();
        userManager.OnCurrentUserUpdate += () =>
        {
            var currentUser = userManager.GetCurrentUser();
            var premiumType = userManager.GetCurrentUserPremiumType();

            string premiumString = "";
            switch(premiumType)
            {
                case PremiumType.None:
                    premiumString = "not subscribed to nitro";
                    break;
                case PremiumType.Tier1:
                    premiumString = "are subscribed to nitro clasic";
                    break;
                case PremiumType.Tier2:
                    premiumString = "are subscribed to nitro";
                    break;
            }

            string flagString = "";
            if(userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse1)) flagString = "Bravery";
            else if(userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse2)) flagString = "Brilliance";
            else if (userManager.CurrentUserHasFlag(Discord.UserFlag.HypeSquadHouse3)) flagString = "Balance";

            text.text = $"Username is: {currentUser.Username}, discrim is: {currentUser.Discriminator}, id is: {currentUser.Id}, the user {premiumString} and they are part of the {flagString} house";

            
        };
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
    }
}
