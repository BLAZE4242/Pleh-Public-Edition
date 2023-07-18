using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ButtonHandle : MonoBehaviour
{
    public SpriteRenderer whatif;
    [SerializeField] TextMeshProUGUI tutText;
    [SerializeField] KeyCode action = KeyCode.LeftShift;
    [SerializeField] UnityEvent OnToggle;
    [SerializeField] bool shouldSpam = false;

    [HideInInspector] public Color notTriggeredColour = new Color(0.89f, 0.07f, 0.07f);
    Color triggedColour = new Color(0.07f, 0.89f, 0.27f);

    bool couldTrigger = false;
    public bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateTutText();
            couldTrigger = true;
        }
    }

    void UpdateTutText()
    {
        if (!isTriggered) tutText.text = $"Press {action} to activate";
        else tutText.text = $"Press {action} to deactivate";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutText.text = "";
            couldTrigger = false;
        }
    }

    private void Update()
    {
        if (couldTrigger && Input.GetKeyDown(action))
        {
            isTriggered = !isTriggered;
            whatif.color = isTriggered ? triggedColour : notTriggeredColour;
            UpdateTutText();

            if(!shouldSpam) OnToggle.Invoke();
        }
        
        if (shouldSpam)
        {
            OnToggle.Invoke();
        }
    }
}
