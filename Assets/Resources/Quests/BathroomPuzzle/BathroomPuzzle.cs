using UnityEngine;
using UnityEngine.Rendering;

public class BathroomPuzzle : QuestStep
{
    /// <summary>
    /// Need to provide a smoke detector game object which can be placed in the bathroom 
    /// for the quest to complete which results in clearing up the smoke in one of the lab.
    /// </summary>

    private GameObject smokeDetector, placementLocation, newSmoke;
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
            Vector3 spawnPoint = placementLocation.transform.position;
            Destroy(smokeDetector);
            ItemTracker.Instance.itemEvents.ItemRemoved();
            newSmoke = Resources.Load("Quests/BathroomPuzzle/SmokeDetector") as GameObject;
            Instantiate(newSmoke, spawnPoint, Quaternion.identity, GameObject.Find("Room: Bathroom").transform);
            Draghandler.isLocked = true;
            isFinished = true;
        }
    }

}