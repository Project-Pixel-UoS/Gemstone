using UnityEngine;
using Util;



/// <summary>
/// Handles movement within the bathroom depending on whether the quest is done or not
/// </summary>
public class Bathroom : MonoBehaviour
{
    private GameObject withSmoker, noSmoker, questPoint, smokeDetector, placementLocation;

    private void OnEnable()
    {
        //finding and setting all the gameObjects needed for navigation
        withSmoker = transform.Find("WithSmoker")?.gameObject;
        noSmoker = transform.Find("WithoutSmoker")?.gameObject;
        questPoint = transform.Find("QuestPoint")?.gameObject;
        placementLocation = transform.Find("DetectorPlacement")?.gameObject;
        smokeDetector = transform.Find("SmokeDetector")?.gameObject;
    
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
            FixDetectorLocation();
        }
    }
    private void Update()
    {
        //clicking smoker moves player to clean bathroom, to-do: add dialogue
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
    /// <summary>
    /// Checks if the quest is complete 
    /// </summary>
    /// <returns></returns>
    private bool SmokerConditionDisabled()
    { 
        return BathroomPuzzle.isFinished;
    }

    /// <summary>
    /// Fixes the detector location once the quest is completed.
    /// Needed when reloading the scene otherwise the detector is in the wrong place.
    /// </summary>
    private void FixDetectorLocation()
    {
        smokeDetector.transform.SetParent(placementLocation.transform, true);
        smokeDetector.transform.position = placementLocation.transform.position;
        smokeDetector.tag = "Untagged";
        Draghandler.isLocked = true;
    }    
}
