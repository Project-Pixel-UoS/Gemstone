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
                retryCount++;
                MoveSpookyCloser(retryCount);
                Debug.Log("Try again! Attempt: " + retryCount);

            }
        }
    }

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

    private string GetClickedScene()
    {
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (clickedItem == null) { return null; }
        else { 
            return clickedItem.gameObject.name;
        }
    }
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
    private void ReparentChildren(GameObject curr, GameObject prev)
    {
        curr?.SetActive(true);
        point = prev.transform.GetChild(0).gameObject;
        steps = prev.transform.GetChild(1).gameObject;
        point.transform.SetParent(curr.transform, true);
        steps.transform.SetParent(curr.transform, true);
        prev?.SetActive(false);  
    }
    private void MoveSpookyCloser(int retries)
    { 
        backButton.SetActive(false);

        switch (retries)
        {
            case 1: ReparentChildren(fail1, corridor1); break;
            case 2: ReparentChildren(fail2, fail1); break;
            case 3: ReparentChildren(fail3, fail2); break;
            case 4: StartCoroutine(DeathScrean()); break;
        }
    }
    private IEnumerator DeathScrean()
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

}

    


