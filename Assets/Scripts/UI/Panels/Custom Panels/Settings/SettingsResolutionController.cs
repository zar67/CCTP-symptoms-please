using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Settings
{
    public class SettingsResolutionController : MonoBehaviour
    {
        private Resolution[] m_resolutions = { };
        private int m_currentResolutionIndex = 0;

        private void Awake()
        {
            m_resolutions = Screen.resolutions;
        }

        public Resolution GetCurrentResolution()
        {
            return m_resolutions[m_currentResolutionIndex];
        }

        public void SetToCurrentResolution()
        {
            Resolution resolution = m_resolutions[m_currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetResultion(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void NextResolution()
        {
            m_currentResolutionIndex++;
            if (m_currentResolutionIndex >= m_resolutions.Length)
            {
                m_currentResolutionIndex = 0;
            }
        }

        public void PreviousResolution()
        {
            m_currentResolutionIndex--;
            if (m_currentResolutionIndex < 0)
            {
                m_currentResolutionIndex = m_resolutions.Length - 1;
            }
        }
    }
}