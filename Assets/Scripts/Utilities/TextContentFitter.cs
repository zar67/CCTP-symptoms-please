using System;
using TMPro;
using UnityEngine;

namespace SymptomsPlease.Utilities
{
    [ExecuteInEditMode]
    public class TextContentFitter : MonoBehaviour
    {
        [Flags]
        public enum Mode
        {
            None = 0,
            Horizontal = 0x1,
            Vertical = 0x2,
            Both = Horizontal | Vertical
        }

        [SerializeField] private TMP_Text m_textMeshProComponent;
        [SerializeField] private bool m_resizeTextObject = true;
        [SerializeField] private Vector2 m_padding;
        [SerializeField] private Vector2 m_maxSize = new Vector2(1000, float.PositiveInfinity);
        [SerializeField] private Vector2 m_minSize;
        [SerializeField] private Mode m_controlAxes = Mode.Both;

        private DrivenRectTransformTracker m_textRectTransformTracker;

        private RectTransform m_textRectTransform;
        private RectTransform m_selfRectTransform;

        private bool m_forceRefresh;
        private bool m_isTextNull = true;

        private string m_lastText;
        private Mode m_lastControlAxes = Mode.None;
        private Vector2 m_lastSize;

        protected virtual float MinX
        {
            get
            {
                if ((m_controlAxes & Mode.Horizontal) != 0)
                {
                    return m_minSize.x;
                }

                return m_selfRectTransform.rect.width - m_padding.x;
            }
        }
        protected virtual float MinY
        {
            get
            {
                if ((m_controlAxes & Mode.Vertical) != 0)
                {
                    return m_minSize.y;
                }

                return m_selfRectTransform.rect.height - m_padding.y;
            }
        }
        protected virtual float MaxX
        {
            get
            {
                if ((m_controlAxes & Mode.Horizontal) != 0)
                {
                    return m_maxSize.x;
                }

                return m_selfRectTransform.rect.width - m_padding.x;
            }
        }

        protected virtual float MaxY
        {
            get
            {
                if ((m_controlAxes & Mode.Vertical) != 0)
                {
                    return m_maxSize.y;
                }

                return m_selfRectTransform.rect.height - m_padding.y;
            }
        }

        protected virtual void Update()
        {
            if (m_lastControlAxes != m_controlAxes)
            {
                UpdateRectTranformValues();
            }

            if (!m_isTextNull && (m_textMeshProComponent.text != m_lastText || m_lastSize != m_selfRectTransform.rect.size || m_forceRefresh || m_controlAxes != m_lastControlAxes))
            {
                Vector2 preferredSize = m_textMeshProComponent.GetPreferredValues(MaxX, MaxY);
                preferredSize.x = Mathf.Clamp(preferredSize.x, MinX, MaxX);
                preferredSize.y = Mathf.Clamp(preferredSize.y, MinY, MaxY);
                preferredSize += m_padding;

                if ((m_controlAxes & Mode.Horizontal) != 0)
                {
                    m_selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
                    if (m_resizeTextObject)
                    {
                        m_textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize.x);
                    }
                }
                if ((m_controlAxes & Mode.Vertical) != 0)
                {
                    m_selfRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
                    if (m_resizeTextObject)
                    {
                        m_textRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
                    }
                }

                m_lastText = m_textMeshProComponent.text;
                m_lastSize = m_selfRectTransform.rect.size;
                m_lastControlAxes = m_controlAxes;
                m_forceRefresh = false;
            }
        }

        public virtual void Refresh()
        {
            m_forceRefresh = true;
            m_isTextNull = m_textMeshProComponent == null;

            if (m_textMeshProComponent)
            {
                m_textRectTransform = m_textMeshProComponent.GetComponent<RectTransform>();
            }

            m_selfRectTransform = GetComponent<RectTransform>();

            UpdateRectTranformValues();
        }
        private void OnValidate()
        {
            Refresh();
        }

        private void Start()
        {
            m_textRectTransformTracker = new DrivenRectTransformTracker();
            m_textRectTransformTracker.Clear();

            Refresh();
        }

        private void UpdateRectTranformValues()
        {
            DrivenTransformProperties propertiesToLock = DrivenTransformProperties.None;
            if (m_controlAxes.HasFlag(Mode.Horizontal))
            {
                propertiesToLock |= DrivenTransformProperties.SizeDeltaX;
            }
            if (m_controlAxes.HasFlag(Mode.Vertical))
            {
                propertiesToLock |= DrivenTransformProperties.SizeDeltaY;
            }

            m_textRectTransformTracker.Clear();
            m_textRectTransformTracker.Add(this, m_textRectTransform, propertiesToLock);
        }
    }
}