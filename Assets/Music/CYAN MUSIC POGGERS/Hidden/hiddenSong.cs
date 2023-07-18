using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class hiddenSong : MonoBehaviour
{
    [SerializeField] int songID;
    [SerializeField] TextMeshPro text;
    [SerializeField] AudioClip juiceGroan;

    public void OnClick()
    {
        if (!PlayerPrefs.GetString("hs").Contains(songID.ToString()))
        {
            PlayerPrefs.SetString("hs", PlayerPrefs.GetString("hs") + songID.ToString());
        }
        text.text = "G:\\Pleh!_Backend_Data\\Jammers\\";
        FindObjectOfType<AudioSource>().PlayOneShot(juiceGroan);
    }
}
