using Firebase.Auth;
using Firebase.Database;
using SymptomsPlease.SaveSystem;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FirebaseDatabaseManager : MonoBehaviour
{
    public const string USERS_REFERENCE = "users";
    public const string ONLINE_PERMISSION_REFERENCE = "online_enabled";
    public const string USERNAME_REFERENCE = "username";
    public const string SCORE_REFERENCE = "score";
    public const string SAVE_DATA_REFERENCE = "save_data";

    public const string VALID_NAMES_REFERENCE = "valid_names";

    public static void UpdateOnlinePermission(bool enable)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child(USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(ONLINE_PERMISSION_REFERENCE).SetValueAsync(enable).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update online permission in database");
            }
        });
    }

    public static void ReservePlayerName(string name)
    {
        List<string> availableNames = GameData.AvailablePlayerNames;
        availableNames.Remove(name);

        string allNames = availableNames[0];
        for (int i = 1; i < availableNames.Count; i++)
        {
            allNames += "," + availableNames[i];
        }

        FirebaseDatabase.DefaultInstance.RootReference.Child(VALID_NAMES_REFERENCE).SetValueAsync(allNames).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update available names in database.");
            }
        });
    }

    public static void UnresevePlayerName(string name)
    {
        List<string> availableNames = GameData.AvailablePlayerNames;
        availableNames.Add(name);

        string allNames = availableNames[0];
        for (int i = 1; i < availableNames.Count; i++)
        {
            allNames += "," + availableNames[i];
        }

        FirebaseDatabase.DefaultInstance.RootReference.Child(VALID_NAMES_REFERENCE).SetValueAsync(allNames).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update available names in database.");
            }
        });
    }

    public static void UpdatePlayerName(string name)
    {
        var userProfile = new UserProfile()
        {
            DisplayName = name
        };

        FirebaseAuthManager.CurrentUser.UpdateEmailAsync(FirebaseAuthManager.ConvertNameToEmail(name)).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update user email");
            }
        });

        FirebaseAuthManager.CurrentUser.UpdateUserProfileAsync(userProfile).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update user profile");
            }
        });

        FirebaseDatabase.DefaultInstance.RootReference.Child(USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(USERNAME_REFERENCE).SetValueAsync(name).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update username in database");
            }
        });
    }

    public static void UpdateScore(int score)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child(USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(SCORE_REFERENCE).SetValueAsync(score).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update score in database");
            }
        });
    }

    public static void UploadSaveData()
    {
        string currentProfile = SaveSystem.CurrentProfile.CurrentSave.FilePath;
        string allData = File.ReadAllText(currentProfile);

        FirebaseDatabase.DefaultInstance.RootReference.Child(USERS_REFERENCE).Child(FirebaseAuthManager.CurrentUser.UserId).Child(SAVE_DATA_REFERENCE).SetValueAsync(allData).ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to update score in database");
            }
        });
    }
}