using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "SymptomsPlease/ModificationData")]
public class ModificationsData : GameScriptableObject
{
    [Serializable]
    public struct TopicData
    {
        public Topic Topic;
        public List<string> ActivateDescriptions;
        public List<string> DeactivateDescriptions;
    }

    [SerializeField] private List<TopicData> m_topicDatas = new List<TopicData>();

    private Dictionary<Topic, TopicData> m_topicDataDictionary = new Dictionary<Topic, TopicData>();

    public string GetRandomActivateDescription(Topic topic)
    {
        TopicData topicData = m_topicDataDictionary[topic];
        int randomIndex = Random.Range(0, topicData.ActivateDescriptions.Count);
        return topicData.ActivateDescriptions[randomIndex];
    }

    public string GetRandomDeactivateDescription(Topic topic)
    {
        TopicData topicData = m_topicDataDictionary[topic];
        int randomIndex = Random.Range(0, topicData.DeactivateDescriptions.Count);
        return topicData.DeactivateDescriptions[randomIndex];
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_topicDataDictionary = new Dictionary<Topic, TopicData>();
        foreach (TopicData topicData in m_topicDatas)
        {
            m_topicDataDictionary.Add(topicData.Topic, topicData);
        }
    }
}