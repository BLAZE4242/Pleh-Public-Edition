using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;

public class pmurgsIloveYouWithAllMyHeart : MonoBehaviour
{
    [SerializeField] TMP_Text X, Y, other;

    private void Start()
    {
        if (Gas.Initialized && SteamUser.GetSteamID().ToString() == "76561198008037399")
        {
            other.gameObject.SetActive(true);
            other.text = "Hello pmurgs apparently the invert mouse species didn't go extinct in 2018 so heres ur update enjoy :)";
        }
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("pmurgs"))
        {
            string thing = PlayerPrefs.GetString("pmurgs");
            
            if (thing.Contains("X"))
            {
                X.color = Color.yellow;
            }
            else
            {
                X.color = Color.white;
            }

            if (thing.Contains("Y"))
            {
                Y.color = Color.yellow;
            }
            else
            {
                Y.color = Color.white;
            }
        }
    }

    public void OnXselect()
    {
        if (!PlayerPrefs.HasKey("pmurgs"))
        {
            PlayerPrefs.SetString("pmurgs", "X");
            return;
        }

        if (PlayerPrefs.GetString("pmurgs").Contains("X"))
        {
            PlayerPrefs.SetString("pmurgs", PlayerPrefs.GetString("pmurgs").Replace("X", ""));
        }
        else
        {
            PlayerPrefs.SetString("pmurgs", PlayerPrefs.GetString("pmurgs") + "X");
        }
    }

    public void OnYselect()
    {
        if (!PlayerPrefs.HasKey("pmurgs"))
        {
            PlayerPrefs.SetString("pmurgs", "Y");
            return;
        }

        if (PlayerPrefs.GetString("pmurgs").Contains("Y"))
        {
            PlayerPrefs.SetString("pmurgs", PlayerPrefs.GetString("pmurgs").Replace("Y", ""));
        }
        else
        {
            PlayerPrefs.SetString("pmurgs", PlayerPrefs.GetString("pmurgs") + "Y");
        }
    }
}
