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

    public void InstantiateCurrQuestStep(Transform parent)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parent).GetComponent<QuestStep>();
            questStep.InitQuestStep(info.id);
        }

    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if(CurrStepExists()) questStepPrefab = info.questSteps[currQuestStepIndex];
        return questStepPrefab;
    }
}
