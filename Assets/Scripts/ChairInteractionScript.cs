using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ChairInteractionScript : MonoBehaviour
{
    public GameObject canvas;
    public GameObject table;
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
        //if mouse in collider area and mouse down, transition to table prefab
        if (Input.GetMouseButtonDown(0))
        {
            canvas.SetActive(false);
            table.SetActive(true);
        }
    }
}



