using TMPro;
using UnityEngine;

namespace SymptomsPlease.Debugger
{
    public class ConsoleTabButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_errorText = default;
        [SerializeField] private TextMeshProUGUI m_warningText = default;
        [SerializeField] private TextMeshProUGUI m_messageText = default;

        private int m_errorCount = 0;
        private int m_warningCount = 0;
        private int m_messageCount = 0;

        private void Awake()
        {
            Application.logMessageReceived += HandleLog;

            m_messageText.text = m_messageCount.ToString();
            m_warningText.text = m_warningCount.ToString();
            m_errorText.text = m_errorCount.ToString();
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                {
                    m_messageCount++;
                    m_messageText.text = m_messageCount.ToString();
                    break;
                }
                case LogType.Warning:
                {
                    m_warningCount++;
                    m_warningText.text = m_warningCount.ToString();
                    break;
                }
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                {
                    m_errorCount++;
                    m_errorText.text = m_errorCount.ToString();
                    break;
                }
            }
        }
    }
}