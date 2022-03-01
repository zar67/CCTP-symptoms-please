using SymptomsPlease.SaveSystem;
using UnityEngine;

public class FTUEManager : MonoBehaviour, ISaveable
{
    public const string SAVE_IDENTIFIER = "FTUE";

    public struct SaveData
    {
        public bool SeenTimerFTUE;
        public bool SeenSymptomsFTUE;
        public bool SeenActionsFTUE;
        public bool SeenActionsResultsFTUE;
        public bool SeenInformationFTUE;
        public bool SeenTestingFTUE;
        public bool SeenTestKitResultsFTUE;
        public bool SeenAdviceFTUE;
    }

    public static bool SeenTimerFTUE { get; set; } = false;
    public static bool SeenSymptomsFTUE { get; set; } = false;
    public static bool SeenActionsFTUE { get; set; } = false;
    public static bool SeenActionsResultsFTUE { get; set; } = false;
    public static bool SeenInformationFTUE { get; set; } = false;
    public static bool SeenTestingFTUE { get; set; } = false;
    public static bool SeenTestKitResultsFTUE { get; set; } = false;
    public static bool SeenAdviceFTUE { get; set; } = false;

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            SeenTimerFTUE = data.SeenTimerFTUE;
            SeenSymptomsFTUE = data.SeenSymptomsFTUE;
            SeenActionsFTUE = data.SeenActionsFTUE;
            SeenActionsResultsFTUE = data.SeenActionsResultsFTUE;
            SeenInformationFTUE = data.SeenInformationFTUE;
            SeenTestingFTUE = data.SeenTestingFTUE;
            SeenTestKitResultsFTUE = data.SeenTestKitResultsFTUE;
            SeenAdviceFTUE = data.SeenAdviceFTUE;
        }   
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                SeenTimerFTUE = SeenTimerFTUE,
                SeenSymptomsFTUE = SeenSymptomsFTUE,
                SeenActionsFTUE = SeenActionsFTUE,
                SeenActionsResultsFTUE = SeenActionsResultsFTUE,
                SeenInformationFTUE = SeenInformationFTUE,
                SeenTestingFTUE = SeenTestingFTUE,
                SeenTestKitResultsFTUE = SeenTestKitResultsFTUE,
                SeenAdviceFTUE = SeenAdviceFTUE
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
                    SeenTimerFTUE = false,
                    SeenSymptomsFTUE = false,
                    SeenActionsFTUE = false,
                    SeenActionsResultsFTUE = false,
                    SeenInformationFTUE = false,
                    SeenTestingFTUE = false,
                    SeenTestKitResultsFTUE = false,
                    SeenAdviceFTUE = false
                });
            }
        }
    }
}