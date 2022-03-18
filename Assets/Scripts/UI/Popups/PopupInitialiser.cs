using System.Collections.Generic;
using UnityEngine;

namespace SymptomsPlease.UI.Popups
{
    public class PopupInitialiser : MonoBehaviour
    {
        [SerializeField] private PopupData m_popupData = default;
        [SerializeField] private GameObject m_parentManagerObject = default;

        private void Awake()
        {
            m_popupData.InstantiatedPopups = new Dictionary<string, Popup>();
            m_popupData.ClearOpenedPopupsList();

            Popup[] popups = m_parentManagerObject.GetComponentsInChildren<Popup>(true);
            foreach (Popup popup in popups)
            {
                m_popupData.InstantiatedPopups.Add(popup.Type, popup);
                popup.gameObject.SetActive(false);
            }
        }
    }
}
