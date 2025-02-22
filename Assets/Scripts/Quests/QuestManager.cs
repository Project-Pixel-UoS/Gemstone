//Quest Manager script. Mostly a blackbox. responsible for starting, advancing, and finishing quests. 

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
    }

    //constantly check if there are any quests that can be started.
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

    //initialise all quests in the game.
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

    /// <summary>
    /// checks quest prerequisites for quest to start.
    /// </summary>
    /// <param name="quest">quest to be checked for prereqs.</param>
    /// <returns>true if all quest prereqs are complete, false otherwise.</returns>
    private bool CheckReqs(Quest quest)
    {
        bool meetsReqs = true;
        foreach (QuestInfoSO prereqQuestInfo in quest.info.questPrereqs)
        {
            if (GetQuestByID(prereqQuestInfo.id).state != QuestState.FINISHED) meetsReqs = false;
        }
        return meetsReqs;
    }

    /// <summary>
    /// returns quest object by id (name).
    /// </summary>
    /// <param name="id">id (name) of the quest</param>
    /// <returns>quest object in questMap dict.</returns>
    private Quest GetQuestByID(string id)
    {
        return questMap[id];
    }

    /// <summary>
    /// instantiates first quest step object under QuestManager.
    /// </summary>
    /// <param name="id">name of questSO to start.</param>
    private void StartQuest(string id)
    {
        Debug.Log("start quest");

        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    /// <summary>
    /// if a questSO contains > 1 quest step, move onto next quest step, else update quest state.
    /// </summary>
    /// <param name="id">name of questSO to advance.</param>
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

    /// <summary>
    /// update questSO to CAN_FINISH.
    /// </summary>
    /// <param name="id">name of questSO to finish.</param>
    private void FinishQuest(string id)
    {
        Debug.Log("finish quest");

        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    /// <summary>
    /// give rewards to player after quest is completed.
    /// </summary>
    /// <param name="quest">the quest that was completed.</param>
    private void ClaimRewards(Quest quest)
    {
        //TODO: add some rewards (game object?)
        Debug.Log("Quest finished");
    }
}
