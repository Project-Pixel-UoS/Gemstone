using UnityEngine;

public class TriggerRoomMusic : MonoBehaviour
{
    public AudioClip roomMusic;

    void OnEnable()
    {
        if (roomMusic != null && AudioManagement.instance != null)
        {
            AudioManagement.instance.PlayMusic(roomMusic);
        }
    }
}
