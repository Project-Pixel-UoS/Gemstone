using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StoreInteractionScript : MonoBehaviour
{
    public GameObject cafe;
    public GameObject store;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        //if mouse in collider area and mouse down, transition to store prefab
        if (Input.GetMouseButtonDown(0))
        {
            cafe.SetActive(false);
            store.SetActive(true);
        }
    }
}
