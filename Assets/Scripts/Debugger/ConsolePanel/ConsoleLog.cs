using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SymptomsPlease.Debugger
{
    public class ConsoleLog : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image m_logTypeImage = default;
        [SerializeField] private TextMeshProUGUI m_logSummaryText = default;

        public event Action<string, string> OnClicked = null;

        private string m_summaryText;
        private string m_stackTraceText;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke(m_summaryText, m_stackTraceText);
        }

        public void SetLogTypeSprite(Sprite sprite)
        {
            m_logTypeImage.sprite = sprite;
        }

        public void SetText(string summary, string stackTrace)
        {
            m_logSummaryText.text = summary;

            m_summaryText = summary;
            m_stackTraceText = stackTrace;
        }
    }
}