using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Gas : MonoBehaviour
{
    public string AchievementToRemove;
    public static bool Initialized;
    public static bool EarnAchievement(string AchievementName)
    {
        if (AchievementName == "ACH_MATH") return false; // Might add back, might not
        if (!SteamManager.Initialized) return false;
        bool hasAchievement;
        SteamUserStats.GetAchievement(AchievementName, out hasAchievement);
        if (hasAchievement) return false;

        SteamUserStats.SetAchievement(AchievementName);
        SteamUserStats.StoreStats();

        return hasAchievement;
    }

    public static void SetStat(string statName, int statValue)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetStat(statName, statValue);
        SteamUserStats.StoreStats();
    }

    public static int GetStat(string statName)
    {
        if (!SteamManager.Initialized) return -1;
        int statValue = 0;
        SteamUserStats.GetStat(statName, out statValue);
        return statValue;
    }

    public static bool HasAchievement(string AchievementName)
    {
        if (!SteamManager.Initialized) return false;
        bool hasAchievement;
        SteamUserStats.GetAchievement(AchievementName, out hasAchievement);
        return hasAchievement;
    }

    public static void DeleteStat(bool i = false)
    {
        SteamUserStats.ResetAllStats(i);
    }

    private void Update()
    {
        Initialized = SteamManager.Initialized;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Gas))]
    public class GasEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Gas _target = (Gas)target;

            base.OnInspectorGUI();

            if (GUILayout.Button("Reset Achievement") && SteamManager.Initialized)
            {
                SteamUserStats.ClearAchievement(_target.AchievementToRemove);
                SteamUserStats.StoreStats();
                _target.AchievementToRemove = "";
            }
            else if (GUILayout.Button("Reset all achievements") && SteamManager.Initialized)
            {
                SteamUserStats.ResetAllStats(true);
                SteamUserStats.StoreStats();
            }
        }
    }

    #endif
}
