using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Amelia_Confess_Manager : MonoBehaviour
{
    [SerializeField] Image blackScreen;

    private void Start()
    {
        FindObjectOfType<DRP>().RP("IT MADE ME DO THIS.");
        GeneralManager.SetSceneSave();
    }

    public void PickUpPhone()
    {
        StartCoroutine(PickUpPhoneEnum());
    }

    IEnumerator PickUpPhoneEnum()
    {
        blackScreen.gameObject.SetActive(true);
        yield return GeneralManager.waitForSeconds(2f);
        SceneManager.LoadScene("New Messages");
    }
}
