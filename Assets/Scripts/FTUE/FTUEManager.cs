using SymptomsPlease.SaveSystem;
using UnityEngine;

public class FTUEManager : MonoBehaviour, ISaveable
{
    public const string SAVE_IDENTIFIER = "FTUE";

    public struct SaveData
    {
        public bool SeenFTUE;
    }

    public static bool SeenFTUE { get; private set; } = false;

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            SeenFTUE = data.SeenFTUE;
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                SeenFTUE = SeenFTUE
            });
        }
    }

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                file.SaveObject(SAVE_IDENTIFIER, new SaveData()
                {
                    SeenFTUE = false
                });
            }
        }
    }

    public static void CompleteFTUE()
    {
        SeenFTUE = true;
    }
}