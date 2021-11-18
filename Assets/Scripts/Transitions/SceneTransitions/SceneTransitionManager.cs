using SymptomsPlease.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SymptomsPlease.Transitions.Scenes
{
    public class SceneTransitionManager : MonoBehaviour, IEventDependancy
    {
        private const string DEPENDANCY_SCENE_ASSET_LOADED = "SCENE_ASSET_LOADED";

        private AsyncOperation m_loadOperation = null;

        private void OnEnable()
        {
            TransitionManager.OnPreloadBegin.Subscribe(TransitionBegin);
            TransitionManager.OnPreloadComplete.Subscribe(LoadScene);
        }

        private void OnDisable()
        {
            TransitionManager.OnPreloadBegin.UnSubscribe(TransitionBegin);
            TransitionManager.OnPreloadComplete.UnSubscribe(LoadScene);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_SCENE_ASSET_LOADED);
        }

        private bool IsSceneTransition(TransitionData data)
        {
            return data is SceneTransitionData;
        }

        private void TransitionBegin(TransitionData data)
        {
            if (!IsSceneTransition(data))
            {
                return;
            }

            TransitionManager.OnLoadComplete.AddDependancy(DEPENDANCY_SCENE_ASSET_LOADED, this);
        }

        private void LoadScene(TransitionData data)
        {
            if (data is SceneTransitionData sceneData)
            {
                StartCoroutine(LoadSceneAsset(sceneData));
            }
        }

        private IEnumerator LoadSceneAsset(SceneTransitionData data)
        {
            m_loadOperation = SceneManager.LoadSceneAsync(data.SceneInfo.BuildIndex);

            while (!m_loadOperation.isDone)
            {
                yield return null;
            }

            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_SCENE_ASSET_LOADED);
        }

        public float PercentageComplete(string identifier)
        {
            if (identifier == DEPENDANCY_SCENE_ASSET_LOADED && m_loadOperation != null)
            {
                if (m_loadOperation.isDone)
                {
                    return 100;
                }

                return m_loadOperation.progress * 100;
            }

            return 0;
        }
    }
}