using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    private Dictionary<string, Quest> questMap;
    public QuestEvent questEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Item Events Manager in the scene.");
        }
        Instance = this;

        questMap = CreateQuestMap();
        questEvents = new QuestEvent();
        questEvents.onStartQuest += StartQuest;
        questEvents.onAdvanceQuest += AdvanceQuest;
        questEvents.onFinishQuest += FinishQuest;
    }

    private void Start()
    {
        //broadcast all initial quest states on startup
        foreach (Quest quest in questMap.Values)
        {
            questEvents.QuestStateChange(quest);
        }
        UpdateQuestStates();
    }

    //constantly check if there are any quests taht can be started.
    private void Update()
    {
        UpdateQuestStates();
    }

    private void UpdateQuestStates()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQ_NOT_MET && CheckReqs(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    private void OnDisable()
    {
        questEvents.onStartQuest -= StartQuest;
        questEvents.onAdvanceQuest -= AdvanceQuest;
        questEvents.onFinishQuest -= FinishQuest;
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach(QuestInfoSO quest in allQuests)
        {
            if(!idToQuestMap.ContainsKey(quest.id)) idToQuestMap.Add(quest.id, new Quest(quest));
        }
        return idToQuestMap;
    }
    
    /// <summary>
    /// updates quest state.
    /// </summary>
    /// <param name="id">id of quest to be changed.</param>
    /// <param name="state">what state to change to.</param>
    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        questEvents.QuestStateChange(quest); //broadcast event
        Debug.Log("updating quest "+quest.info.id+" to "+ quest.state);
    }

    private bool CheckReqs(Quest quest)
    {
        bool meetsReqs = true;
        foreach (QuestInfoSO prereqQuestInfo in quest.info.questPrereqs)
        {
            if (GetQuestByID(prereqQuestInfo.id).state != QuestState.FINISHED) meetsReqs = false;
        }
        return meetsReqs;
    }

    private Quest GetQuestByID(string id)
    {
        return questMap[id];
    }

    private void StartQuest(string id)
    {
        Debug.Log("start quest");

        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }
    private void AdvanceQuest(string id)
    {
        Debug.Log("advance quest");

        Quest quest = GetQuestByID(id);
        quest.moveToNextStep();

        if(quest.CurrStepExists())
        {
            quest.InstantiateCurrQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }
    private void FinishQuest(string id)
    {
        Debug.Log("finish quest");

        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        //TODO: add some rewards (game object?)
        Debug.Log("Quest finished");
    }
}
