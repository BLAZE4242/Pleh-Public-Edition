using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class DiscordLockScreen : MonoBehaviour
{
    [SerializeField] long FriendToPlehId = 957621018271293522;
    TextMeshProUGUI mainText;
    DRP drp;

    void Start()
    {
        mainText = FindObjectOfType<TextMeshProUGUI>();
        drp = FindObjectOfType<DRP>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (drp.friendListAsId.Contains(FriendToPlehId))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(drp.friendListAsId.Count() > 0)
        {
            mainText.text = "Error! You do not have permission to play pleh (lol)\nMake sure that the discord account FriendToPleh#9238 is added on your friend list.\nPress R to refresh.";
        }
    }
}
