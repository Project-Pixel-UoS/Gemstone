using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ChairInteractionScript : MonoBehaviour
{
    public GameObject cafe;
    public GameObject table;
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
        //if mouse in collider area and mouse down, transition to table prefab
        if (Input.GetMouseButtonDown(0))
        {
            cafe.SetActive(false);
            table.SetActive(true);
        }
    }
}
