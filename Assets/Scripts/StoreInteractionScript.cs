using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StoreInteractionScript : MonoBehaviour
{
    public GameObject store;
    public GameObject canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseEnter()
    {
        //if mouse in collider area and mouse down, transition to store prefab
        if (Input.GetMouseButtonDown(0))
        {
            canvas.SetActive(false);
            store.SetActive(true);
        }
    }
}
