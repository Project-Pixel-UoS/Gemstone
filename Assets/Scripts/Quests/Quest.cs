//Class for quest objects. Stores information about a quest and its state.

using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQ_NOT_MET;
        this.currQuestStepIndex = 0;
    }

    public void moveToNextStep()
    {
        currQuestStepIndex++;
    }

    public bool CurrStepExists()
    {
        return currQuestStepIndex < info.questSteps.Length; 
    }

    /// <summary>
    /// Creates a queststep game object in the scene.
    /// </summary>
    /// <param name="parent">parent to create the game object under.</param>
    public void InstantiateCurrQuestStep(Transform parent)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parent).GetComponent<QuestStep>();
            questStep.InitQuestStep(info.id);
        }

    }

    /// <summary>
    /// get current quest step prefab.
    /// </summary>
    /// <returns>current quest step prefab.</returns>
    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if(CurrStepExists()) questStepPrefab = info.questSteps[currQuestStepIndex];
        return questStepPrefab;
    }
}
