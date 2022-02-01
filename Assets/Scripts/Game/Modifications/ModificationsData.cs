using SymptomsPlease.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SymptomsPlease/ModificationData")]
public class ModificationsData : GameScriptableObject
{
    public struct DayActivationData
    {
        public Topic Topic;
        public string Description;
    }

    [Serializable]
    public struct DayData
    {
        public int DayNumber;
        public string Description;
    }

    [Serializable]
    public struct TopicData
    {
        public Topic Topic;
        public string DefaultDescription;
        public List<DayData> ActivationDays;
        public List<DayData> DeactivationDays;
    }

    [SerializeField] private List<TopicData> m_topicDatas = new List<TopicData>();

    private Dictionary<Topic, TopicData> m_topicDataDictionary = new Dictionary<Topic, TopicData>();

    public IEnumerable<DayActivationData> GetActivationsForDay(int day)
    {
        foreach (TopicData topicData in m_topicDatas)
        {
            foreach (DayData activation in topicData.ActivationDays)
            {
                if (activation.DayNumber == day)
                {
                    yield return new DayActivationData()
                    {
                        Topic = topicData.Topic,
                        Description = activation.Description == "" ? topicData.DefaultDescription : activation.Description
                    };
                }
            }
        }
    }

    public IEnumerable<Topic> GetDeactivationsForDay(int day)
    {
        foreach (TopicData topicData in m_topicDatas)
        {
            foreach (DayData deactivation in topicData.DeactivationDays)
            {
                if (deactivation.DayNumber == day)
                {
                    yield return topicData.Topic;
                }
            }
        }
    }

    public string GetDefaultDescriptionForTopic(Topic topic)
    {
        return m_topicDataDictionary[topic].DefaultDescription;
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
            if (!m_topicDataDictionary.ContainsKey(topicData.Topic))
            {
                m_topicDataDictionary.Add(topicData.Topic, topicData);
            }
        }
    }
}