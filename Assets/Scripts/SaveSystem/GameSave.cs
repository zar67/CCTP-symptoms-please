using System;

namespace SymptomsPlease.SaveSystem
{
    [Serializable]
    public class GameSave : SaveFile
    {
        public int Index;

        public GameSave(string directory, int index, bool saveAsText) : base(directory, saveAsText)
        {
            Index = index;

            LoadData();
        }
    }
}