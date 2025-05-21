using UnityEngine;
using UnityEngine.EventSystems;
using Util;

public class MeetingRoom : MonoBehaviour
{
    private GameObject monster, empty;
    public static bool bookCollected = false;

    private void OnEnable()
    {
        monster = transform.Find("Monster")?.gameObject;
        empty = transform.Find("Empty")?.gameObject;
        if(bookCollected)
        { 
            monster.SetActive(false);
            empty.SetActive(true);
        }
    }
    void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
            if (clickedItem != null && clickedItem.gameObject.name == "Book")
            {
                monster.SetActive(false);
                empty.SetActive(true);
                bookCollected = true;
            }

        }
    }
}
