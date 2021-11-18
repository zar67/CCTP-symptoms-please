using SymptomsPlease.Common;
using UnityEngine;

namespace SymptomsPlease.UI.Popups
{
    public class PopupTriggerComponent : Triggerable
    {
        [SerializeField] private PopupData m_popupdata = default;
        [SerializeField] private string m_popupType = default;

        public override void Trigger()
        {
            m_popupdata.OpenPopup(m_popupType);
        }
    }
}