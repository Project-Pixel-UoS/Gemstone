using UnityEngine;
using Util;

public class Puzzle1QuestStep : QuestStep
{
    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            GetClickedScene();
        }
    }

    private void GetClickedScene()
    {
        var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (clickedItem == null) return;
        
        switch(clickedItem.gameObject.name)
        {
            case "Step1": FinishQuestStep(); break;
            case "Step2": FinishQuestStep(); break;
            case "Step3": FinishQuestStep(); break;
            case "Step4": FinishQuestStep(); break;
        }
    }
}
