using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questID;

    public void InitQuestStep(string questID)
    {
        this.questID = questID;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            QuestManager.Instance.questEvents.AdvanceQuest(questID);
            Destroy(this.gameObject);
        }
    }
}
