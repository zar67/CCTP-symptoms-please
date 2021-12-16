using SymptomsPlease.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public class ModificationsManager : MonoBehaviour, ISaveable
{
    public const string SAVE_IDENTIFIER = "Modifications";

    public struct SaveData
    {
        public List<Topic> ActiveTopics;
    }

    public static IEnumerable<Topic> ActiveTopics => m_activeTopics;

    private static List<Topic> m_activeTopics = new List<Topic>();

    public static void ActivateTopic(Topic topic)
    {
        if (!m_activeTopics.Contains(topic))
        {
            m_activeTopics.Add(topic);
        }
    }

    public static void DeactivateTopic(Topic topic)
    {
        if (m_activeTopics.Contains(topic))
        {
            m_activeTopics.Remove(topic);
        }
    }

    public static bool IsTopicActive(Topic topic)
    {
        return m_activeTopics.Contains(topic);
    }

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                file.SaveObject(SAVE_IDENTIFIER, new SaveData()
                {
                    ActiveTopics = new List<Topic>() { Topic.TEST1 }
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
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                ActiveTopics = m_activeTopics
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