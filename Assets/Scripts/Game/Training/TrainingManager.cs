using SymptomsPlease.SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingManager : MonoBehaviour, ISaveable
{
    [Serializable]
    public struct ModificationInstanceData
    {
        public Topic Topic;
        public string Description;
    }

    public const string SAVE_IDENTIFIER = "Training";

    public struct SaveData
    {
        public Dictionary<Topic, int> NumCorrectQuestions;
        public Dictionary<Topic, int> TotalQuestions;
    }

    private static Dictionary<Topic, int> m_numCorrectQuestions = new Dictionary<Topic, int>();
    private static Dictionary<Topic, int> m_totalQuestions = new Dictionary<Topic, int>();

    public static List<KeyValuePair<Topic, int>> GetRankedTopics()
    {
        return m_numCorrectQuestions.OrderBy(x => x.Value).Reverse().ToList();
    }

    public static Topic GetBestTopic()
    {
        Topic bestTopic = Topic.TEST1;

        foreach (KeyValuePair<Topic, int> data in m_numCorrectQuestions)
        {
            if (m_numCorrectQuestions[bestTopic] < data.Value)
            {
                bestTopic = data.Key;
            }
        }

        return bestTopic;
    }

    public static Topic GetWorstTopic()
    {
        Topic bestTopic = Topic.TEST1;

        foreach (KeyValuePair<Topic, int> data in m_numCorrectQuestions)
        {
            if (m_numCorrectQuestions[bestTopic] > data.Value)
            {
                bestTopic = data.Key;
            }
        }

        return bestTopic;
    }

    public static void RegisterQuestion(Topic topic, bool answerCorrect)
    {
        m_totalQuestions[topic]++;

        if (answerCorrect)
        {
            m_numCorrectQuestions[topic]++;
        }
    }

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                var zeroedTopics = new Dictionary<Topic, int>();
                foreach (Topic topic in Enum.GetValues(typeof(Topic)))
                {
                    zeroedTopics.Add(topic, 0);
                }

                file.SaveObject(SAVE_IDENTIFIER, new SaveData()
                {
                    NumCorrectQuestions = zeroedTopics,
                    TotalQuestions = zeroedTopics
                });
            }
        }
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            m_numCorrectQuestions = data.NumCorrectQuestions;
            m_totalQuestions = data.TotalQuestions;
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                NumCorrectQuestions = m_numCorrectQuestions,
                TotalQuestions = m_totalQuestions
            });
        }
    }

    private void OnEnable()
    {
        SaveFile.OnCreated.Subscribe(SaveFileCreation);
        SaveFile.OnLoad.Subscribe(LoadFromSaveFile);
        SaveFile.OnSave.Subscribe(PopulateToSaveFile);
    }

    private void OnDisable()
    {
        SaveFile.OnCreated.UnSubscribe(SaveFileCreation);
        SaveFile.OnLoad.UnSubscribe(LoadFromSaveFile);
        SaveFile.OnSave.UnSubscribe(PopulateToSaveFile);
    }
}