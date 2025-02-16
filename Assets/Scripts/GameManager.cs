using UnityEngine;
using Util;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;
using JetBrains.Annotations;
using UnityEditor.Animations;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //references to all GameObjects
    private GameObject panel, mainHall, cafe, table, store, reception, corridor1, backButton, backButton2;

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
        //may need to change depending on previous saved location
        InitialiseScene();
    }

    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            GetClickedScene();
        }
    }

    /// <summary>
    /// Finds all the GameObjects in ground floor and caches them in their 
    /// respective variable, this is done to ensure that the objects can be 
    /// referenced when switching from first floor to ground floor.
    /// </summary>
    private void CacheGameObjects()
    {
        panel = GameObject.FindWithTag("GameStage");
        if (panel == null) return;

        mainHall = panel.transform.Find("Room: Main Hall")?.gameObject;
        cafe = panel.transform.Find("Room: Cafe")?.gameObject;
        table = panel.transform.Find("Room: Table")?.gameObject;
        store = panel.transform.Find("Room: Shop Front")?.gameObject;
        backButton = panel.transform.Find("BackButton")?.gameObject;
        reception = panel.transform.Find("Room: Reception")?.gameObject;
        corridor1 = panel.transform.Find("Room: Corridor 1")?.gameObject;
        backButton2 = panel.transform.Find("BackButton2")?.gameObject;

    }
    
    /// <summary>
    /// Intialises the ground floor scene
    /// </summary>
    private void InitialiseScene()
    {
        if (mainHall == null || backButton == null) return;

        mainHall.SetActive(true);
        backButton.SetActive(false);
        cafe?.SetActive(false);
        table?.SetActive(false);
        store?.SetActive(false);
    }

    //select which scene to switch to depending on which collider mouse collides w/
    private void GetClickedScene()
    {
        CacheGameObjects();
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (clickedItem == null) return;

        switch (clickedItem.gameObject.name)
        {
            case "Store": OnStoreClicked(); break;
            case "Chair": OnChairClicked(); break;
            case "Sign": OnSignClicked(); break;
            case "Elevator": OnElevatorClicked(); break;
            case "BackButton": OnBackButtonClicked(); break;
            case "BackButton2": OnBackButton2Clicked(); break;
            case "Corridor 1": OnCorridor1Clicked(); break;
        }
    }
    /// <summary>
    /// Loops through each child of the panel transform and deactivates them,
    /// then activates only the room that you want to show.
    /// </summary>
    /// <param name="roomToShow"></param>
    private void SwitchRooms(GameObject roomToShow)
    {
        if (panel == null || roomToShow == null) return;

        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(false);
        }

        roomToShow.SetActive(true);
        if (roomToShow != mainHall) backButton?.SetActive(true);
    }
    /// <summary>
    /// Room switching functions
    /// </summary>
    public void OnSignClicked() => SwitchRooms(cafe);
    public void OnChairClicked() => SwitchRooms(table);
    public void OnStoreClicked() => SwitchRooms(store);
    public void OnCorridor1Clicked() => SwitchRooms(corridor1);    
    public void OnElevatorClicked() => SceneManager.LoadScene("First Floor");

    public void OnBackButtonClicked()
    {
        //note: need to make our own back button graphic to avoid copyright
        if (table?.activeSelf == true || store?.activeSelf == true)
        {
            SwitchRooms(cafe);
        }
        else
        {
            SwitchRooms(mainHall);
            backButton?.SetActive(false);
        }
    }

    public void OnBackButton2Clicked()
    {
        if (reception?.activeSelf == true)
        {
            SceneManager.LoadScene("Ground Floor");
        }
        else
        {
            SwitchRooms(reception);
        }
    }
}
