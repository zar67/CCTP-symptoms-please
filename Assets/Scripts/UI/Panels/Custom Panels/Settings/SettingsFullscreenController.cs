using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Settings
{
    public class SettingsFullscreenController : MonoBehaviour
    {
        private FullScreenMode m_currentMode = FullScreenMode.Windowed;

        public void SetFullscreenMode(FullScreenMode mode)
        {
            Screen.fullScreenMode = mode;
        }

        public void SetToCurrentMode()
        {
            Screen.fullScreenMode = m_currentMode;
        }

        public void NextMode()
        {
            m_currentMode++;
            if (m_currentMode > FullScreenMode.Windowed)
            {
                m_currentMode = FullScreenMode.ExclusiveFullScreen;
            }
        }

        public void PreviousMode()
        {
            m_currentMode--;
            if (m_currentMode <= FullScreenMode.ExclusiveFullScreen)
            {
                m_currentMode = FullScreenMode.Windowed;
            }
        }
    }
}