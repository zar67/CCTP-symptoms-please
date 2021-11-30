using SymptomsPlease.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public class DayEventsManager : MonoBehaviour, IGameSaveCallback, IGameSaveCreationCallback
{
    public struct SaveData
    {
        public List<DayEvent> Events;
    }

    public const string SAVE_IDENTIFIER = "DayEvents";

    public static List<DayEvent> DayEvents = new List<DayEvent>();

    public void SaveCreation(SaveFile file)
    {
        if (!file.HasObject(SAVE_IDENTIFIER))
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData() { Events = new List<DayEvent>() });
        }
    }

    public void LoadFromSave(SaveFile file)
    {
        SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
        DayEvents = data.Events;
    }

    public void PopulateToSave(SaveFile file)
    {
        file.SaveObject(SAVE_IDENTIFIER, new SaveData() { Events = DayEvents });
    }

    public static void AddEvent(DayEventType type)
    {
        DayEvents.Add(new DayEvent() { EventType = type});
    }

    private void OnEnable()
    {
        SaveFile.OnCreated.Subscribe(SaveCreation);
        SaveFile.OnLoad.Subscribe(LoadFromSave);
        SaveFile.OnSave.Subscribe(PopulateToSave);
    }

    private void OnDisable()
    {
        SaveFile.OnCreated.UnSubscribe(SaveCreation);
        SaveFile.OnLoad.UnSubscribe(LoadFromSave);
        SaveFile.OnSave.UnSubscribe(PopulateToSave);
    }
}
