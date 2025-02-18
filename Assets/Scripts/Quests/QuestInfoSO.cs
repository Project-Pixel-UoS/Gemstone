//A scriptable object for the structure of a Quest.
//Contains info for quest name, prerequisites, any steps (subquests)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public QuestInfoSO[] questPrereqs;

    [Header("Steps")]
    public GameObject[] questSteps;

    [Header("Rewards")]
    public int tempReward;
    //TODO: add rewards for quests

    //sets id to the name of the file (i.e quest name)
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
