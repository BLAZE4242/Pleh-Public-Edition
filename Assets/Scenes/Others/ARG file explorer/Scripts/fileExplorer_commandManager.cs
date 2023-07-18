using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class fileExplorer_commandManager : MonoBehaviour
{
    [Header("Variables for commands")]
    [SerializeField] string secretIPaddress = "87.0.0.291.5383";
    public string adminPassword = "password";
    fileExplorer_inputManager _inputManager;
    fileExplorer_manager _manager;
    DemoDataHolder _data;
    DRP _drp;
    public string argument;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _inputManager = GetComponent<fileExplorer_inputManager>();
        _manager = GetComponent<fileExplorer_manager>();
        _data = FindObjectOfType<DemoDataHolder>();
        _drp = FindObjectOfType<DRP>();
    }

    //public string upload(bool isInboked = false)
    //{
    //    if (isInboked) return "lol";

    //    StartCoroutine(doUpload());
        
    //    return "";
    //}

    //IEnumerator doUpload()
    //{
    //    while (true)
    //    {
    //        string LongNum = longNum();
    //        _inputManager.ConsoleWrite("Uploading chunk: " + LongNum + ".asset");
    //        yield return GeneralManager.waitForSeconds(0.03f);
    //        _inputManager.ConsoleWrite("Uploaded chunk: " + LongNum + ".asset");
    //        yield return GeneralManager.waitForSeconds(UnityEngine.Random.Range(0.0f, 0.4f));
    //    }
    //}

    string longNum()
    {
        string yup = "";
        for (int i = 0; i < UnityEngine.Random.Range(9, 18); i++)
        {
            yup += UnityEngine.Random.Range(0, 9);
        }
        return yup;
    }

    public string help(bool isInvoked = false)
    {
        if(isInvoked) return "Displays this help menu";

        MethodInfo[] methods = typeof(fileExplorer_commandManager).GetMethods();
        foreach(MethodInfo method in methods)
        {
            if(method.Name == "callCommand") break; // If you change the name of the callCommand method make sure to change it here to!
            _inputManager.ConsoleWrite(method.Name + " - " + method.Invoke(this, new object[]{true}));
        }

        return "";
    }

    /* NOT USING
    public string fps(bool isInvoked = false)
    {
        if(isInvoked) return "use fps y or fps n to enable/disable the fps counter";
        switch (argument)
        {
            case "y":
                _data.CanShowFPS = true;
                _inputManager.ConsoleWrite("Enabled fps counter");
                break;
            case "n":
                _data.CanShowFPS = false;
                _inputManager.ConsoleWrite("Disabled fps counter");
                break;
            default:
                _inputManager.ConsoleThrowError("Invalid use of fps. Use fps y or fps n to enable/disable the fps counter");
                break;
        }

        argument = "";
        return "";
    }

    public string limit(bool isInvoked = false)
    {
        if(isInvoked) return "(NOT CURRENTLY WORKING) use limit X to set the FPS limit. Set to 60 by default";
        Application.targetFrameRate = int.Parse(argument);
        argument = "";
        return "";
    }

    public string crash(bool isInvoked = false)
    {
        if(isInvoked) return "(NOT CURRENTLY WORKING) use crash y or crash n to change if pleh will crash after chamber 2";
        switch(argument)
        {
            case "y":
                _data.CanCrash = true;
                _inputManager.ConsoleWrite("Enabled crashing");
                break;
            case "n":
                _data.CanCrash = false;
                _inputManager.ConsoleWrite("Disabled crashing");
                break;
            default:
                _inputManager.ConsoleThrowError("Invalid use of crash. Use crash y or crash n to enable/disable the ability for pleh to crash after chamber 2");
                break;
        }

        argument = "";
        return "";
    }

    public string glitch(bool isInvoked = false)
    {
        if(isInvoked) return "use glitch y or glitch n to change if pleh will launch in a glitched stage when running main.exe.";
        switch (argument)
        {
            case "y":
                PlayerPrefs.SetInt("glotchiness", 1);
                _inputManager.ConsoleWrite("Pleh is now in a glitched state");
                break;
            case "n":
                PlayerPrefs.SetInt("glotchiness", 0);
                _inputManager.ConsoleWrite("Pleh is no longer in a glitched state");
                break;
            default:
                _inputManager.ConsoleThrowError("Invalid use of glitch. Use glitch y or glitch n to enable/disable pleh launching in a glitched state when running main.exe");
                break;
        }

        argument = "";
        return "";
    }

    public string dox(bool isInvoked = false)
    {
        if(isInvoked) return "Dox yourself";
        _inputManager.ConsoleWrite("Pc name: " + Environment.UserName);
        _inputManager.ConsoleWrite("Discord name: " + _drp.playerName);
        _inputManager.ConsoleWrite("Discrim: " + _drp.discriminator);
        _inputManager.ConsoleWrite(_drp.premiumString);
        _inputManager.ConsoleWrite(_drp.playerName + " is in house " + _drp.flagString);
        
        string[] friends = new string[20];
        string[] blocks = new string[20];
        // asign friends to the first 3 elements of _drp.friendList
        for(int i = 0; i < 20; i++)
        {
            friends[i] = _drp.friendList[UnityEngine.Random.Range(0, _drp.friendList.Length)];
            blocks[i] = _drp.blockedList[UnityEngine.Random.Range(0, _drp.blockedList.Length)];
        }

        _inputManager.ConsoleWrite("");
        _inputManager.ConsoleWrite("Friends: " + string.Join(", ", friends));
        _inputManager.ConsoleWrite("");
        _inputManager.ConsoleWrite("Blocked: " + string.Join(", ", blocks));
        _inputManager.ConsoleWrite("");
        _inputManager.ConsoleWrite("Get doxed :pensive:");
        return "";
    }

    public string cd(bool isInvoked = false)
    {
        if(isInvoked) return "Loads a new directory based on subdirectories in current loaded directory";
        _manager.cdeeznuts(argument);
        argument = "";
        return "";
    }

    */

    public string print(bool isInvoked = false)
    {
        if (isInvoked) return "Prints text to the console";
        _inputManager.ConsoleWrite(argument);
        argument = "";
        return "";
    }

    public string credits(bool isInvoked = false)
    {
        if (isInvoked) return "Opens Full Credit Document.pdf";
        _manager.OpenCredits();
        return "";
    }

    public string cls(bool isInvoked = false)
    {
        if (isInvoked) return "Clears the history of the console";
        _inputManager.ConsoleClear();
        return "";
    }

    public string users(bool isInvoked = false)
    {
        if (isInvoked) return "Shows avaliable users to sign into";
        _manager.ShowUsers();
        return "";
    }

    public string dir(bool isInvoked = false)
    {
        if (isInvoked) return "Displays a list of files and subdirectories in current loaded directory";
        _manager.dir();
        return "";
    }

    public string open(bool isInvoked = false)
    {
        if(isInvoked) return "Opens a file or folder in current loaded directory by name";
        _manager.open(argument);
        argument = "";
        return "";
    }

    public string delete(bool isInvoked = false)
    {
        if (isInvoked) return "Deletes a file or folder from current loaded directory by name, if user is admin";
        _manager.delete(argument);
        argument = "";
        return "";
    }

    public string rollback(bool isInvoked = false)
    {
        if(isInvoked) return "Rolls back to the previous loaded directory";
        _manager.rollback();
        return "";
    }

    public string root(bool isInvoked = false)
    {
        if(isInvoked) return "Rolls back to the root of the current loaded drive";
        _manager.root();
        return "";
    }

    public string ping(bool isInvoked = false)
    {
        if(isInvoked) return "Pings a host based on an I.P address";
        else if(argument == secretIPaddress)
        {
            _inputManager.ConsoleWrite($"Pinging {argument} with 32 bytes of data:");
            _inputManager.ConsoleWrite($"Reply from {argument}: bytes=32 time=1ms TTL=115");
            _inputManager.ConsoleWrite("Reply content:\n");
            _inputManager.ConsoleWrite("getoutofmylife");
            _inputManager.ConsoleWrite("\nEnd content");

        }
        else if(argument == "") _inputManager.ConsoleThrowError("Invalid usage of 'ping'. To ping a host please include an I.P address as an argument. Example: ping 123.456.789");
        else _inputManager.ConsoleThrowError($"Ping request could not find host {argument}. Please check the name and/or your internet connection and try again.");
        return "";
    }

    public string brightness(bool isInvoked = false)
    {
        if (isInvoked) return "Changes the brightness in game as a percentage";
        _manager.Brightness(argument);
        return "";
    }

    public string speed(bool isInvoked = false)
    {
        if (isInvoked) return "Multiplies the speed of the player (could break game)";

        if (argument == "") _inputManager.ConsoleThrowError("Invalid usage of 'speed'. Include speed multiplier as argument. Example: speed 2.5");
        else
        {
            try
            {
                float multiplier = float.Parse(argument);
                PlayerPrefs.SetFloat("speed", multiplier);
                _inputManager.ConsoleWrite("Successfully set speed multiplier to " + multiplier + ".");
            }
            catch
            {
                _inputManager.ConsoleThrowError($"'{argument}' is not a speed multiplier. Include speed multiplier as argument. Example: speed 2.5");
            }
        }

        return "";
    }

    public string close(bool isInvoked = false)
    {
        if (isInvoked) return "Closes BIOSener.config and returns to game";
        _inputManager.ConsoleWrite("Reloading 'Pleh!.exe'");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        return "";
    }

    public string quit(bool isInvoked = false)
    {
        if (isInvoked) return "Quits the game";
        _manager.Quit();
        return "";
    }

    public void callCommand(string commandToCall, string argumentLocal = "")
    {
        argument = argumentLocal;
        var thisType = this.GetType();
        if (thisType.GetMethod(commandToCall) == null)
        {
            _inputManager.ConsoleThrowError($"'{commandToCall}' is not recognized as an internal or external command, operable program or batch file.");
            if (commandToCall == "cd") _inputManager.ConsoleWrite("like jesus this isn't a windows system32 terminal");
            return;
        }
        thisType.GetMethod(commandToCall).Invoke(this, new object[]{false});
        // Invoke(commandToCall, 0);
    }

    
}
