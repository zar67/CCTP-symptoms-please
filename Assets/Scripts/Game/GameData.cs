using SymptomsPlease.SaveSystem;
using UnityEngine;

public class GameData : MonoBehaviour, ISaveable
{
    public const string SAVE_IDENTIFIER = "GameData";

    public struct SaveData
    {
        public string PlayerName;
        public float TotalTimePlayed;
        public int DayNumber;
        public int TotalPatientsHelped;
        public int TotalPatientsSeen;
        public AvatarIndexData AvatarData;
    }

    public static string PlayerName;
    public static float TotalTimePlayed;
    public static int DayNumber;
    public static int TotalPatientsHelped;
    public static int TotalPatientsSeen;
    public static AvatarIndexData AvatarData;

    public void SaveFileCreation(SaveFile file)
    {
        if (file is GameSave)
        {
            if (!file.HasObject(SAVE_IDENTIFIER))
            {
                file.SaveObject(SAVE_IDENTIFIER, new SaveData()
                {
                    PlayerName = "",
                    TotalTimePlayed = 0,
                    DayNumber = 1,
                    TotalPatientsHelped = 0,
                    TotalPatientsSeen = 0,
                    AvatarData = new AvatarIndexData()
                });
            }
        }
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            PlayerName = data.PlayerName;
            TotalTimePlayed = data.TotalTimePlayed;
            DayNumber = data.DayNumber;
            TotalPatientsHelped = data.TotalPatientsHelped;
            TotalPatientsSeen = data.TotalPatientsSeen;
            AvatarData = data.AvatarData;
        }
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                PlayerName = PlayerName,
                TotalTimePlayed = TotalTimePlayed,
                DayNumber = DayNumber,
                TotalPatientsHelped = TotalPatientsHelped,
                TotalPatientsSeen = TotalPatientsSeen,
                AvatarData = AvatarData
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

    private void Update()
    {
        TotalTimePlayed += Time.deltaTime;
    }
}