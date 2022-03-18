using SymptomsPlease.Common;
using SymptomsPlease.UI.Panels;
using UnityEngine;

namespace SymptomsPlease.Transitions.Panels
{
    public class PanelGoBackComponent : Triggerable
    {
        [SerializeField] private PanelsData m_panelManager = default;
        [SerializeField] private bool m_showLoadingScreen = default;
        [SerializeField] private string m_transitionEffect = default;

        public override void Trigger()
        {
            m_panelManager.GoBack(m_showLoadingScreen, m_transitionEffect);
        }
    }
}