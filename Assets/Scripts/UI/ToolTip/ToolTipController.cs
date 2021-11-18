using SymptomsPlease.Events;
using SymptomsPlease.Utilities.ExtensionMethods;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace SymptomsPlease.UI.ToolTip
{
    public struct ToolTipData
    {
        public string Header;
        public string ContentText;
        public GameObject ContentPrefab;

        public ToolTipData(GameObject content, string header = "")
        {
            Header = header;
            ContentText = "";
            ContentPrefab = content;
        }

        public ToolTipData(string content, string header = "")
        {
            Header = header;
            ContentText = content;
            ContentPrefab = null;
        }
    }

    public class ToolTipController : MonoBehaviour
    {
        [Header("Sizing Values")]
        [SerializeField] private int m_characterWrapLimit = 80;

        [Header("Raycast Values")]
        [SerializeField] private bool m_raycast2D = false;
        [SerializeField] private float m_raycastRadius = 1f;

        [Header("Display References")]
        [SerializeField] private GameObject m_toolTipObject = default;
        [SerializeField] private GameObject m_contentHolder = default;
        [SerializeField] private TextMeshProUGUI m_headerText = default;
        [SerializeField] private TextMeshProUGUI m_contentText = default;
        [SerializeField] private LayoutElement m_layoutElement = default;

        private void Awake()
        {
            m_toolTipObject.SetActive(false);
        }

        private void Update()
        {
            if (m_raycast2D)
            {
                UpdateRaycast2D();
            }
            else
            {
                UpdateRaycast();
            }
        }

        private void ShowToolTip(ToolTipData data)
        {
            m_headerText.gameObject.SetActive(!string.IsNullOrEmpty(data.Header));
            m_headerText.text = data.Header;

            m_contentText.gameObject.SetActive(data.ContentPrefab == null);
            m_contentHolder.SetActive(data.ContentPrefab != null);
            if (data.ContentPrefab == null)
            {
                m_contentText.text = data.ContentText;
            }
            else
            {
                m_contentHolder.DestroyChildren();
                Instantiate(data.ContentPrefab, m_contentHolder.transform);
            }

            m_toolTipObject.SetActive(true);

            int headerLength = m_headerText.text.Length;
            int contentLength = m_contentText.text.Length;

            m_layoutElement.enabled = headerLength > m_characterWrapLimit || contentLength > m_characterWrapLimit;
        }

        private void HideToolTip()
        {
            m_toolTipObject.SetActive(false);
        }

        private void UpdateRaycast2D()
        {
            Vector3 position = Mouse.current.position.ReadValue();
            position.z = -Camera.main.transform.position.z;
            position = Camera.main.ScreenToWorldPoint(position);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(position, m_raycastRadius, Vector2.zero);

            if (hits.Length > 0 && hits[0].transform.GetComponent<IToolTipObject>() != null)
            {
                IToolTipObject raycastable = hits[0].transform.GetComponent<IToolTipObject>();
                if (raycastable.HandleRaycast())
                {
                    ShowToolTip(raycastable.GetData());
                }
            }
            else
            {
                HideToolTip();
            }
        }

        private void UpdateRaycast()
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.SphereCastAll(mouseRay, m_raycastRadius);

            if (hits.Length > 0 && hits[0].transform.GetComponent<IToolTipObject>() != null)
            {
                IToolTipObject raycastable = hits[0].transform.GetComponent<IToolTipObject>();
                if (raycastable.HandleRaycast())
                {
                    ShowToolTip(raycastable.GetData());
                }
            }
            else
            {
                HideToolTip();
            }
        }
    }
}