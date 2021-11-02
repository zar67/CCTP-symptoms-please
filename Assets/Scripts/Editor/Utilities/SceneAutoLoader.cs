using SymptomsPlease.Debugging.Logging;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SymptomsPlease.SceneManagement
{
    [InitializeOnLoad]
    internal static class SceneAutoLoader
    {
        private const string EDITORPREFS_LOAD_MASTER_ON_PLAY = "SymptomsPlease.SceneAutoLoader.LoadSceneOnPlay";
        private const string EDITORPREFS_INTIAL_SCENE_PATH = "SymptomsPlease.SceneAutoLoader.InitialScene";
        private const string EDITORPREDS_PREVIOUS_SCENE_PATH = "SymptomsPlease.SceneAutoLoader.PreviousScene";

        private static bool LoadMasterOnPlay
        {
            get => EditorPrefs.GetBool(EDITORPREFS_LOAD_MASTER_ON_PLAY, false);
            set => EditorPrefs.SetBool(EDITORPREFS_LOAD_MASTER_ON_PLAY, value);
        }

        private static string StartingScene
        {
            get => EditorPrefs.GetString(EDITORPREFS_INTIAL_SCENE_PATH, "Master.unity");
            set => EditorPrefs.SetString(EDITORPREFS_INTIAL_SCENE_PATH, value);
        }

        private static string PreviousScene
        {
            get => EditorPrefs.GetString(EDITORPREDS_PREVIOUS_SCENE_PATH, EditorSceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(EDITORPREDS_PREVIOUS_SCENE_PATH, value);
        }

        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        [MenuItem("SymptomsPlease/Scene Autoload/Select Initial Scene...")]
        private static void SelectInitialScene()
        {
            string masterScene = EditorUtility.OpenFilePanel("Select Initial Scene", Application.dataPath, "unity");
            masterScene = masterScene.Replace(Application.dataPath, "Assets");

            if (!string.IsNullOrEmpty(masterScene))
            {
                StartingScene = masterScene;
                LoadMasterOnPlay = true;
            }
        }

        [MenuItem("SymptomsPlease/Scene Autoload/Load Master On Play", true)]
        private static bool ShowLoadMasterOnPlay()
        {
            return !LoadMasterOnPlay;
        }

        [MenuItem("SymptomsPlease/Scene Autoload/Load Master On Play")]
        private static void EnableLoadMasterOnPlay()
        {
            LoadMasterOnPlay = true;
        }

        [MenuItem("SymptomsPlease/Scene Autoload/Don't Load Master On Play", true)]
        private static bool ShowDontLoadMasterOnPlay()
        {
            return LoadMasterOnPlay;
        }

        [MenuItem("SymptomsPlease/Scene Autoload/Don't Load Master On Play")]
        private static void DisableLoadMasterOnPlay()
        {
            LoadMasterOnPlay = false;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (!LoadMasterOnPlay)
            {
                return;
            }

            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                PreviousScene = EditorSceneManager.GetActiveScene().path;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    try
                    {
                        EditorSceneManager.OpenScene(StartingScene);
                    }
                    catch
                    {
                        CustomLogger.Error(LoggingChannels.SceneAutoloader, $"Scene not found: {StartingScene}");
                        EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    EditorApplication.isPlaying = false;
                }
            }

            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                try
                {
                    EditorSceneManager.OpenScene(PreviousScene);
                }
                catch
                {
                    CustomLogger.Error(LoggingChannels.SceneAutoloader, $"Scene not found: {PreviousScene}");
                }
            }
        }
    }
}