using UnityEngine;

public class TriggerRoomMusic : MonoBehaviour
{
    public AudioClip roomMusic;

    void Awake()
    {
        if (roomMusic != null && AudioManagement.instance != null)
        {
            AudioManagement.instance.PlayMusic(roomMusic);
        }
    }
}
