using SymptomsPlease.SaveSystem;
using System.IO;
using UnityEditor;

public static class SaveMenuItems
{
    [MenuItem("SymptomsPlease/Clear Save Data")]
    public static void ClearSaveData()
    {
        foreach (string directory in Directory.GetDirectories(SaveSystem.PROFILES_DIRECTORY))
        {
            var data = new DirectoryInfo(directory);
            data.Delete(true);
        }

        foreach (string file in Directory.GetFiles(SaveSystem.PROFILES_DIRECTORY))
        {
            var data = new FileInfo(file);
            data.Delete();
        }
    }
}
