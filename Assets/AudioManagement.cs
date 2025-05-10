using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public static AudioManagement instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Play background music.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {       
        if (sfxSource != null && clip != null) //If the clip and SFXSource have been properly assigned and they exist
        {
            if (musicSource.clip == clip && musicSource.isPlaying) return;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Play a sound effect once.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {   
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Stop music playback.
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
