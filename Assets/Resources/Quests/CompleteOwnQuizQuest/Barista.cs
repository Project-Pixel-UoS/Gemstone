using UnityEngine;
using UnityEngine.EventSystems;

public class Barista : MonoBehaviour, IPointerClickHandler
{
    public AudioClip coffeeClip;
    /// <summary>
    /// When the barista is clicked, the player will use the money item and receive a coffee item.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: Trigger starting dialogue
        Debug.Log("Barista clicked");
        var item = ItemTracker.Instance.currentItem;
        if (item != null && item.name == "Money")
        {
            Debug.Log("Barista clicked");
            DialogueHandler.PlayDialogue("barista_success");
            AudioManagement.instance.PlaySFX(coffeeClip);
            GameManager.instance.AllowRoom("table");
            GameManager.instance.AllowRoom("elevator");
            ItemTracker.Instance.UseItem(item.name);
            var coffee = Instantiate(Resources.Load("Items/Coffee"), transform.parent);
            coffee.name = "Coffee";
        }
        else
        {
            DialogueHandler.PlayDialogue("barista_fail", true);
        }
    }
}
