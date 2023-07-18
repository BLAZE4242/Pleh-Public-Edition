using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitTrigger2D : MonoBehaviour
{
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        playerMove _player = collision.GetComponent<playerMove>();
        _player.fadeAnim.SetTrigger("fadeOut");
        _player._infoFromScene.messageFromScene = "fade in";
        _player.lockRestart = true;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        yield return GeneralManager.waitForSeconds(2.4f);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
