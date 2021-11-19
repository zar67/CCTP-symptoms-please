using TMPro;
using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    public class CreditsStaticDisplay : MonoBehaviour
    {
        [SerializeField] private CreditsData m_creditsData = default;

        [SerializeField] private TextMeshProUGUI m_staticTextObject = default;

        private void Awake()
        {
            string creditsText = m_creditsData.GetString();
            m_staticTextObject.text = creditsText;
        }
    }
}