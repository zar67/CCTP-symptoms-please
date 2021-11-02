using SymptomsPlease.Common;
using SymptomsPlease.UI.Panels;
using UnityEngine;

namespace SymptomsPlease.Transitions.Panels
{
    public class PanelTransitionComponent : Triggerable
    {
        [SerializeField] private PanelsData m_panelManager = default;
        [SerializeField] private PanelTransitionData m_transition = default;

        public override void Trigger()
        {
            m_transition.State = TransitionData.TransitionState.OUT;
            m_panelManager.TransitionToPanel(m_transition);
        }
    }
}