using UnityEngine;

namespace SymptomsPlease.Common.Conditions
{
    public class GroundedCondition : BaseCondition
    {
        [SerializeField] private CharacterController m_controller = default;
        [SerializeField] private bool m_grounded = default;

        public override bool IsSatisfied()
        {
            return m_grounded == m_controller.isGrounded;
        }
    }
}
