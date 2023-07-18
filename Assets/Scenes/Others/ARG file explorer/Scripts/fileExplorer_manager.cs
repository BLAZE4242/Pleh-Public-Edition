using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Kino;

public class fileExplorer_manager : MonoBehaviour
{
    [Header("Folder Variables")]
    [SerializeField] fileExplorer_folder currentDir;
    fileExplorer_folder defaultDir;
    [SerializeField] fileExplorer_folder backendFolder;
    [SerializeField] fileExplorer_folder theFinalFolder;
    List<fileExplorer_folder> folderHistory = new List<fileExplorer_folder>();
    string currentDirectoryLocation = "G:\\Pleh!_Backend_Data\\";
    List<string> dirLocationHistory = new List<string>();
    fileExplorer_inputManager _inputManager;
    [HideInInspector] public bool selectingUser;
    public bool isAdmin;
    [SerializeField] GameObject uiPrefab;
    [SerializeField] AudioClip glitchSfx;
    VideoPlayer vidPlayer;
    bool watchingVideo;
    [SerializeField] AudioSource musicPlayer;

    void Start()
    {
        FindObjectOfType<DRP>().RP("Error: Cannot locate player");
        Gas.EarnAchievement("ACH_BIOSENER");
        PlayerPrefs.DeleteKey("thats it");
        if (FindObjectOfType<music>() != null) Destroy(FindObjectOfType<music>().gameObject);

        isAdmin = PlayerPrefs.GetInt("isAdmin") == 1;

        _inputManager = GetComponent<fileExplorer_inputManager>();
        vidPlayer = FindObjectOfType<VideoPlayer>();
        vidPlayer.SetTargetAudioSource(0, vidPlayer.GetComponent<AudioSource>());
        defaultDir = currentDir;
        // Keep this here only if we're starting in a folder that is not Backend
        folderHistory.Add(backendFolder);
        dirLocationHistory.Add("G:\\Pleh!_Backend_Data\\");

    }

    public void dir()
    {
        ConsoleWrite("Directory of " + currentDirectoryLocation);
        if (currentDir.roots.Count() == 0 && currentDir.files.Count() == 0)
        {
            ConsoleWrite("  Nothing to display in this directory");
            return;
        }
        if (currentDir.roots.Length > 0)
        {
            foreach (fileExplorer_folder subdir in currentDir.roots)
            {
                if (subdir.name == "Jammers" && !PlayerPrefs.HasKey("hs")) { }
                else ConsoleWrite("  " + subdir.name + "   <dir>");
            }
        }
        try
        {
            if (currentDir.files.Length > 0)
            {
                foreach (fileExplorer_file subfile in currentDir.files)
                {
                    if (subfile.fileType == fileExplorer_file.FileTypes.zip && subfile.isOpened())
                    {
                        ConsoleWrite("  " + subfile.m_zipFolder.name + "   <dir>");
                    }
                    else
                    {
                        if (subfile.fileType != fileExplorer_file.FileTypes.sex)
                        {
                            if (currentDir.name == "Jammers" && !CanShowAudio(subfile.name))
                            {
                                continue;
                            }
                            ConsoleWrite($"  {subfile.name}   <{subfile.fileType}>");
                        }
                        else ConsoleWrite($"  {subfile.name}   <exe>");
                        if (subfile.name == "confession.mp4")
                        {
                            StartCoroutine(glitchAbit());
                        }
                    }
                }
            }
        }
        catch
        {
            Debug.Log("Look, I don't get it either but whatever");
        }
    }

    bool CanShowAudio(string audioName)
    {
        switch (audioName)
        {
            case "hidden song 1.mp3":
                return PlayerPrefs.GetString("hs").Contains("1");
            case "hidden song 2.mp3":
                return PlayerPrefs.GetString("hs").Contains("2");
            case "hidden song 3.mp3":
                return PlayerPrefs.GetString("hs").Contains("3");
            case "hidden song 4.mp3":
                return PlayerPrefs.GetString("hs").Contains("4");
            case "hidden song 5.mp3":
                return PlayerPrefs.GetString("hs").Contains("5");
            case "beans and children.mp3":
                return PlayerPrefs.GetString("hs").Contains("6");
            case "thanks cyan.txt":
                string hs = PlayerPrefs.GetString("hs");
                return hs.Contains("1") && hs.Contains("2") && hs.Contains("3") && hs.Contains("4") && hs.Contains("5") && hs.Contains("6");
            default:
                return true;
        }
    }

