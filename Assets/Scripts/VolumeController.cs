using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip endDragSound; 

    [SerializeField] private string volumeParam = "MasterVolume"; // Ensure this matches the exposed parameter in the AudioMixer (Default: "MasterVolume")

    private void Start()
    {
        // Set the slider to the default value (1 is max volume)
        volumeSlider.value = 1f;

        // Set the initial volume based on the slider value
        SetVolume(volumeSlider.value);

        // Add listener to handle changes in slider value
        volumeSlider.onValueChanged.AddListener(SetVolume);

    }

    // Adjust volume based on slider value
    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat(volumeParam, Mathf.Log10(sliderValue) * 20); // Convert linear to logarithmic scale
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        audioSource.PlayOneShot(endDragSound);
    }
}
