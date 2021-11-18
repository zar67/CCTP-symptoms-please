using UnityEngine;
using UnityEngine.InputSystem;

namespace SymptomsPlease.UI.ToolTip
{
    public class ToolTipPositioner : MonoBehaviour
    {
        [SerializeField] private RectTransform m_rectTransform = default;
        [SerializeField] private float m_mouseSpacing = 0.2f;

        private void Update()
        {
            Vector2 position = Mouse.current.position.ReadValue();

            float pivotX = position.x / Screen.width;
            pivotX += pivotX > 0.5f ? m_mouseSpacing : -m_mouseSpacing;

            float pivotY = position.y / Screen.height;
            pivotY += pivotY > 0.5f ? m_mouseSpacing : -m_mouseSpacing;

            m_rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}