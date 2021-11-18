using UnityEngine;

namespace SymptomsPlease.Common.Conditions
{
    public class RangeCondition : BaseCondition
    {
        [SerializeField] private Transform m_initialTransform = default;
        [SerializeField] private Transform m_targetTransform = default;
        [SerializeField] private float m_range = default;
        [SerializeField] private bool m_withinRange = default;

        public override bool IsSatisfied()
        {
            float distance = Vector3.Distance(transform.position, m_targetTransform.position);

            if (m_initialTransform != null)
            {
                distance = Vector3.Distance(m_initialTransform.position, m_targetTransform.position);
            }

            return m_withinRange ? distance <= m_range : distance > m_range;
        }
    }
}
