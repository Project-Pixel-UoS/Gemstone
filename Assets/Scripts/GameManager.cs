using UnityEngine;
using Util;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;
using JetBrains.Annotations;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject cafe;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject mainHall;
    [SerializeField] private GameObject sign;
    [SerializeField] private GameObject elevator;
    [SerializeField] private GameObject backButton;

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
        mainHall.SetActive(true);
        backButton.SetActive(false);
        cafe.SetActive(false);
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
            case "Sign":
                OnSignClicked();
                break;
            case "Elevator":
                OnElevatorClicked();
                break;
            case "BackButton":
                OnBackButtonClicked();
                break;
            case "Corridor 1":
                OnCorridor1Clicked();
                break;
            default:
                break;
        }
    }
    public void OnSignClicked()
    {
        mainHall.SetActive(false);
        cafe.SetActive(true);
        backButton.SetActive(true);
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
    public void OnElevatorClicked()
    {
        SceneManager.LoadScene("First Floor");
    }
    public void OnCorridor1Clicked()
    {
        GameObject panel = GameObject.FindWithTag("GameStage");
        //transform.Find instead of gameObject.find so you can find inactive gameObjects
        GameObject reception = panel.transform.Find("Room: Reception").gameObject;
        GameObject corridor1 = panel.transform.Find("Room: Corridor 1").gameObject;
        reception.SetActive(false);
        corridor1.SetActive(true);
    }
    public void OnBackButtonClicked()
    {
        //note: need to make our own back button graphic to avoid copyright lol
        if (table.activeSelf == true || store.activeSelf == true)
        {
            cafe.SetActive(true);
            table.SetActive(false);
            store.SetActive(false);
        }
        else
        {
            mainHall.SetActive(true);
            cafe.SetActive(false);
            backButton.SetActive(false);
        }
    }
}
