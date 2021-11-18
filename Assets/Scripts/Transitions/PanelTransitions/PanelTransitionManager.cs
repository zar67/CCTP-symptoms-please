using SymptomsPlease.Events;
using SymptomsPlease.UI.Panels;
using UnityEngine;

namespace SymptomsPlease.Transitions.Panels
{
    public class PanelTransitionManager : MonoBehaviour, IEventDependancy
    {
        private const string DEPENDANCY_PANEL_OBJECT_LOADED = "PANEL_OBJECT_LOADED";

        [SerializeField] private PanelsData m_panelData = default;

        private void Awake()
        {
            m_panelData.SetupInitialPanel("");
        }

        private void OnEnable()
        {
            TransitionManager.OnPreloadBegin.Subscribe(TransitionBegin);
            TransitionManager.OnPreloadComplete.Subscribe(LoadPanel);
        }

        private void OnDisable()
        {
            TransitionManager.OnPreloadBegin.UnSubscribe(TransitionBegin);
            TransitionManager.OnPreloadComplete.UnSubscribe(LoadPanel);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_PANEL_OBJECT_LOADED);
        }

        private bool IsPanelTransition(TransitionData data)
        {
            return data is PanelTransitionData;
        }

        private void TransitionBegin(TransitionData data)
        {
            if (!IsPanelTransition(data))
            {
                return;
            }

            TransitionManager.OnLoadComplete.AddDependancy(DEPENDANCY_PANEL_OBJECT_LOADED, this);
        }

        private void LoadPanel(TransitionData data)
        {
            if (!IsPanelTransition(data))
            {
                return;
            }

            var panelData = data as PanelTransitionData;

            m_panelData.GoToPanel(panelData.PanelID, panelData.AddToPrevious);
            TransitionManager.OnLoadComplete.CompleteDependancy(DEPENDANCY_PANEL_OBJECT_LOADED);
        }

        public float PercentageComplete(string identifier)
        {
            return 1;
        }
    }
}