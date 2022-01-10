using UnityEngine;

namespace SymptomsPlease.Debugger
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] private GameObject m_debuggerButton = default;
        [SerializeField] private GameObject m_expandedViewGameObject = default;

        [SerializeField] private OptionsPanel m_optionsPanel = default;
        [SerializeField] private ConsolePanel m_consolePanel = default;

        private static Debugger m_instance;

        public static OptionsPanel OptionsPanel => m_instance.m_optionsPanel;

        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }
            else
            {
                Debug.LogError("More than one instance of RowbotDebugger...");
                return;
            }

            m_consolePanel.Initialise();
        }

        private void Start()
        {
            m_debuggerButton.SetActive(true);
            m_expandedViewGameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            m_instance = null;
        }
    }
}