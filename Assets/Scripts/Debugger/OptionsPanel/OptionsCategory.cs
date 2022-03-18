using TMPro;
using UnityEngine;

namespace SymptomsPlease.Debugger
{
    public class OptionsCategory : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_categoryTitleText = default;
        [SerializeField] private Transform m_optionsParentTransform = default;

        public Transform CategoryParent => m_optionsParentTransform;

        public void SetTitleText(string text)
        {
            m_categoryTitleText.text = text;
        }
    }
}