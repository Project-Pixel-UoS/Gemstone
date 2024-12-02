using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StoreInteractionScript : MonoBehaviour
{
    public GameObject cafe;
    public GameObject store;
    private SpriteRenderer storeSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storeSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        storeSprite.color = Color.gray;
        //if mouse in collider area and mouse down, transition to store prefab
        if (Input.GetMouseButtonDown(0))
        {
            cafe.SetActive(false);
            store.SetActive(true);
        }

    }
    private void OnMouseExit()
    {
        storeSprite.color = Color.white;
    }
}
