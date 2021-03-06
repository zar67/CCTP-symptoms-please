using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymptomsPlease.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        public static string PROFILES_DIRECTORY => Application.persistentDataPath + "/profiles";

        public static bool SaveFilesAsText = true;
        public static int MaxNumberOfSaves = 1;

        public static List<GameProfile> Profiles = new List<GameProfile>();

        public static GameProfile CurrentProfile
        {
            get; private set;
        }

        public static void SetCurrentProfile(int index)
        {
            CurrentProfile = Profiles[index];
        }

        public static void Save(bool upload = true)
        {
            CurrentProfile.Save();
            CurrentProfile.CurrentSave.Save();

            if (upload)
            {
                FirebaseDatabaseManager.UploadSaveData();
            }
        }

        public static void Load()
        {
            CurrentProfile.Load();
            CurrentProfile.CurrentSave.Load();
        }

        public static void DeleteData()
        {
            foreach (string directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                var data = new DirectoryInfo(directory);
                data.Delete(true);
            }

            foreach (string file in Directory.GetFiles(Application.persistentDataPath))
            {
                var data = new FileInfo(file);
                data.Delete();
            }
        }

        public static void CreateNewProfile(string name)
        {
            if (!Directory.Exists(PROFILES_DIRECTORY))
            {
                Profiles = new List<GameProfile>();
                Directory.CreateDirectory(PROFILES_DIRECTORY);
            }

            Directory.CreateDirectory(PROFILES_DIRECTORY + "/" + name);
            Directory.CreateDirectory(PROFILES_DIRECTORY + "/" + name + "/saves");
            string folderPath = PROFILES_DIRECTORY + "/" + name;
            string filePath = folderPath + "/" + name + ".sav";

            FileStream file = File.Create(filePath);
            file.Close();
            File.WriteAllText(filePath, "");

            var newProfile = new GameProfile(folderPath, name, SaveFilesAsText);
            Profiles.Add(newProfile);
        }

        public static void CreateNewSave()
        {
            CurrentProfile.CreateNewSave(SaveFilesAsText);
        }

        public static void LoadSaveDataFromFiles()
        {
            if (!Directory.Exists(PROFILES_DIRECTORY))
            {
                Directory.CreateDirectory(PROFILES_DIRECTORY);
                CreateNewProfile("DefaultProfile");
            }

            Profiles = new List<GameProfile>();
            foreach (string profileDir in Directory.EnumerateDirectories(PROFILES_DIRECTORY))
            {
                string name = Path.GetFileName(profileDir);
                string[] saves = Directory.GetFiles(profileDir + "/saves");

                var profile = new GameProfile(profileDir, name, SaveFilesAsText);
                foreach (string saveDir in saves)
                {
                    var saveGame = new GameSave(saveDir, profile.Saves.Count, profile.IsTextFile);
                    profile.Saves.Add(saveGame);
                }

                if (saves.Length == 0)
                {
                    profile.CreateNewSave(SaveFilesAsText);
                }

                profile.SetCurrentSave(0);
                Profiles.Add(profile);
            }

            if (Profiles.Count == 0)
            {
                CreateNewProfile("DefaultProfile");
            }

            CurrentProfile = Profiles[0];

            if (CurrentProfile.Saves.Count == 0)
            {
                CurrentProfile.CreateNewSave(SaveFilesAsText);
                CurrentProfile.SetCurrentSave(0);
            }
        }
    }
}