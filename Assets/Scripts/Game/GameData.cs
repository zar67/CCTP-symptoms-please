using Firebase.Database;
using SymptomsPlease;
using SymptomsPlease.Events;
using SymptomsPlease.SaveSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameData : MonoBehaviour, ISaveable, IEventDependancy
{
    public const string SAVE_IDENTIFIER = "GameData";
    private const string DEPENDANCY_GET_ONLINE_PERMISSION = "GET_FIREBASE_ONLINE_PERMISSION";
    private const string DEPENDANCY_GET_SCORE = "GET_FIREBASE_SCORE";
    private const string DEPENDANCY_GET_AVAILABLE_NAMES = "GET_AVAILABLE_PLAYER_NAMES";

    public struct SaveData
    {
        public string PlayerName;
        public float TotalTimePlayed;
        public int DayNumber;
        public int TotalPatientsHelped;
        public int TotalPatientsSeen;
        public int NextPatientID;
        public AvatarIndexData AvatarData;
        public PatientData[] PatientsData;
    }

    public static bool EnabledOnline
    {
        get => m_onlineEnabled;
        set
        {
            m_onlineEnabled = value;
            FirebaseDatabaseManager.UpdateOnlinePermission(m_onlineEnabled);
        }
    }

    public static string PlayerName
    {
        get => m_playerName;
        set
        {
            m_playerName = value;
            FirebaseDatabaseManager.UpdatePlayerName(m_playerName);
        }
    }

    public static int TotalScore
    {
        get => m_totalScore;
        set
        {
            m_totalScore = value;
            FirebaseDatabaseManager.UpdateScore(m_totalScore);
        }
    }

    public static List<string> AvailablePlayerNames { get; private set; } = new List<string>();

    public static Dictionary<int, PatientData> Patients { get; private set; } = new Dictionary<int, PatientData>();

    public static float TotalTimePlayed;
    public static int DayNumber;
    public static int TotalPatientsHelped;
    public static int TotalPatientsSeen;
    public static int NextPatientID;

    public static AvatarIndexData AvatarData;

    private static bool m_onlineEnabled;
    private static string m_playerName;
    private static int m_totalScore;

    private bool m_initialised = false;

    public static void FetchDataFromFirebase(string name, bool isNewUser = false)
    {
        if (isNewUser)
        {
            EnabledOnline = false;
            PlayerName = name;
            TotalScore = 0;

            GameInitialiser.OnGameLoaded.CompleteDependancy(DEPENDANCY_GET_ONLINE_PERMISSION);
            GameInitialiser.OnGameLoaded.CompleteDependancy(DEPENDANCY_GET_SCORE);
        }
        else
        {
            GetOnlinePermissionFromDatabase();
            GetScoreFromDatabase();
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
                    PlayerName = "",
                    TotalTimePlayed = 0,
                    DayNumber = 1,
                    TotalPatientsHelped = 0,
                    TotalPatientsSeen = 0,
                    NextPatientID = 0,
                    AvatarData = new AvatarIndexData(),
                    PatientsData = new PatientData[] { }
                });
            }
        }
    }

    public void LoadFromSaveFile(SaveFile file)
    {
        if (file is GameSave)
        {
            SaveData data = file.LoadObject<SaveData>(SAVE_IDENTIFIER);
            m_playerName = data.PlayerName;
            TotalTimePlayed = data.TotalTimePlayed;
            DayNumber = data.DayNumber;
            TotalPatientsHelped = data.TotalPatientsHelped;
            TotalPatientsSeen = data.TotalPatientsSeen;
            NextPatientID = data.NextPatientID;
            AvatarData = data.AvatarData;

            Patients = new Dictionary<int, PatientData>();
            foreach (var patient in data.PatientsData)
            {
                Patients.Add(patient.ID, patient);
            }
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
                NextPatientID = NextPatientID,
                AvatarData = AvatarData,
                PatientsData = Patients.Values.ToArray()
            });
        }
    }

    private static void GetOnlinePermissionFromDatabase()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child(FirebaseDatabaseManager.USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(FirebaseDatabaseManager.ONLINE_PERMISSION_REFERENCE).GetValueAsync().ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                if (task.Result.Value != null)
                {
                    m_onlineEnabled = (bool)task.Result.Value;
                    GameInitialiser.OnGameLoaded.CompleteDependancy(DEPENDANCY_GET_ONLINE_PERMISSION);
                }
                else
                {
                    Debug.LogError("Failed to get result of getting online permission from database");
                }
            }
            else
            {
                Debug.LogError("Cannot get online permission from database");
            }
        });
    }

    private static void GetScoreFromDatabase()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child(FirebaseDatabaseManager.USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(FirebaseDatabaseManager.SCORE_REFERENCE).GetValueAsync().ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                if (task.Result.Value != null)
                {
                    m_totalScore = Convert.ToInt32(task.Result.Value);
                    GameInitialiser.OnGameLoaded.CompleteDependancy(DEPENDANCY_GET_SCORE);
                }
                else
                {
                    Debug.LogError("Failed to get result of getting score from database");
                }
            }
            else
            {
                Debug.LogError("Cannot get score from database");
            }
        });
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

    private void Awake()
    {
        GameInitialiser.OnGameLoaded.AddDependancy(DEPENDANCY_GET_ONLINE_PERMISSION, this);
        GameInitialiser.OnGameLoaded.AddDependancy(DEPENDANCY_GET_SCORE, this);
        GameInitialiser.OnGameLoaded.AddDependancy(DEPENDANCY_GET_AVAILABLE_NAMES, this);

        m_initialised = false;

        FirebaseDatabase.DefaultInstance.GetReference(FirebaseDatabaseManager.VALID_NAMES_REFERENCE).ValueChanged += OnValidNamesChanged;
    }

    private void Update()
    {
        TotalTimePlayed += Time.deltaTime;
    }

    public float PercentageComplete(string identifier)
    {
        return 0;
    }

    private void OnValidNamesChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError == null)
        {
            string allNames = e.Snapshot.Value.ToString();
            string[] splitNames = allNames.Split(',');

            AvailablePlayerNames = splitNames.ToList();

            GameInitialiser.OnGameLoaded.CompleteDependancy(DEPENDANCY_GET_AVAILABLE_NAMES);

            if (!m_initialised)
            {
                m_initialised = true;
                Initialise();
            }
        }
        else
        {
            Debug.LogError("Cannot available names from database");
        }
    }

    private void Initialise()
    {
        SaveSystem.LoadSaveDataFromFiles();
        SaveSystem.Load();

        if (PlayerName == "")
        {
            if (AvailablePlayerNames.Count == 0)
            {
                Debug.Log("MAX USERS REACHED");
                return;
            }

            int randomIndex = Random.Range(0, AvailablePlayerNames.Count);
            m_playerName = AvailablePlayerNames[randomIndex];
            SaveSystem.Save(false);
            FirebaseDatabaseManager.ReservePlayerName(m_playerName);
        }

        FirebaseAuthManager.LoginUser(PlayerName);
    }
}