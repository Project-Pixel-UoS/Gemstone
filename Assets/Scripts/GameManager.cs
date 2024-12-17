using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject cafeCanvas;
    public GameObject chair;
    public GameObject table;
    public GameObject store;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //this will have to change according to starting location or prev. saved position
        cafeCanvas.SetActive(true);
        table.SetActive(false);
        store.SetActive(false);
    }

    private void Update()
    {
        if (IsMouseClicked())
        {
            GetClickedScene();
        }
    }

    //check if mouse click happens
    private bool IsMouseClicked()
    {
        return Input.GetMouseButtonDown(0);
    }

    //calculate mouse position
    private static RaycastHit2D CalculateMouseDownRaycast()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1; //the layer that the objects will collide w the raycast
        return Physics2D.Raycast(mousePos, Vector2.zero, float.PositiveInfinity, layerMask);
    }

    //select which scene to switch to depending on which collider mouse collides w/
    private void GetClickedScene()
    {
        var clickedItem = CalculateMouseDownRaycast().collider;
        switch (clickedItem.gameObject.name)
        {
            case "Store":
                OnStoreClicked();
                break;
            case "Chair":
                OnChairClicked();
                break;
            default:
                break;
        }
    }

    public void OnChairClicked()
    {
        cafeCanvas.SetActive(false);
        table.SetActive(true);
    }
    public void OnStoreClicked()
    {
        cafeCanvas.SetActive(false);
        store.SetActive(true);
    }
}
