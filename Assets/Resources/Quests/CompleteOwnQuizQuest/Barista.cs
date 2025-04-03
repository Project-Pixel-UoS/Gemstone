using UnityEngine;
using UnityEngine.EventSystems;

public class Barista : MonoBehaviour, IPointerClickHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var item = ItemTracker.Instance.currentItem;
        if (item != null && item.name == "Money")
        {
            Debug.Log("Barista clicked");
            ItemTracker.Instance.UseItem(item.name);
            var coffee = Instantiate(Resources.Load("Items/Coffee"), transform.parent);
            coffee.name = "Coffee";
        }
    }


}
