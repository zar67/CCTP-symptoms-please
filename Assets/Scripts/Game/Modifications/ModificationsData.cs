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
        public List<string> NewsDescriptions;
    }

    [SerializeField] private List<TopicData> m_topicDatas = new List<TopicData>();

    private Dictionary<Topic, TopicData> m_topicDataDictionary = new Dictionary<Topic, TopicData>();

    public string GetRandomDescription(Topic topic)
    {
        TopicData topicData = m_topicDataDictionary[topic];
        int randomIndex = Random.Range(0, topicData.NewsDescriptions.Count);
        return topicData.NewsDescriptions[randomIndex];
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        m_topicDataDictionary = new Dictionary<Topic, TopicData>();
        foreach (var topicData in m_topicDatas)
        {
            m_topicDataDictionary.Add(topicData.Topic, topicData);
        }
    }
}