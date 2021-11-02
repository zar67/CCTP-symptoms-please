using SymptomsPlease.Common;
using UnityEngine;

namespace SymptomsPlease.UI.Panels
{
    public class SetupPanelComponent : Triggerable
    {
        [SerializeField] private PanelsData m_panelManagerData = default;
        [SerializeField] private string m_panelToTransitionTo;

        public override void Trigger()
        {
            m_panelManagerData.SetupInitialPanel(m_panelToTransitionTo);
        }
    }
}