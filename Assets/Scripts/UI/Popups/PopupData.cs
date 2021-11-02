using SymptomsPlease.Debugging.Logging;
using SymptomsPlease.ScriptableObjects;
using SymptomsPlease.ScriptableObjects.Variables;
using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Popups
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "SymptomsPlease/UI/Popups/PopupData")]
    public class PopupData : GameScriptableObject
    {
        [SerializeField] private BoolVariable m_gamePaused = default;

        public Dictionary<string, Popup> InstantiatedPopups = new Dictionary<string, Popup>();

        private HashSet<string> m_openedPopups = new HashSet<string>();

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
                    m_gamePaused.Value = true;
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
                
                m_gamePaused.Value = false;
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
