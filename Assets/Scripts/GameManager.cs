using UnityEngine;
using Util;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject cafe;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject store;

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
        cafe.SetActive(true);
        table.SetActive(false);
        store.SetActive(false);
    }

    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            GetClickedScene();
        }
    }

    //select which scene to switch to depending on which collider mouse collides w/
    private void GetClickedScene()
    {
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (clickedItem == null) return;
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
        cafe.SetActive(false);
        table.SetActive(true);
    }
    public void OnStoreClicked()
    {
        cafe.SetActive(false);
        store.SetActive(true);
    }
}
