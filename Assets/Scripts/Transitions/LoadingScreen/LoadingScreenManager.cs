using SymptomsPlease.Events;
using SymptomsPlease.Transitions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.SceneManagement
{
    public class LoadingScreenManager : MonoBehaviour, IEventDependancy
    {
        [Header("Loading Data")]
        [SerializeField] private int m_minimumLoadingTimeInSeconds = 2;

        [Header("Display References")]
        [SerializeField] private GameObject m_loadingScreenHolder = default;
        [SerializeField] private Slider m_loadingSlider = default;

        private const string DEPENDANCY_MINIMUM_LOADING_TIME = "MINIMUM_LOADING_TIME_PASSED";

        private bool m_timerComplete = false;
        private float m_finishLoadingtime = 0;

        private void Awake()
        {
            m_loadingSlider.maxValue = 100;
        }

        private void OnEnable()
        {
            TransitionManager.OnPreloadComplete.Subscribe(StartLoadScreen);
        }

        private void OnDisable()
        {
            TransitionManager.OnPreloadComplete.UnSubscribe(StartLoadScreen);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_MINIMUM_LOADING_TIME);
        }

        private void StartLoadScreen(TransitionData data)
        {
            if (data.ShowLoadingScreen)
            {
                TransitionManager.OnLoadComplete.AddDependancy(DEPENDANCY_MINIMUM_LOADING_TIME, this);
                ShowLoadingScreen();
                StartCoroutine(UpdateSlider());
                return;
            }

            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_MINIMUM_LOADING_TIME);
        }

        private void ShowLoadingScreen()
        {
            m_loadingSlider.value = 0;
            m_loadingScreenHolder.SetActive(true);
        }

        private IEnumerator UpdateSlider()
        {
            m_timerComplete = false;
            m_finishLoadingtime = Time.time + m_minimumLoadingTimeInSeconds;

            while (!TransitionManager.OnLoadComplete.DependanciesComplete(DEPENDANCY_MINIMUM_LOADING_TIME) || !m_timerComplete)
            {
                if (!m_timerComplete && m_finishLoadingtime < Time.time)
                {
                    m_timerComplete = true;
                }

                m_loadingSlider.value = TransitionManager.OnLoadComplete.GetDependancyPercentageComplete();

                // Get Dependencies complete percentage and show average
                yield return null;
            }

            CompleteLoadingScene();
        }

        private void CompleteLoadingScene()
        {
            m_loadingScreenHolder.SetActive(false);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_MINIMUM_LOADING_TIME);
        }

        public float PercentageComplete(string identifier)
        {
            if (identifier == DEPENDANCY_MINIMUM_LOADING_TIME)
            {
                if (m_timerComplete)
                {
                    return 100;
                }

                return 100 - ((m_finishLoadingtime - Time.time) / m_minimumLoadingTimeInSeconds * 100);
            }

            return 0;
        }
    }
}