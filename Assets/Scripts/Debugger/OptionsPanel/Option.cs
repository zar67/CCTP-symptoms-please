using TMPro;
using UnityEngine;

namespace SymptomsPlease.Debugger
{
    public class Option : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_nameText = default;

        public void SetOptionText(string text)
        {
            m_nameText.text = text;
        }
    }
}