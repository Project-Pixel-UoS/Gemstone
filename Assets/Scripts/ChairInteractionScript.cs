using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ChairInteractionScript : MonoBehaviour
{
    public GameObject cafe;
    public GameObject table;
    private SpriteRenderer chairSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chairSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseOver()
    {
        chairSprite.color = new Color (188,188,188);
        //if mouse in collider area and mouse down, transition to table prefab
        if (Input.GetMouseButtonDown(0))
        {
            cafe.SetActive(false);
            table.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        chairSprite.color= Color.white;
    }
}
