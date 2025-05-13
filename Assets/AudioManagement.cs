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
    public void PlayMusic(AudioClip newClip)
    {
        float fadeTime = 1f; // seconds
        float originalVolume = musicSource.volume;

        if (musicSource == null || newClip == null) return;

        // If the same clip is already playing, do nothing
        if (musicSource.clip == newClip && musicSource.isPlaying) return;

        // Fade out current music
        LeanTween.value(gameObject, musicSource.volume, 0f, fadeTime).setOnUpdate((float val) =>
        {
            musicSource.volume = val;
        }).setOnComplete(() =>
        {
            // Switch clip and play new one
            musicSource.clip = newClip;
            musicSource.loop = true;
            musicSource.Play();

            // Fade in new music
            LeanTween.value(gameObject, 0f, originalVolume, fadeTime).setOnUpdate((float val) =>
            {
                musicSource.volume = val;
            });
        });
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