    IEnumerator glitchAbit()
    {
        FindObjectOfType<DigitalGlitch>().intensity = 0.3f;
        FindObjectOfType<AudioSource>().PlayOneShot(glitchSfx);

        yield return GeneralManager.waitForSeconds(glitchSfx.length);

        FindObjectOfType<DigitalGlitch>().intensity = 0f;
    }

    public void rollback()
    {
        if (currentDir == theFinalFolder)
        {
            _inputManager.ConsoleThrowError(currentDirectoryLocation + " is already root of directory.");
            return;
        }
        if (currentDir == backendFolder)
        {
            currentDir = theFinalFolder;
            currentDirectoryLocation = "G:\\";
            folderHistory.Remove(currentDir);
            dirLocationHistory.Remove(currentDirectoryLocation);
            ConsoleWrite("Successfully rolled back to " + currentDirectoryLocation);
            return;
        }

        currentDir = folderHistory.Last();
        currentDirectoryLocation = dirLocationHistory.Last();
        folderHistory.Remove(currentDir);
        dirLocationHistory.Remove(currentDirectoryLocation);
        ConsoleWrite("Successfully rolled back to " + currentDirectoryLocation);
    }

    public void root()
    {
        currentDir = defaultDir;
        currentDirectoryLocation = "G:\\Pleh!_Backend_Data\\";
        folderHistory = new List<fileExplorer_folder>();
        dirLocationHistory = new List<string>();
        ConsoleWrite("Successfully rolled back to " + currentDirectoryLocation);
    }

    public void cdeeznuts(string desiredSubdirectory)
    {
        if (desiredSubdirectory == "")
        {
            _inputManager.ConsoleThrowError("Invalid usage of 'cd'. To load a new directory please include a valid subfolder to load. Example: cd folder1. You can see all available subfolders by using 'dir' to see the current loaded directory.");
            return;
        }

        foreach (fileExplorer_folder subdir in currentDir.roots)
        {
            if (desiredSubdirectory == subdir.name)
            {
                ActuallyCdFr(subdir);
                return;
            }
        }

        _inputManager.ConsoleThrowError("The system cannot find the path specified");
    }

    public void ActuallyCdFr(fileExplorer_folder subdir)
    {
        folderHistory.Add(currentDir);
        dirLocationHistory.Add(currentDirectoryLocation);
        currentDirectoryLocation += subdir.name + "\\";
        currentDir = subdir;
        ConsoleWrite("Successfully loaded into directory " + currentDirectoryLocation);
        return;
    }

