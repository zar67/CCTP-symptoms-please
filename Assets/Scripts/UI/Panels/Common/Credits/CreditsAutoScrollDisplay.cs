using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    public class CreditsAutoScrollDisplay : MonoBehaviour
    {
        [SerializeField] private CreditsData m_creditsData = default;

        [SerializeField] private ScrollRect m_scrollingTextScrollView = default;
        [SerializeField] private TextMeshProUGUI m_scrollingTextObject = default;
        [SerializeField] private float m_scrollLinesPerSecond = default;

        private int m_textLines = 0;

        private void Awake()
        {
            string creditsText = m_creditsData.GetString();
            m_textLines = m_creditsData.GetNumberOfLines();

            m_scrollingTextObject.text = creditsText;

            m_scrollingTextScrollView.enabled = false;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(AutoScrollCredits());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator AutoScrollCredits()
        {
            float startTime = Time.time;
            float currentTime = Time.time;

            yield return null;

            m_scrollingTextScrollView.normalizedPosition = new Vector2(0, 1);
            while (m_scrollingTextScrollView.verticalNormalizedPosition != 0)
            {
                if (m_scrollingTextScrollView.verticalNormalizedPosition < 0.05f)
                {
                    m_scrollingTextScrollView.verticalNormalizedPosition = 0;
                }

                currentTime = Time.time;
                float timePassed = currentTime - startTime;
                float linesTraversed = m_scrollLinesPerSecond * timePassed;

                float newValue = Mathf.Lerp(1, 0, linesTraversed / m_textLines);
                m_scrollingTextScrollView.verticalNormalizedPosition = newValue;
                yield return null;
            }
        }
    }
}