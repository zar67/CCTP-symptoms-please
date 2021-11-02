using SymptomsPlease.SceneManagement;
using UnityEditor;

namespace SymptomsPlease.Utilities.Processes
{
    [InitializeOnLoad]
    internal static class PlayProcess
    {
        static PlayProcess()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            // Began Playing
            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                SceneData data = BuildProcess.GetBuildSceneData();
                if (data == null)
                {
                    EditorApplication.isPlaying = false;
                    //throw new System.OperationCanceledException($"[{LoggingChannels.CustomPlayProcess}] - Build Scene Data is null");
                }
            }

            // Stopped Playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {

            }
        }
    }
}