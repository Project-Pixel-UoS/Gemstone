using UnityEngine;
using Util;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;

    private string questID;
    private QuestState questState;

    private void Awake()
    {
        questID = questForPoint.id;
        questState = QuestState.REQ_NOT_MET;
    }

    private void Update()
    {
        if (Utils.IsMouseClicked() && QuestPointInteract())
        {
            if(questState.Equals(QuestState.CAN_START) && startPoint)
            {
                QuestManager.Instance.questEvents.StartQuest(questID);
            }else if(questState.Equals(QuestState.CAN_FINISH) && endPoint)
            {
                QuestManager.Instance.questEvents.FinishQuest(questID);
            }
        }
    }

    private void Start()
    {
        QuestManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnEnable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
            Debug.Log("reenabled quest point " + questState);
        }
    }

    /* uncomment in future in case of performance issue.
    private void OnDisable()
    {
        QuestManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
        Debug.Log("disabled quest point " + questState);
    }*/

    //checks if quest point has correct quest, then change state.
    private void QuestStateChange(Quest quest)
    {
        if(quest.info.id.Equals(questID))
        {
            questState = quest.state;
            print("actually changed state" + quest.state);
        }
    }

    //checks if quest point is clicked.
    private bool QuestPointInteract()
    {
        var item = Utils.CalculateMouseDownRaycast(LayerMask.GetMask("Default")).collider;
        if (item != null && item.transform.tag.Equals("QuestPoint") && item.transform.Equals(this.transform))
        {
            return true;
        }
        return false;
    }
}
