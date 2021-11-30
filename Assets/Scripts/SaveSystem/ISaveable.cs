namespace SymptomsPlease.SaveSystem
{
    public interface ISaveable : IGameSaveCreationCallback, IGameSaveCallback, IProfileSaveCreationCallback, IProfileSaveCallback
    {

    }

    public interface IGameSaveCreationCallback
    {
        void SaveCreation(SaveFile file);
    }

    public interface IGameSaveCallback
    {
        void LoadFromSave(SaveFile file);

        void PopulateToSave(SaveFile file);
    }

    public interface IProfileSaveCreationCallback
    {
        void ProfileCreation(SaveFile file);
    }

    public interface IProfileSaveCallback
    {
        void LoadFromProfile(SaveFile file);

        void PopulateToProfile(SaveFile file);
    }
}


