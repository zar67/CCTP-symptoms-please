using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymptomsPlease.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        public static bool SaveFilesAsText = true;
        public static int MaxNumberOfSaves = 1;

        public static List<Profile> Profiles = new List<Profile>();

        public static Profile CurrentProfile
        {
            get; private set;
        }

        public static void SetCurrentProfile(int index)
        {
            CurrentProfile = Profiles[index];
        }

        public static void Save()
        {
            CurrentProfile.Save();
            CurrentProfile.CurrentSave.Save();
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
            string folderDirectory = Application.persistentDataPath + "/profiles";
            if (!Directory.Exists(folderDirectory))
            {
                Profiles = new List<Profile>();
                Directory.CreateDirectory(folderDirectory);
            }

            Directory.CreateDirectory(folderDirectory + "/" + name);
            Directory.CreateDirectory(folderDirectory + "/" + name + "/saves");
            string folderPath = folderDirectory + "/" + name;
            string filePath = folderPath + "/" + name + ".sav";

            FileStream file = File.Create(filePath);
            file.Close();
            File.WriteAllText(filePath, "");

            var newProfile = new Profile(folderPath, name, SaveFilesAsText);
            Profiles.Add(newProfile);
        }

        public static void CreateNewSave()
        {
            CurrentProfile.CreateNewSave(SaveFilesAsText);
        }

        public static void LoadSaveDataFromFiles()
        {
            string directoryPath = Application.persistentDataPath + "/profiles";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                CreateNewProfile("DefaultProfile");
            }

            Profiles = new List<Profile>();
            string[] profiles = Directory.GetDirectories(directoryPath);
            foreach (string profileDir in profiles)
            {
                string name = profileDir.Split('\\')[1];
                string[] saves = Directory.GetFiles(profileDir + "/saves");

                var profile = new Profile(profileDir, name, SaveFilesAsText);
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

        private void Awake()
        {
            SaveSystem.LoadSaveDataFromFiles();
            SaveSystem.Load();
        }
    }
}