using SymptomsPlease.Events;
using SymptomsPlease.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace SymptomsPlease.Transitions.Scenes
{
    public class InitialSceneTransitionComponent : MonoBehaviour, IEventDependancy
    {
        private const string DEPENDANCY_GAME_LOADED = "GAME_LOADED";

        [SerializeField] private SceneData m_sceneData = default;
        [SerializeField] private SceneTransitionData m_startingSceneTransition = default;

        private void Awake()
        {
            m_startingSceneTransition.State = TransitionData.TransitionState.OUT;
        }

        private void Start()
        {
            TransitionManager.OnLoadComplete.AddDependancy(DEPENDANCY_GAME_LOADED, this);
            StartCoroutine(InitialGameLoad());
        }

        private IEnumerator InitialGameLoad()
        {
            yield return new WaitUntil(() => GameInitialiser.OnGameLoaded.DependanciesComplete() == true);
            GameInitialiser.OnGameLoaded.Invoke();
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_GAME_LOADED);
        }

        private void OnEnable()
        {
            GameInitialiser.OnGameLoaded.Subscribe(GameLoaded);
        }

        private void OnDisable()
        {
            GameInitialiser.OnGameLoaded.UnSubscribe(GameLoaded);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_GAME_LOADED);
        }

        private void GameLoaded()
        {
            m_sceneData.TransitionToScene(m_startingSceneTransition);
        }

        public float PercentageComplete(string identifier)
        {
            return GameInitialiser.OnGameLoaded.GetDependancyPercentageComplete();
        }
    }
}