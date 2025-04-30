using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // Assign in Inspector
    [SerializeField] private AudioClip buttonClickSound; // Assign in Inspector

    public void PlaySound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
