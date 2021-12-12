using SymptomsPlease.SaveSystem;
using System;
using UnityEngine;

public class GameData : MonoBehaviour, IGameSaveCallback, IGameSaveCreationCallback
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

    [Serializable]
    public struct AvatarIndexData
    {
        public int SkinColourIndex;
        public int HairTypeIndex;
        public int HairColourIndex;
        public int EyeTypeIndex;
        public int EyeColourIndex;
        public int EyebrowTypeIndex;
        public int EyebrowColourIndex;
        public int NoseTypeIndex;
        public int MouthTypeIndex;
        public int ShirtTypeIndex;
        public int ShirtSleeveTypeIndex;
        public int ShirtColourIndex;
        public int PantsTypeIndex;
        public int PantLegTypeIndex;
        public int PantsColourIndex;
        public int ShoesTypeIndex;
        public int ShoesColourIndex;
    }

    public static string PlayerName;
    public static float TotalTimePlayed;
    public static int DayNumber;
    public static int TotalPatientsHelped;
    public static int TotalPatientsSeen;
    public static AvatarIndexData AvatarData;

    public void SaveCreation(SaveFile file)
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

    public void LoadFromSave(SaveFile file)
    {
        SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
        PlayerName = data.PlayerName;
        TotalTimePlayed = data.TotalTimePlayed;
        DayNumber = data.DayNumber;
        TotalPatientsHelped = data.TotalPatientsHelped;
        TotalPatientsSeen = data.TotalPatientsSeen;
        AvatarData = data.AvatarData;
    }

    public void PopulateToSave(SaveFile file)
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

    private void Update()
    {
        TotalTimePlayed += Time.deltaTime;
    }
}