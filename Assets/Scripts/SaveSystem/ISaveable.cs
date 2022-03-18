namespace SymptomsPlease.SaveSystem
{
    public interface ISaveable
    {
        void SaveFileCreation(SaveFile file);
        void PopulateToSaveFile(SaveFile file);
        void LoadFromSaveFile(SaveFile file);
    }
}