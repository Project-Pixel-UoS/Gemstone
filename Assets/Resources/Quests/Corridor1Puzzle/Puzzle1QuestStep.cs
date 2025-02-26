using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

public class Puzzle1QuestStep : QuestStep
{
    private string clickedScene;
    private string[] correctOrder = { "Step1", "Step2", "Step3", "Step4" };
    private List<string> clickedOrder = new List<string>();
    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            clickedScene = GetClickedScene();
            clickedOrder.Add(clickedScene);
        }
        if (clickedOrder.Count == 4)
        {
            if (CompareOrders(correctOrder, clickedOrder.ToArray()))
            {
                Debug.Log("Correct order, finsh quest!");
                FinishQuestStep();
            }
            else
            {
                clickedOrder.Clear();
                Debug.Log("Try again!");
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
}
