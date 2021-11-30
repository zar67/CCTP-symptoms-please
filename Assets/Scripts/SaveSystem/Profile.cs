using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymptomsPlease.SaveSystem
{
    /// <summary>
    /// Save file to hold profile information about a user.
    /// </summary>
    [Serializable]
    public class Profile : SaveFile
    {
        /// <summary>
        /// Directory where profile information is stored.
        /// </summary>
        public string FolderDirectory;

        /// <summary>
        /// Gets the current save of the profile.
        /// </summary>
        public GameSave CurrentSave
        {
            get; private set;
        }

        /// <summary>
        /// List of game saves related to the profile.
        /// </summary>
        public List<GameSave> Saves = new List<GameSave>();

        /// <summary>
        /// Total time played buy the profile.
        /// </summary>
        public float TimePlayed = 0;

        private const string DATA_IDENTIFIER = "Data";

        /// <summary>
        /// Base data to be stored in the profile.
        /// </summary>
        public class ProfileData
        {
            /// <summary>
            /// Total time played.
            /// </summary>
            public float TimePlayed;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProfileData"/> class.
            /// </summary>
            public ProfileData()
            {
                TimePlayed = 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="saveSystem">Save system data.</param>
        /// <param name="folderDirectory">Directory to save data to.</param>
        /// <param name="name">Profile name.</param>
        /// <param name="saveAsText">Bool to save as text or encrypt.</param>
        public Profile(string folderDirectory, string name, bool saveAsText)
            : base(folderDirectory + "/" + name + ".sav", saveAsText)
        {
            FolderDirectory = folderDirectory;
            Saves = new List<GameSave>();
        }

        /// <summary>
        /// Sets the current save.
        /// </summary>
        /// <param name="index">Index of the save to set.</param>
        public void SetCurrentSave(int index)
        {
            CurrentSave = Saves[index];
        }

        /// <summary>
        /// Clears all profile data and deletes saves.
        /// </summary>
        public override void Clear()
        {
            Saves = new List<GameSave>();
            string[] saves = Directory.GetFiles(FolderDirectory + "/saves");
            foreach (string file in saves)
            {
                Directory.Delete(file);
            }

            base.Clear();
        }

        /// <summary>
        /// Creates a new save for the profile.
        /// </summary>
        /// <param name="saveFilesAsText">Bool to save file as text or encrypt.</param>
        public void CreateNewSave(bool saveFilesAsText)
        {
            int index = Saves.Count;
            string path = FolderDirectory + "/saves/save_" + index.ToString("00") + ".sav";

            FileStream file = File.Create(path);
            file.Close();
            File.WriteAllText(path, "");

            var newSave = new GameSave(path, Saves.Count, IsTextFile);
            Saves.Add(newSave);
        }
    }
}