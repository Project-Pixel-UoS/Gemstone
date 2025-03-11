using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Util;
using static UnityEngine.Rendering.DebugUI;

public class Puzzle1QuestStep : QuestStep
{
    public GameObject selectionMarker;
    private GameObject fail1, fail2, fail3, fail4, corridor1, backButton, point, steps, panel;
    private string clickedScene;
    private string[] correctOrder = { "Step1", "Step2", "Step3", "Step4" };
    private List<string> clickedOrder = new List<string>();
    private int retryCount = 0;

    private void OnEnable()
    {
        StoreObjects();
    }
    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            clickedScene = GetClickedScene();
            clickedOrder.Add(clickedScene);
            PlaceSelectionMarker();
            Debug.Log(clickedScene);
        }
        if (clickedOrder.Count == 4)
        {
            if (CompareOrders(correctOrder, clickedOrder.ToArray()))
            {
                FinishQuestStep();
                backButton.SetActive(true);
            }
            else
            {
                clickedOrder.Clear();
                DestroySelectionMarkers();
                retryCount++;
                MoveSpookyCloser(retryCount);
                Debug.Log("Try again! Attempt: " + retryCount);

            }
        }
    }

    /// <summary>
    /// Compares the order of clicked items to the correct order
    /// </summary>
    /// <param name="correct"></param>
    /// <param name="clicked"></param>
    /// <returns>true if correct, false is incorrect</returns>
    private bool CompareOrders(string[] correct, string[] clicked)
    {
        int correctCount = 0;
        for (int i = 0; i < correct.Length; i++)
        {
            if (correct[i] == clicked[i])
            {
                correctCount++;
            }
        }
        if (correctCount == correct.Length)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Finds object that you are clicking
    /// </summary>
    /// <returns>name of the gameobject clicked</returns>
    private string GetClickedScene()
    {
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (clickedItem == null) { return null; }
        else { 
            return clickedItem.gameObject.name;
        }
    }
    /// <summary>
    /// Stores all the objects in the scene so they can be accessed throughout the script
    /// This is done because you can't place gameobjects onto a prefab script in the inspector.
    /// </summary>
    private void StoreObjects()
    {
        panel = GameObject.FindWithTag("GameStage");
        backButton = GameObject.FindWithTag("BackButton");
        corridor1 = panel.transform.Find("Room: Corridor 1").gameObject;
        fail1 = panel.transform.Find("Fail1").gameObject;
        fail2 = panel.transform.Find("Fail2").gameObject;
        fail3 = panel.transform.Find("Fail3").gameObject;
        fail4 = panel.transform.Find("Fail4").gameObject;
    }
    /// <summary>
    /// Reparents the children of the previous attempt of the puzzle to the next,
    /// this is used because each attempt has the same things in it so this removes
    /// the need for duplicating gameobjects
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="prev"></param>
    private void ReparentChildren(GameObject curr, GameObject prev)
    {
        curr?.SetActive(true);
        point = prev.transform.GetChild(0).gameObject;
        steps = prev.transform.GetChild(1).gameObject;
        point.transform.SetParent(curr.transform, true);
        steps.transform.SetParent(curr.transform, true);
        prev?.SetActive(false);  
    }
    /// <summary>
    /// Moves spooky guy closer with each fail
    /// </summary>
    /// <param name="retries"></param>
    private void MoveSpookyCloser(int retries)
    { 
        backButton.SetActive(false);

        switch (retries)
        {
            case 1: ReparentChildren(fail1, corridor1); break;
            case 2: ReparentChildren(fail2, fail1); break;
            case 3: ReparentChildren(fail3, fail2); break;
            case 4: StartCoroutine(DeathScreen()); break;
        }
    }
    /// <summary>
    /// This is used to show a "death" screen when you fail the final attempt
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathScreen()
    {
        ReparentChildren(corridor1, fail3);

        fail4?.SetActive(true);
        fail3?.SetActive(false);
        backButton?.SetActive(false);
        retryCount = 0;
        
        yield return new WaitForSeconds(2);
        
        corridor1?.SetActive(true);
        backButton?.SetActive(true);
        fail4?.SetActive(false);

    }
    //wip: places selection marker, currently no way to destory them.
    private void PlaceSelectionMarker()
    {
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        Vector3 targetPos = clickedItem.transform.position;
        if (clickedItem != null)
        {
            Instantiate(selectionMarker, targetPos, Quaternion.identity,
                GameObject.FindGameObjectWithTag("GameStage").transform);
        }
    }
    private void DestroySelectionMarkers()
    {
        foreach(Transform child in panel.transform)
        {
            if (child.gameObject.name == "SelectionMarker(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }

}




