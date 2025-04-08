using UnityEngine;
using UnityEngine.Rendering;

public class BathroomPuzzle : QuestStep
{
    /// <summary>
    /// Need to provide a smoke detector game object which can be placed in the bathroom 
    /// for the quest to complete which results in clearing up the smoke in one of the lab.
    /// </summary>

    private GameObject smokeDetector, placementLocation;
    public static bool isFinished = false;


    void Start()
    {
        smokeDetector = GameObject.Find("SmokeDetector");
        placementLocation = GameObject.Find("DetectorPlacement");
    }

    // Update is called once per frame
    void Update()
    {
        if (smokeDetector.GetComponent<Collider2D>().IsTouching(placementLocation.GetComponent<Collider2D>()))
        {
            FinishQuestStep();
            isFinished = true;
        }
    }

}