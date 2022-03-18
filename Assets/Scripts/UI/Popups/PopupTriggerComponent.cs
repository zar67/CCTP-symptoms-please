using SymptomsPlease.Common;
using UnityEngine;

namespace SymptomsPlease.UI.Popups
{
    public class PopupTriggerComponent : Triggerable
    {
        [SerializeField] private PopupData m_popupdata = default;
        [SerializeField] private string m_popupType = default;
        [SerializeField] private bool m_open = true;

        public override void Trigger()
        {
            if (m_open)
            {
                m_popupdata.OpenPopup(m_popupType);
            }
            else
            {
                m_popupdata.ClosePopup(m_popupType);
            }
        }
    }
}