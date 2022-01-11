using SymptomsPlease.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public class DayEventsManager : MonoBehaviour, ISaveable
{
    public struct SaveData
    {
        public List<DayEvent> Events;
    }

    public const string SAVE_IDENTIFIER = "DayEvents";

    public static List<DayEvent> DayEvents = new List<DayEvent>();

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                file.SaveObject(SAVE_IDENTIFIER, new SaveData() { Events = new List<DayEvent>() });
            }
        }
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            DayEvents = data.Events;
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData() { Events = DayEvents });
        }
    }

    public static void AddEvent(DayEventType type)
    {
        DayEvents.Add(new DayEvent() { EventType = type });
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
