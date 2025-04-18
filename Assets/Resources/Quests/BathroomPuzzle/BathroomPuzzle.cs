using UnityEngine;
using UnityEngine.Rendering;

public class BathroomPuzzle : QuestStep
{
    /// <summary>
    /// Need to provide a smoke detector game object which can be placed in the bathroom 
    /// for the quest to complete which results in clearing up the smoke in one of the lab.
    /// </summary>

    private GameObject smokeDetector, placementLocation, a;
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
            Vector3 spawnPoint = smokeDetector.transform.position;
            Destroy(smokeDetector);
            ItemTracker.Instance.itemEvents.ItemRemoved();
            a = Resources.Load("Quests/BathroomPuzzle/SmokeDetector") as GameObject;
            Debug.Log(spawnPoint);
            Instantiate(a, spawnPoint, Quaternion.identity, placementLocation.transform);
            isFinished = true;
        }
    }

}