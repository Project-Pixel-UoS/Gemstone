using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Ground Floor");
        // StartCoroutine(GameTransition());
    }

    private IEnumerator GameTransition()
    {
        // yield return StartCoroutine(GameManager.instance.RoomTransitionFade(true));
        // SceneManager.LoadScene("Ground Floor");
        yield return null;
    }
}