    Coroutine playVideoEnum;
    public void open(string fileToOpen)
    {
        List<fileExplorer_folder> currentRoots = new List<fileExplorer_folder>(currentDir.roots);
        foreach (fileExplorer_file file in currentDir.files)
        {
            if (file.isOpened()) currentRoots.Add(file.m_zipFolder);
        }

        foreach (fileExplorer_folder subdir in currentRoots)
        {
            if (fileToOpen == subdir.name && subdir.name == "Jammers" && !PlayerPrefs.HasKey("hs"))
            {

            }
            else if (fileToOpen == subdir.name)
            {
                ActuallyCdFr(subdir);
                return;
            }
        }

        if (fileToOpen == "")
        {
            _inputManager.ConsoleThrowError("Invalid usage of 'open'. To open a file/folder please include the file/folder name (with file extension type) as an argument. Example: open filename.txt");
            return;
        }

        foreach (fileExplorer_file file in currentDir.files)
        {
            if (fileToOpen == file.name)
            {
                if (file.fileType != fileExplorer_file.FileTypes.zip && file.fileType != fileExplorer_file.FileTypes.sex && file.fileType != fileExplorer_file.FileTypes.mp3 && file.fileType != fileExplorer_file.FileTypes.txt) ConsoleWrite("Successfully opened " + file.name);
                switch (file.fileType)
                {
                    case fileExplorer_file.FileTypes.txt:
                        if(currentDir.name == "Jammers" && !CanShowAudio(file.name))
                        {
                            _inputManager.ConsoleThrowError("The system cannot find the file specified");
                            return;
                        }
                        ConsoleWrite("Successfully opened " + file.name);
                        ConsoleWrite("Contents of file:");
                        ConsoleWrite("\n" + file.m_textMessage.ToString());
                        ConsoleWrite("\nEnd file");
                        break;
                    case fileExplorer_file.FileTypes.exe:
                        ConsoleWrite("Running " + file.name + "...");
                        if (file.IsMalware)
                        {
                            _inputManager.askConfirmationMalware(file);
                        }
                        else try
                            {
                                PlayerPrefs.SetInt("glotchiness", file.glotchiness);
                                SceneManager.LoadScene(file.sceneName);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                                _inputManager.ConsoleThrowError("The executable is corrupt or damaged");
                            }
                        break;
                    case fileExplorer_file.FileTypes.zip:
                        if (!file.isOpened()) _inputManager.askConfirmationPassword(file);
                        else _inputManager.ConsoleThrowError("The system cannot find the file specified");
                        break;
                    case fileExplorer_file.FileTypes.mp4:
                        if (playVideoEnum != null)
                        {
                            StopVideo();
                        }
                        if (file.name == "confession.mp4")
                        {
                            StartCoroutine(endOne());
                            ConsoleWrite("Opening confession.mp4...");
                            return;
                        }
                        playVideoEnum = StartCoroutine(PlayVideo(file.m_videoClip));
                        break;
                    case fileExplorer_file.FileTypes.sex:
                        if (!PlayerPrefs.HasKey("reddelete")) ConsoleWrite("You cannot do that.");
                        else ConsoleWrite("You still cannot do that.");
                        break;
                    case fileExplorer_file.FileTypes.mp3:
                        if (CanShowAudio(file.name))
                        {
                            ConsoleWrite("Successfully opened " + file.name);
                            ConsoleWrite("Playing audio...");
                            musicPlayer.clip = file.m_audioClip;
                            musicPlayer.Play();
                        }
                        else
                        {
                            _inputManager.ConsoleThrowError("The system cannot find the file specified");
                        }
                        break;
                }
                return;
            }
        }

        _inputManager.ConsoleThrowError("The system cannot find the file specified");
    }

    private protected IEnumerator endOne()
    {
        _inputManager.canType = false;
        yield return GeneralManager.waitForSeconds(1);
        FindObjectOfType<DigitalGlitch>().intensity = 1;
        AudioSource source = FindObjectOfType<AudioSource>();
        source.gameObject.AddComponent<AudioDistortionFilter>();
        source.PlayOneShot(glitchSfx);

        yield return GeneralManager.waitForSeconds(glitchSfx.length);

        FindObjectOfType<DigitalGlitch>().intensity = 0;
        GetComponent<fileExplorer_se>().enabled = true;
    }

    void StopVideo()
    {
        if(playVideoEnum != null) StopCoroutine(playVideoEnum);
        vidPlayer.Stop();
        _inputManager.consoleText.color = Color.white;
        _inputManager.inputTextUI.color = Color.white;
        _inputManager.canType = true;
        watchingVideo = false;
    }

