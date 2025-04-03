using UnityEngine;
using UnityEngine.EventSystems;

public class Barista : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// When the barista is clicked, the player will use the money item and receive a coffee item.
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: Trigger starting dialogue
        var item = ItemTracker.Instance.currentItem;
        if (item != null && item.name == "Money")
        {
            Debug.Log("Barista clicked");
            ItemTracker.Instance.UseItem(item.name);
            var coffee = Instantiate(Resources.Load("Items/Coffee"), transform.parent);
            coffee.name = "Coffee";
        }//TODO: else trigger dialogue for not having money/incorrect item
    }


}
