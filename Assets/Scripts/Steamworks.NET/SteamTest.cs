using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamTest : MonoBehaviour
{
    CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;


    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) return;

        SteamUserStats.SetAchievement("ACH_QUIT");

        SteamUserStats.StoreStats();
    }
    private void Update()
    {
        
        SteamAPI.RunCallbacks();
    }
}
