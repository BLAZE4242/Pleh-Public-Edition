using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class PauseScreenOption : MonoBehaviour
{
    public string optionName;
    public enum pauseOptionType { action, toggle, option }
    public pauseOptionType TypeOfOption;

    // action option variables
    [SerializeField] UnityEvent OnOptionSelect;

    [Header("Option settings")]
    public List<string> settingOptions = new List<string>();

    public void OptionPress()
    {
        OnOptionSelect.Invoke();
    }

}
