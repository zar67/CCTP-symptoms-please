using SymptomsPlease.Debugging.Logging;
using SymptomsPlease.Managers;
using SymptomsPlease.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Popups
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "SymptomsPlease/UI/Popups/PopupData")]
    public class PopupData : GameScriptableObject
    {
        public Dictionary<string, Popup> InstantiatedPopups = new Dictionary<string, Popup>();

        private List<string> m_openedPopups = new List<string>();

        public void ClearOpenedPopupsList()
        {
            m_openedPopups = new List<string>();
        }

        public Popup GetPopup(string type)
        {
            if (InstantiatedPopups.ContainsKey(type))
            {
                return InstantiatedPopups[type];
            }

            return null;
        }

        public Popup OpenPopup(string type)
        {
            if (IsPopupOpen(type))
            {
                CustomLogger.Warning(LoggingChannels.Popups, $"Popup {type} is already open");
            }

            if (InstantiatedPopups.ContainsKey(type))
            {
                InstantiatedPopups[type].gameObject.SetActive(true);
                InstantiatedPopups[type].PlayOpenAnimation();
                m_openedPopups.Add(type);

                if (InstantiatedPopups[type].PauseGameWhenOpen)
                {
                    GamePausedHandler.PauseGame(true);
                }

                return InstantiatedPopups[type];
            }

            return null;
        }

        public void ClosePopup(string type)
        {
            if (!IsPopupOpen(type))
            {
                CustomLogger.Warning(LoggingChannels.Popups, $"Popup {type} is not open");
                return;
            }

            if (InstantiatedPopups.ContainsKey(type))
            {
                InstantiatedPopups[type].PlayCloseAnimation();
                m_openedPopups.Remove(type);

                if (InstantiatedPopups[type].PauseGameWhenOpen)
                {
                    if (m_openedPopups.Count == 0)
                    {
                        GamePausedHandler.PauseGame(false);
                    }
                    else
                    {
                        GamePausedHandler.PauseGame(InstantiatedPopups[m_openedPopups[m_openedPopups.Count - 1]].PauseGameWhenOpen);
                    }
                }
            }
        }

        public void CloseAllPopupsOfType<T>()
        {
            var openedPopupsOfType = new List<string>();
            foreach (string popup in m_openedPopups)
            {
                if (InstantiatedPopups[popup] is T)
                {
                    openedPopupsOfType.Add(popup);
                }
            }

            foreach (string popup in openedPopupsOfType)
            {
                ClosePopup(popup);
            }
        }

        public bool IsPopupOpen(string type)
        {
            return m_openedPopups.Contains(type);
        }
    }
}