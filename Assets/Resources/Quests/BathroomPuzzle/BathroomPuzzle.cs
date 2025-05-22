using UnityEngine;
using UnityEngine.Rendering;

public class BathroomPuzzle : QuestStep
{
    /// <summary>
    /// Need to provide a smoke detector game object which can be placed in the bathroom 
    /// for the quest to complete which results in clearing up the smoke in one of the labs.
    /// </summary>

    private GameObject smokeDetector, placementLocation;
    public static bool isFinished = false;


    void Start()
    {
        smokeDetector = GameObject.Find("SmokeDetector");
        placementLocation = GameObject.Find("DetectorPlacement");
    }

    
    void Update()
    {
        //checks if smoke detector is touching the placement location
        if (smokeDetector.GetComponent<Collider2D>().IsTouching(placementLocation.GetComponent<Collider2D>()))
        {
            FinishQuestStep();
            //reparent and untag smoke detector so it doesn't overlap other scenes when leaving bathroom
            smokeDetector.transform.SetParent(placementLocation.transform, true); 
            smokeDetector.tag = "Untagged";
            isFinished = true;
        }
    }

}