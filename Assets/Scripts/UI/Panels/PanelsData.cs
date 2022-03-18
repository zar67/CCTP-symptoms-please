using SymptomsPlease.Debugging.Logging;
using SymptomsPlease.ScriptableObjects;
using SymptomsPlease.Transitions;
using SymptomsPlease.Utilities.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    [CreateAssetMenu(menuName = "SymptomsPlease/UI/Panels/Panels Data")]
    public class PanelsData : GameScriptableObject
    {
        [Header("Panels Data")]
        [SerializeField] private PanelsSet m_allPanels = default;
        [SerializeField] private PanelFlowConfig m_panelsFlowConfig = default;

        public string CurrentPanel => m_currentPanel;
        [ReadOnly, SerializeField] private string m_currentPanel;

        public string PanelToStart => m_panelToStart;
        [ReadOnly, SerializeField] private string m_panelToStart;
        [ReadOnly, SerializeField] private List<string> m_previousPanels = new List<string>();

        public void Reset()
        {
            m_currentPanel = string.Empty;
            m_previousPanels = new List<string>();
        }

        public void TransitionToPanel(PanelTransitionData data)
        {
            TransitionManager.OnTransitionBegin.Invoke(data);
        }

        public bool GoToPanel(string panel, bool AddToPrevious = true)
        {
            if (!string.IsNullOrWhiteSpace(m_panelToStart))
            {
                m_previousPanels = m_panelsFlowConfig.Flow.FindPath(m_panelsFlowConfig.StartingPanel, m_panelToStart);
                m_panelToStart = string.Empty;

                if (m_previousPanels.Count > 0)
                {
                    m_previousPanels.RemoveAt(m_previousPanels.Count - 1);
                }
                else
                {
                    CustomLogger.Warning(LoggingChannels.PanelManager, "Path from FlowConfig Graph is empty");
                }
            }

            if (string.IsNullOrWhiteSpace(panel))
            {
                m_currentPanel = string.Empty;
                return true;
            }

            if (CurrentPanel != string.Empty && !m_panelsFlowConfig.Flow.HasTransition(CurrentPanel, panel))
            {
                CustomLogger.Error(LoggingChannels.PanelManager, $"{CurrentPanel} does not have a transition to {panel}");
                return false;
            }

            Panel currentPanel = m_allPanels.GetPanelsObject(CurrentPanel);
            Panel nextPanel = m_allPanels.GetPanelsObject(panel);

            if (currentPanel == null && !string.IsNullOrWhiteSpace(CurrentPanel))
            {
                CustomLogger.Error(LoggingChannels.PanelManager, $"Could not find panel {CurrentPanel}");
                return false;
            }
            if (nextPanel == null)
            {
                CustomLogger.Error(LoggingChannels.PanelManager, $"Could not find panel {panel}");
                return false;
            }

            if (nextPanel.DisablePreviousPanel && !string.IsNullOrWhiteSpace(CurrentPanel))
            {
                currentPanel.OnClose();
                currentPanel.gameObject.SetActive(false);
            }

            if (AddToPrevious && !string.IsNullOrWhiteSpace(CurrentPanel))
            {
                m_previousPanels.Add(CurrentPanel);
            }

            m_currentPanel = panel;
            nextPanel.OnOpen();
            nextPanel.gameObject.SetActive(true);

            return true;
        }

        public void GoBack(bool showLoadingScreen, string effectType)
        {
            string nextPanel;
            if (m_previousPanels.Count == 0)
            {
                nextPanel = m_panelsFlowConfig.StartingPanel;
                return;
            }
            else if (m_previousPanels.Contains(CurrentPanel))
            {
                int currentPanelIndex = m_previousPanels.IndexOf(CurrentPanel);

                if (currentPanelIndex == 0)
                {
                    nextPanel = m_panelsFlowConfig.StartingPanel;
                    return;
                }

                nextPanel = m_previousPanels[currentPanelIndex - 1];
                return;
            }
            else
            {
                nextPanel = m_previousPanels[m_previousPanels.Count - 1];
            }

            TransitionToPanel(new PanelTransitionData()
            {
                PanelID = nextPanel,
                AddToPrevious = false,
                ForceTransition = true,
                ShowLoadingScreen = showLoadingScreen,
                State = TransitionData.TransitionState.OUT,
                TransitionType = effectType
            });

            int nextPanelIndex = m_previousPanels.IndexOf(nextPanel);
            m_previousPanels.RemoveRange(nextPanelIndex, m_previousPanels.Count - nextPanelIndex);
        }

        public bool GoToStartingPanel()
        {
            return GoToPanel(m_panelsFlowConfig.StartingPanel);
        }

        public void SetupInitialPanel(string panel)
        {
            m_panelToStart = panel;
        }
    }
}