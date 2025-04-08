using UnityEngine;
using Util;


public class Bathroom : MonoBehaviour
{
    /// <summary>
    /// Handles movement within the bathroom depending on whether the quest is done or not
    /// </summary>
    private GameObject withSmoker, noSmoker, questPoint;

    private void OnEnable()
    {
        withSmoker = transform.Find("WithSmoker")?.gameObject;
        noSmoker = transform.Find("WithoutSmoker")?.gameObject;
        questPoint = transform.Find("QuestPoint")?.gameObject;
        if (!SmokerConditionDisabled())
        {
            withSmoker.SetActive(true);
            noSmoker.SetActive(false);
            questPoint.SetActive(true);
        }
        else
        {
            withSmoker.SetActive(false);
            noSmoker.SetActive(true);
            questPoint.SetActive(false);
        }
    }
    private void Update()
    {
        if (Utils.IsMouseClicked() && Utils.CheckMousePosInsideStage("GameStage"))
        {
            var clickedItem = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
            if (clickedItem != null && clickedItem.gameObject.name == "Smoker")
            {
                withSmoker.SetActive(false);
                noSmoker.SetActive(true);
                questPoint?.SetActive(false);
            }

        }
    }
    private bool SmokerConditionDisabled()
    { 
        return BathroomPuzzle.isFinished;
    }
}
