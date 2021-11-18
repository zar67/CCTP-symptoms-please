using System.Collections.Generic;
using UnityEngine;
using SymptomsPlease.ScriptableObjects;
using SymptomsPlease.Transitions;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SymptomsPlease.SceneManagement
{
    /// <summary>
    /// Data to represent the scenes in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildSceneData", menuName = "SymptomsPlease/SceneData/BuildSceneData")]
    public class SceneData : GameScriptableObject
    {
        /// <summary>
        /// Scenes stored in this data.
        /// </summary>
        [HideInInspector] public List<SceneInfo> Scenes = new List<SceneInfo>();

        /// <summary>
        /// Gets the active scene.
        /// </summary>
        public SceneInfo CurrentScene => GetSceneInfo(SceneManager.GetActiveScene().buildIndex);

        /// <summary>
        /// Begins a transition to a different scene.
        /// </summary>
        /// <param name="type">Scene to transition to.</param>
        /// <param name="transitionType">Transition effect type.</param>
        /// <param name="forceTransition">Bool to force transition.</param>
        public void TransitionToScene(string type, string transitionType = "", bool forceTransition = false)
        {
            SceneInfo sceneInfo = GetSceneInfo(type);
            var data = new SceneTransitionData()
            {
                SceneInfo = sceneInfo,
                TransitionType = transitionType,
                State = TransitionData.TransitionState.OUT,
                ForceTransition = true,
            };
            TransitionToScene(data);
        }

        /// <summary>
        /// Begins a transition to another scene.
        /// </summary>
        /// <param name="data">Data for the transition.</param>
        public void TransitionToScene(SceneTransitionData data)
        {
            TransitionManager.OnTransitionBegin.Invoke(data);
        }

        /// <summary>
        /// Gets the scene info for a scene type.
        /// </summary>
        /// <param name="type">Scene type to find the info for.</param>
        /// <returns>Scene info of the given type.</returns>
        public SceneInfo GetSceneInfo(string type)
        {
            foreach (SceneInfo scene in Scenes)
            {
                if (scene.Type == type)
                {
                    return scene;
                }
            }

            return default;
        }

        /// <summary>
        /// Gets the scene info for a build index.
        /// </summary>
        /// <param name="buildIndex">Build index to find the scene info of.</param>
        /// <returns>Scene info with the given build index.</returns>
        public SceneInfo GetSceneInfo(int buildIndex)
        {
            foreach (SceneInfo scene in Scenes)
            {
                if (scene.BuildIndex == buildIndex)
                {
                    return scene;
                }
            }

            return default;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Gets the scenes currently included in the data.
        /// </summary>
        /// <returns>List of included scenes.</returns>
        public List<string> GetIncludedScenePaths()
        {
            var scenes = new List<string>();

            foreach (SceneInfo scene in Scenes)
            {
                scenes.Add(AssetDatabase.GetAssetPath(scene.SceneAsset));
            }

            return scenes;
        }

        /// <summary>
        /// Updates the build index and the build settings to the current scenes.
        /// </summary>
        public void BuildSceneDataListToSettings()
        {
            var includedPaths = new List<string>();
            var editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

            for (int i = 0; i < Scenes.Count; i++)
            {
                SceneInfo sceneInfo = Scenes[i];
                string path = AssetDatabase.GetAssetPath(sceneInfo.SceneAsset);
                if (!string.IsNullOrEmpty(path) && !includedPaths.Contains(path))
                {
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
                    includedPaths.Add(path);
                    sceneInfo.BuildIndex = i;
                    EditorUtility.SetDirty(sceneInfo);
                }
            }

            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
#endif
    }
}