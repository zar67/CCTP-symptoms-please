using SymptomsPlease.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModificationsManager : MonoBehaviour, ISaveable
{
    [Serializable]
    public struct ModificationInstanceData
    {
        public Topic Topic;
        public string Description;
    }

    public const string SAVE_IDENTIFIER = "Modifications";

    public struct SaveData
    {
        public Dictionary<Topic, ModificationInstanceData> ActiveTopics;
        public Dictionary<Topic, ModificationInstanceData> UnhandledTopics; // New Topics that haven't been added to a day yet.
    }

    public static IEnumerable<KeyValuePair<Topic, ModificationInstanceData>> ActiveTopics => m_activeTopics;

    public static IEnumerable<KeyValuePair<Topic, ModificationInstanceData>> UnhandledTopics => m_unhandledTopics;

    public static int NumActiveTopics => m_activeTopics.Count;

    public static int NumNewTopics => m_unhandledTopics.Count;

    private static Dictionary<Topic, ModificationInstanceData> m_activeTopics = new Dictionary<Topic, ModificationInstanceData>();
    private static Dictionary<Topic, ModificationInstanceData> m_unhandledTopics = new Dictionary<Topic, ModificationInstanceData>();

    public static void ActivateTopic(Topic topic, string description)
    {
        var data = new ModificationInstanceData()
        {
            Topic = topic,
            Description = description
        };

        if (!m_activeTopics.ContainsKey(topic))
        {
            m_activeTopics.Add(topic, data);
            m_unhandledTopics.Add(topic, data);
        }
    }

    public static void DeactivateTopic(Topic topic)
    {
        if (m_activeTopics.ContainsKey(topic))
        {
            m_activeTopics.Remove(topic);
            m_activeTopics.Remove(topic);
        }
    }

    public static bool IsTopicActive(Topic topic)
    {
        return m_activeTopics.ContainsKey(topic);
    }

    public static void ClearNewTopics()
    {
        m_unhandledTopics = new Dictionary<Topic, ModificationInstanceData>();
    }

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                file.SaveObject(SAVE_IDENTIFIER, new SaveData()
                {
                    ActiveTopics = new Dictionary<Topic, ModificationInstanceData> { },
                    UnhandledTopics = new Dictionary<Topic, ModificationInstanceData> { }
                });
            }
        }
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            m_activeTopics = data.ActiveTopics;
            m_unhandledTopics = data.UnhandledTopics;
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                ActiveTopics = m_activeTopics,
                UnhandledTopics = m_unhandledTopics
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