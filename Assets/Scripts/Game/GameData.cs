using SymptomsPlease.SaveSystem;
using UnityEngine;

public class GameData : MonoBehaviour, IGameSaveCallback, IGameSaveCreationCallback
{
    public const string SAVE_IDENTIFIER = "GameData";

    public struct SaveData
    {
        public int DayNumber;
    }

    public static int DayNumber;

    public void SaveCreation(SaveFile file)
    {
        if (!file.HasObject(SAVE_IDENTIFIER))
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData() { DayNumber = 1 });
        }
    }

    public void LoadFromSave(SaveFile file)
    {
        SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
        GameData.DayNumber = data.DayNumber;
    }

    public void PopulateToSave(SaveFile file)
    {
        file.SaveObject(SAVE_IDENTIFIER, new SaveData() { DayNumber = DayNumber });
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