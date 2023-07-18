using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class glitchText : MonoBehaviour
{
    public AudioClip clickSfx;
    string glitchedText;
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void glitchGlitchedText(TextMeshPro tmp)
    {
        StartCoroutine(createGlitchedText(tmp.text, tmp));
    }

    public void glitchGlitchedText(TextMeshProUGUI tmp)
    {
        StartCoroutine(createGlitchedText(tmp.text, tmp));
    }

    public IEnumerator createGlitchedText(string text, TextMeshPro tmp)
    {
        if(clickSfx != null) source.PlayOneShot(clickSfx, 1.5f);

        char[] characters = text.ToCharArray();
        char[] glitchedCharacters = characters;
        int[] randNumbers = randomNonRepeatingNumbers(Mathf.RoundToInt(text.Length / 4), 0, text.Length);
        foreach (int randIndex in randNumbers)
        {
            glitchedCharacters[randIndex] = '_'; // Replace _ with 🗅
        }

        glitchedText = new string(glitchedCharacters);
        //var replacment = glitchedCharacters.ToString().Replace("_", "🗅");
        //glitchedText = replacment.ToString(); // Maybe add .ToString()?
        tmp.text = glitchedText;
        yield return GeneralManager.waitForSeconds(0.05f);

        foreach (int randIndex in shuffleArray(randNumbers)) // Make random order
        {
            glitchedCharacters[randIndex] = text.ToCharArray()[randIndex];
            glitchedText = new string(glitchedCharacters);
            tmp.text = glitchedText; // If this doesn't work try setting glitchedText.ToCharArray to a variable
            yield return GeneralManager.waitForSeconds(0.05f);
        }
    }

    public IEnumerator createGlitchedText(string text, TextMeshProUGUI tmp)
    {
        source.PlayOneShot(clickSfx, 3f);

        char[] characters = text.ToCharArray();
        char[] glitchedCharacters = characters;
        int[] randNumbers = randomNonRepeatingNumbers(Mathf.RoundToInt(text.Length / 4), 0, text.Length);
        foreach (int randIndex in randNumbers)
        {
            glitchedCharacters[randIndex] = '_'; // Replace _ with 🗅
        }

        glitchedText = new string(glitchedCharacters);
        //var replacment = glitchedCharacters.ToString().Replace("_", "🗅");
        //glitchedText = replacment.ToString(); // Maybe add .ToString()?
        tmp.text = glitchedText;
        yield return GeneralManager.waitForSeconds(0.05f);

        foreach (int randIndex in shuffleArray(randNumbers)) // Make random order
        {
            glitchedCharacters[randIndex] = text.ToCharArray()[randIndex];
            glitchedText = new string(glitchedCharacters);
            tmp.text = glitchedText; // If this doesn't work try setting glitchedText.ToCharArray to a variable
            yield return GeneralManager.waitForSeconds(0.05f);
        }
    }

    int[] shuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = UnityEngine.Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }

    int[] randomNonRepeatingNumbers(int amount, int min, int max)
    {
        List<int> randomNumbers = new List<int>();
        for (int i = amount; i > 0; i--)
        {
            int randomNumber = Random.Range(min, max);
            while (randomNumbers.Contains(randomNumber))
            {
                randomNumber = Random.Range(min, max);
            }
            randomNumbers.Add(randomNumber);
        }
        return randomNumbers.ToArray();
    }
}
