using System;
using System.Collections;
using UnityEngine;
using Util;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    //references to all GameObjects
    private GameObject panel, mainHall, cafe, table, store, reception, corridor1, backButton, backButton2, bathroom, 
                        inventory, tableContainer;

    public bool isDay = true;
    private List<string> allowedRooms = new List<string>();
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

        tableContainer = GameObject.Find("TableContainer");
        if (tableContainer != null)
        {
            tableContainer.SetActive(false); 
            
        }
    }

    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            GetClickedScene();
        }
    }

    public void AllowRoom(string roomName)
    {
        if (!allowedRooms.Contains(roomName))
        {
            allowedRooms.Add(roomName);
        }
    }
    public void DisallowRoom(string roomName)
    {
        if (allowedRooms.Contains(roomName))
        {
            allowedRooms.Remove(roomName);
        }
    }
    private bool IsRoomAllowed(string roomName)
    {
        return true;
        return allowedRooms.Contains(roomName);
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
        bathroom = panel.transform.Find("Room: Bathroom")?.gameObject;
        backButton2 = panel.transform.Find("BackButton2")?.gameObject;
        inventory = panel.transform.Find("Inventory")?.gameObject;

    }
    
    /// <summary>
    /// Intialises the ground floor scene
    /// </summary>
    private void InitialiseScene()
    {
        // StartCoroutine(RoomTransitionFade(true));
        StartCoroutine(RoomTransitionFade(false));

        if (mainHall == null || backButton == null) return;

        mainHall?.SetActive(true);
        //inventory?.SetActive(false);
        cafe?.SetActive(false);
        table?.SetActive(false);
        store?.SetActive(false);
    }

    //select which scene to switch to depending on which collider mouse collides w/
    private void GetClickedScene()
    {
        if (DialogueHandler.IsActive())
        {
            return;
        }
        
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
            case "Corridor1": OnCorridor1Clicked(); break;
            case "Bathroom": OnBathroomClicked(); break;
        }
    }

    private void SwitchRooms(GameObject roomToShow, string entranceDialogueTag = null)
    {
        StartCoroutine(SwitchRoomsIE(roomToShow, entranceDialogueTag));
    }

    
    /// <summary>
    /// Loops through each child of the panel transform and deactivates them,
    /// then activates only the room that you want to show.
    /// </summary>
    /// <param name="roomToShow"></param>
    private IEnumerator SwitchRoomsIE(GameObject roomToShow, string entranceDialogueTag)
    {
        yield return StartCoroutine(RoomTransitionFade(true));
        yield return new WaitForSeconds(0.5f);
        
        if (panel == null || roomToShow == null) yield break;
        Debug.Log(roomToShow.name);
        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(false);
        }

        roomToShow.SetActive(true);
        backButton?.SetActive(true);
        
        yield return StartCoroutine(RoomTransitionFade(false));
        // yield return new WaitForSeconds(0.3f);
        if (entranceDialogueTag != null)
        {
            DialogueHandler.PlayDialogue(entranceDialogueTag);
        }
    }
    
    public IEnumerator RoomTransitionFade(bool fadeIn)
    {
        Image overlayImage = GameObject.Find("TransitionOverlay")?.GetComponent<Image>();
        if (overlayImage.IsUnityNull())
        {
            GameObject overlay = new GameObject("TransitionOverlay");
            overlay.transform.SetParent(GameObject.Find("Canvas").transform, false);
            overlayImage = overlay.AddComponent<Image>();
            overlayImage.color = new Color(0f, 0f, 0f, 0f);

            RectTransform rectTransform = overlay.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            overlay.transform.SetSiblingIndex(overlay.transform.childCount - 2);
        }
        
        const int fadeDuration = 1;
        
        for (float time = 0; time < fadeDuration; time += Time.deltaTime)
        {
            float alpha;
            if (fadeIn)
            {
                // Debug.Log("Fading In");
                alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            }
            else
            {
                // Debug.Log("Fading Out");
                alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            }
            overlayImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (fadeIn)
        {
            overlayImage.color = new Color(0, 0, 0, 1);
        }
        else
        {
            overlayImage.color = new Color(0, 0, 0, 0);
            DestroyTransitionOverlay();
        }
    }

    private void DestroyTransitionOverlay()
    {
        Destroy(GameObject.Find("TransitionOverlay"));
    }
    
    
    /// <summary>
    /// Room switching functions
    /// </summary>
    public void OnSignClicked() => SwitchRooms(cafe, "cafe_morning");
    public void OnChairClicked()
    {
        if (IsRoomAllowed("table"))
        {
            SwitchRooms(table, "table_morning");
        }
        else
        {
            DialogueHandler.PlayDialogue("table_fail");
        }
    }
    public void OnStoreClicked() => SwitchRooms(store, "shopfront_morning");
    public void OnCorridor1Clicked() => SwitchRooms(corridor1, "corridor1");
    public void OnBathroomClicked() => SwitchRooms(bathroom);
    public void OnElevatorClicked()
    {
        if (IsRoomAllowed("elevator"))
        {
            SceneManager.LoadScene("First Floor");
            // StartCoroutine(OnElevatorClickedCoroutine());
        }
        else
        {
            DialogueHandler.PlayDialogue("elevator_fail");
        }
    }

    private IEnumerator OnElevatorClickedCoroutine()
    {
        yield return StartCoroutine(RoomTransitionFade(true));
        SceneManager.LoadScene("First Floor");
        yield return StartCoroutine(RoomTransitionFade(false));
    }
    public void OnBackButtonClicked()
    {
        //note: need to make our own back button graphic to avoid copyright
        if (table?.activeSelf == true)
        {
            SwitchRooms(cafe);
        }
        else if (store?.activeSelf == true)
        {
            SwitchRooms(cafe);
            DialogueHandler.PlayDialogue("cafe_morning2");
        }
        else
        {
            SwitchRooms(mainHall);
        }

        if (mainHall?.activeSelf == true)
        {
            SceneManager.LoadScene("Main Menu");
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


    private void OnMouseDown()
    {
        if (tableContainer != null)
        {
            tableContainer.SetActive(true);
        }
    }

    /*public void HideScreen()
    {
        GameObject greyTable = GameObject.Find("roomTable");
        if (greyTable != null)
        {
            greyTable.SetActive(true);
        }
    }*/
    public void ChangeBackground(Sprite newBackground, string roomName)
    {
        Debug.Log("Changing background of " + roomName);
        if(roomName == "Room: Cafe")
        {
            cafe.GetComponent<Image>().sprite = newBackground;
        }
        else if (roomName == "Room: Main Hall")
        {
            mainHall.GetComponent<Image>().sprite = newBackground;
        }
    }
}
