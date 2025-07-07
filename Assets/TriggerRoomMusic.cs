using UnityEngine;

public class TriggerRoomMusic : MonoBehaviour
{
    public AudioClip roomMusic;
    public AudioClip nightRoomMusic;

    void OnEnable()
    {
        if (GameManager.instance != null && GameManager.instance.isDay)
        {
            if (roomMusic != null && AudioManagement.instance != null)
            {
                AudioManagement.instance.PlayMusic(roomMusic);
            }
        }
        else if (GameManager.instance != null && !GameManager.instance.isDay)
        {
            //ToDo - Use index of Quiz Quest to determine if it is night, then change the music 

            if (nightRoomMusic != null && AudioManagement.instance != null)
            {
                AudioManagement.instance.PlayMusic(nightRoomMusic);
            }
        }
    }
}