    IEnumerator PlayVideo(VideoClip clip)
    {
        watchingVideo = true;
        vidPlayer.clip = clip;
        vidPlayer.Play();
        _inputManager.consoleText.color = Color.clear;
        _inputManager.inputTextUI.color = Color.clear;
        _inputManager.canType = false;
        yield return GeneralManager.waitForSeconds((float)clip.length);
        StopVideo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && watchingVideo)
        {
            StopVideo();
        }
    }

    public void confirm(fileExplorer_file file)
    {
        ConsoleWrite("Running " + file.name + "...");
        try
        {
            SceneManager.LoadScene(file.sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            _inputManager.ConsoleThrowError("The executable is corrupt or damaged");
        }
    }

    public void ShowUsers()
    {
        ConsoleWrite("Current users on this drive:");
        ConsoleWrite("");
        ConsoleWrite($"  {PlayerPrefs.GetString("playerName")}   <User>");
        ConsoleWrite($"  test-user-delete-after   <Admin>");
        ConsoleWrite("");
        ConsoleWrite("Enter name of user to sign into or type nothing to leave");
        selectingUser = true;
    }

    public void delete(string fileToDelete)
    {
        if (fileToDelete == "Pleh!_Backend_Data")
        {
            foreach (fileExplorer_folder folder in currentDir.roots)
            {
                if (folder.name == "Pleh!_Backend_Data")
                {
                    _inputManager.isInDeletingProgress = true;
                    ConsoleWrite("Are you sure you want to erase ALL progress? This includes game save data, passwords for zips, chair stats and more. To confirm type \"goodbye.\" or type nothing to cancel.");
                    return;
                }
            }
        }

        if (!isAdmin)
        {
            _inputManager.ConsoleThrowError("User requires Admin permission to delete files. Try signing into an Admin user.");
            return;
        }
        
        foreach (fileExplorer_file file in currentDir.files)
        {
            if (fileToDelete == file.name.Replace(".zip", ""))
            {
                if (file.name == "unknown entity.exe")
                {
                    Sex();
                    return;
                }
                else if (file.name.Contains(".zip"))
                {
                    _inputManager.ConsoleThrowError("Folder is currently being used by 'G:\\Pleh!_Backend_Data\\unknown entity.exe'. Please close the program then try again.");
                    return;
                }
                else
                {
                    _inputManager.ConsoleThrowError("File is currently being used by 'G:\\Pleh!_Backend_Data\\unknown entity.exe'. Please close the program then try again.");
                    return;
                }
            }
        }

        foreach (fileExplorer_folder folder in currentDir.roots)
        {
            if (fileToDelete == folder.name)
            {
                _inputManager.ConsoleThrowError("Folder is currently being used by 'G:\\Pleh!_Backend_Data\\unknown entity.exe. Please close the program then try again.");
                return;
            }
        }

        _inputManager.ConsoleThrowError($"Could not find file '{fileToDelete}'. Please check spelling and capitalization and try again.");
    }

    void Sex()
    {
        GetComponent<fileExplorer_sex>().Progress();
    }

    public void Brightness(string percentage)
    {
        if (percentage == "")
        {
            _inputManager.ConsoleThrowError("Invalid usage of 'brightness'. To change brightness please unclude a number between 0 and 100 as an argument. Example: brightness 75");
            return;
        }
        if (float.TryParse(percentage.Replace("%", ""), out float percent))
        {
            if (percent >= 0 && percent <= 100)
            {
                ChangeBrightness(percent);
            }
            else
            {
                _inputManager.ConsoleThrowError("Percentage has to be between 0 and 100");
            }
        }
        else _inputManager.ConsoleThrowError($"'{percentage}' is not a valid number");
    }

    void ChangeBrightness(float percent)
    {
        Image brightness = FindObjectOfType<GeneralManager>().GetComponentInChildren<Image>();

        if (brightness == null)
        {
            brightness = Instantiate(uiPrefab, FindObjectOfType<GeneralManager>().transform).GetComponentInChildren<Image>();
        }

        brightness.color = new Color(0, 0, 0, (100 - percent) / 100); // Ami came up with this equation, it's a simple one but I'm dumb lol, thanks Ami :)

        ConsoleWrite($"Changed brightness to {percent}%\nNote this takes place in game and not while using BIOSener.config");
    }

    public void Quit()
    {
        ConsoleWrite("Exiting \'Pleh!.exe\' with exit code 0.");
        Application.Quit();
    }

    void ConsoleWrite(string message)
    {
        _inputManager.ConsoleWrite(message);
    }

    public void OpenCredits()
    {
        ConsoleWrite("Openning credits...");
    }
}
