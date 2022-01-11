using SymptomsPlease.Utilities.ExtensionMethods;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.Debugger
{
    public class ConsolePanel : MonoBehaviour
    {
        [SerializeField] private Sprite m_errorLogSprite = default;
        [SerializeField] private Sprite m_warningLogSprite = default;
        [SerializeField] private Sprite m_messageLogSprite = default;

        [SerializeField] private Transform m_scrollViewContent = default;
        [SerializeField] private TextMeshProUGUI m_stackTraceText = default;
        [SerializeField] private ConsoleLog m_consoleLogPrefab = default;

        [SerializeField] private Toggle m_errorToggle = default;
        [SerializeField] private Toggle m_warningToggle = default;
        [SerializeField] private Toggle m_messagesToggle = default;

        private List<ConsoleLog> m_logs = new List<ConsoleLog>();

        private List<ConsoleLog> m_messageLogsList = new List<ConsoleLog>();
        private List<ConsoleLog> m_warningLogsList = new List<ConsoleLog>();
        private List<ConsoleLog> m_errorLogsList = new List<ConsoleLog>();

        public void ToggleErrorLogs(bool value)
        {
            foreach (ConsoleLog log in m_errorLogsList)
            {
                log.gameObject.SetActive(value);
            }
        }

        public void ToggleWarningsLogs(bool value)
        {
            foreach (ConsoleLog log in m_warningLogsList)
            {
                log.gameObject.SetActive(value);
            }
        }

        public void ToggleMessagesLogs(bool value)
        {
            foreach (ConsoleLog log in m_messageLogsList)
            {
                log.gameObject.SetActive(value);
            }
        }

        public void CopyStackTraceText()
        {
            GUIUtility.systemCopyBuffer = m_stackTraceText.text;
        }

        public void ClearLogs()
        {
            m_scrollViewContent.DestroyChildren();

            m_errorLogsList = new List<ConsoleLog>();
            m_warningLogsList = new List<ConsoleLog>();
            m_messageLogsList = new List<ConsoleLog>();
        }

        public void Initialise()
        {
            Application.logMessageReceived -= HandleLog;
            Application.logMessageReceived += HandleLog;
        }

        private void Awake()
        {
            m_messagesToggle.isOn = true;
            m_warningToggle.isOn = true;
            m_errorToggle.isOn = true;
        }

        private void OnEnable()
        {
            m_stackTraceText.text = "";
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            ConsoleLog newLog = Instantiate(m_consoleLogPrefab, m_scrollViewContent);
            newLog.SetText(condition, stackTrace);
            newLog.OnClicked += SetStackTraceText;

            switch (type)
            {
                case LogType.Log:
                {
                    newLog.SetLogTypeSprite(m_messageLogSprite);
                    m_messageLogsList.Add(newLog);
                    break;
                }
                case LogType.Warning:
                {
                    newLog.SetLogTypeSprite(m_warningLogSprite);
                    m_warningLogsList.Add(newLog);
                    break;
                }
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                {
                    newLog.SetLogTypeSprite(m_errorLogSprite);
                    m_errorLogsList.Add(newLog);
                    break;
                }
            }

            m_logs.Add(newLog);
        }

        private void SetStackTraceText(string summary, string stackTrace)
        {
            m_stackTraceText.text = $"{summary}{Environment.NewLine}{stackTrace}";
        }
    }
}