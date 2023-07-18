using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Kino;
using UnityEngine.SceneManagement;

public class fileExplorer_inputManager : MonoBehaviour
{
    public bool canType = true;
    public TextMeshProUGUI inputTextUI;
    public TextMeshProUGUI consoleText;
    [HideInInspector] public bool isConfirmingMalware;
    [HideInInspector] public bool isConfirmingPassword;
    [HideInInspector] public bool isConfirmingPasswordUser;
    [HideInInspector] public bool isDeleting;
    [HideInInspector] public bool shouldRestart;
    [HideInInspector] public bool isInDeletingProgress;
    [SerializeField] AudioClip glitchSfx;
    List<string> textHistory = new List<string>();
    int historyIndex = 0;
    fileExplorer_commandManager _commandManager;
    fileExplorer_manager _manager;
    DRP _drp;
    string inputText;

    void Start()
    {

        _commandManager = GetComponent<fileExplorer_commandManager>();
        _manager = GetComponent<fileExplorer_manager>();
        _drp = FindObjectOfType<DRP>();

        //if(_drp.playerName != "") consoleText.text = "Signed in as " + _drp.playerName;
    }

    void Update()
    {
        if(canType) checkForInput();
    }

    void checkForInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            undoText();
            return;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            redoText();
            return;
        }

        foreach (char pressedChar in Input.inputString)
        {
            if(pressedChar == '\b')
            {
                if(inputText.Length > 0)
                {
                    inputText = inputText.Substring(0, inputText.Length - 1);
                }
            }
            else if(pressedChar == '\n' || pressedChar == '\r')
            {
                ConsoleWrite("> " + inputText);
                decipherCommand(inputText);
                // TODO: fix bug where forwards and backwards don't really work ;-;
                textHistory.Add(inputText);
                historyIndex = textHistory.Count;
                inputText = "";
            }
            else
            {
                inputText += pressedChar;
            }

            inputTextUI.text = inputText;
        }
    }

    void undoText()
    {
        // Make the inputText equal the last item in the textHistory
        inputText = textHistory.ElementAt(historyIndex - 1);
        historyIndex--;
        // Update the inputTextUI
        inputTextUI.text = inputText;
    }

    void redoText()
    {
        inputText = textHistory.ElementAt(historyIndex + 1);
        historyIndex++;
        inputTextUI.text = inputText;
    }

    void saveText()
    {
        textHistory.Add(inputText);
    }

    fileExplorer_file dangerousFile;

    public void askConfirmationMalware(fileExplorer_file file)
    {
        isConfirmingMalware = true;
        ConsoleWrite($"WARNING: files such as {file.name} are known to possibly contain malware and could run malicious code on your system. Are you sure you would like to run this file? y/n");
        dangerousFile = file;
    }

    fileExplorer_file passwordFile;
    public void askConfirmationPassword(fileExplorer_file file)
    {
        isConfirmingPassword = true;
        ConsoleWrite($"Zip file {file.name} is password protected.");
        ConsoleWrite($"Please enter password or type nothing to leave");
        passwordFile = file;
    }

    void decipherCommand(string commandWritten)
    {
        if (isInDeletingProgress)
        {
            if (commandWritten == "goodbye.")
            {
                PlayerPrefs.DeleteAll();
                Application.Quit();
            }
            else if(commandWritten != "")
            {
                ConsoleWrite("Type \"goodbye.\" to erase ALL progress or type nothing to cancel.");
            }
            else
            {
                ConsoleWrite("Cancelled deletion operation.");
                isInDeletingProgress = false;
            }

            return;
        }
        else if(isConfirmingMalware)
        {
            switch(commandWritten)
            {
                case "y":
                    isConfirmingMalware = false;
                    _manager.confirm(dangerousFile);
                    return;
                case "n":
                    isConfirmingMalware = false;
                    ConsoleWrite("Aborting...");
                    dangerousFile = null;
                    return;
                default:
                    ConsoleThrowError("Invalid use of confirm. Use confirm y or confirm n to confirm or deny the command.");
                    return;
            }
        }
        else if(isConfirmingPassword)
        {
            if (commandWritten.ToLower() == passwordFile.password.ToLower())
            {
                isConfirmingPassword = false;
                

                ConsoleWrite("Extracting.");
                ConsoleWrite("Extracting..");
                ConsoleWrite("Extracting...");
                ConsoleWrite("Extracting....");
                _manager.ActuallyCdFr(passwordFile.m_zipFolder);
                if (!PlayerPrefs.HasKey("zip_opened")) PlayerPrefs.SetString("zip_opened", $"{passwordFile.name},{passwordFile.password},");
                else if (!PlayerPrefs.GetString("zip_opened").Contains($"{passwordFile.name},{passwordFile.password},"))
                {
                    PlayerPrefs.SetString("zip_opened", PlayerPrefs.GetString("zip_opened") + $"{passwordFile.name},{passwordFile.password},");
                }

            }
            else if(commandWritten == "")
            {
                ConsoleWrite("Cancelled Zip operation.");
                isConfirmingPassword = false;
            }
            else
            {
                ConsoleWrite("Incorrect password, try again or type nothing to leave");
            }
            return;
        }
        if (_manager.selectingUser)
        {
            if (isConfirmingPasswordUser)
            {
                if (commandWritten == _commandManager.adminPassword)
                {
                    ConsoleWrite("Successfully signed into Admin <test-user-delete-after>");
                    _manager.isAdmin = true;
                    PlayerPrefs.SetInt("isAdmin", 1);
                    isConfirmingPasswordUser = false;
                    _manager.selectingUser = false;
                }
                else if (commandWritten == "")
                {
                    ConsoleWrite("Cancelled sign in");
                    isConfirmingPasswordUser = false;
                    _manager.selectingUser = false;
                }
                else
                {
                    ConsoleWrite("Incorrect password, try again or type nothing to leave");
                }

                return;
            }

            if (commandWritten == PlayerPrefs.GetString("playerName"))
            {
                if (!_manager.isAdmin) ConsoleWrite($"Already signed into user {PlayerPrefs.GetString("playerName")}");
                else
                {
                    ConsoleWrite($"Successfully signed into User <{PlayerPrefs.GetString("playerName")}>");
                    _manager.isAdmin = false;
                    PlayerPrefs.SetInt("isAdmin", 0);
                }
                _manager.selectingUser = false;
            }
            else if (commandWritten == "test-user-delete-after")
            {
                if(!_manager.isAdmin)
                {
                    isConfirmingPasswordUser = true;
                    ConsoleWrite("Enter password for Admin <test-user-delete-after> or type nothing to leave");
                }
                else
                {
                    ConsoleWrite($"Already signed into user <test-user-delete-after>");
                    _manager.selectingUser = false;
                }
            }
            else
            {
                ConsoleThrowError($"No user named <{commandWritten}> exists");
                _manager.selectingUser = false;
            }
            return;
        }
        if (isDeleting)
        {
            if (commandWritten == "y")
            {
                GetComponent<fileExplorer_sex>().Progress();
            }
            else if (commandWritten == "n" && !shouldRestart)
            {
                ConsoleWrite("Cancelled delete operation.");
                isDeleting = false;
            }
            else if(!shouldRestart)
            {
                ConsoleThrowError("Please type either y or n to confirm delete operation.");
            }
            else
            {
                StartCoroutine(glitchAndRestart());
            }
            return;
        }


        if(commandWritten.Split(' ').Length == 1)
        {
            _commandManager.callCommand(commandWritten);
            return;
        }

        string inputCommand = commandWritten.Split(' ')[0];
        string argument = stringWithoutFirstElement(commandWritten.Split(' '));
        _commandManager.callCommand(inputCommand, argument);
    }

    IEnumerator glitchAndRestart()
    {
        FindObjectOfType<DigitalGlitch>().intensity = 0.1f;
        LerpSolution.StopCoroutines();
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().PlayOneShot(glitchSfx);

        yield return GeneralManager.waitForSeconds(0.1f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    string stringWithoutFirstElement(string[] splitString)
    {
        List<string> stringArray = splitString.ToList<string>();
        stringArray.RemoveAt(0);
        return string.Join(" ", stringArray.ToArray());
    }

    public void ConsoleWrite(string messageToWrite)
    {
        if (messageToWrite.ToLower().Contains("hello world") || messageToWrite.ToLower() == "hello world")
        {
            Gas.EarnAchievement("ACH_PROGRAMMING");
        }
        consoleText.text += messageToWrite + '\n';
    }

    public void ConsoleThrowError(string errorMessage)
    {
        ConsoleWrite("Error compiling command: " + errorMessage);
    }

    public void ConsoleClear()
    {
        consoleText.text = "";
    }
}
