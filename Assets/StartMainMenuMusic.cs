using UnityEngine;

public class StartMainMenuMusic : MonoBehaviour
{
    public AudioClip mainMenuMusic;

    void Awake()
    {
        if (mainMenuMusic != null && AudioManagement.instance != null)
        {
            AudioManagement.instance.PlayMusic(mainMenuMusic);
        }
    }
}
