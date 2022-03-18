using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private PanelsSet m_panelsSet = default;
        [SerializeField] private PanelsData m_panelManager = default;
        [SerializeField] private PanelFlowConfig m_panelFlowConfig = default;

        private void Awake()
        {
            m_panelManager.Reset();
            m_panelsSet.Reset();

            Panel[] panels = GetComponentsInChildren<Panel>(true);
            foreach (Panel panel in panels)
            {
                panel.gameObject.SetActive(true);
                m_panelsSet.Add(panel);
            }
        }

        private void Start()
        {
            m_panelFlowConfig.Flow.Initialise();

            foreach (Panel panel in m_panelsSet.Items)
            {
                panel.gameObject.SetActive(false);
            }

            if (string.IsNullOrEmpty(m_panelManager.PanelToStart))
            {
                m_panelManager.GoToStartingPanel();
            }
            else
            {
                m_panelManager.GoToPanel(m_panelManager.PanelToStart);
            }
        }

        private void OnDestroy()
        {
            m_panelManager.GoToPanel(string.Empty);
            m_panelsSet.Reset();
        }
    }
}