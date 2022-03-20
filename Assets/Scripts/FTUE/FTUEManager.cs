using SymptomsPlease.SaveSystem;
using System.Collections.Generic;
using UnityEngine;

public enum EFTUEType
{
    DISCLAIMER_POPUP,
    SEEN_TIMER_FTUE,
    SEEN_SYMPTOMS_FTUE,
    SEEN_ACTIONS_FTUE,
    SEEN_ACTIONS_RESULTS_FTUE,
    SEEN_INFORMATION_FTUE,
    SEEN_TESTING_FTUE,
    SEEN_TEST_KIT_RESULTS_FTUE,
    SEEN_ADVICE_FTUE,
    SEEN_ADVICE_SLIP_FTUE
}

public class FTUEManager : MonoBehaviour, ISaveable
{
    public const string SAVE_IDENTIFIER = "FTUE";

    public struct SaveData
    {
        public HashSet<EFTUEType> HandledFTUEs;
    }

    private static HashSet<EFTUEType> m_handledFTUEs = new HashSet<EFTUEType>();

    public static void HandleFTUEType(EFTUEType type)
    {
        m_handledFTUEs.Add(type);
    }

    public static bool IsFTUETypeHandled(EFTUEType type)
    {
        return m_handledFTUEs.Contains(type);
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            m_handledFTUEs = data.HandledFTUEs;
        }   
    }

    public void PopulateToSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            file.SaveObject(SAVE_IDENTIFIER, new SaveData()
            {
                HandledFTUEs = m_handledFTUEs
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
                    HandledFTUEs = new HashSet<EFTUEType>()
                });
            }
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