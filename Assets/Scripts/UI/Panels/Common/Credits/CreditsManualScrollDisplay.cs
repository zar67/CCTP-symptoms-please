using TMPro;
using UnityEngine;

namespace SymptomsPlease.UI.Panels.Common.Credits
{
    public class CreditsManualScrollDisplay : MonoBehaviour
    {
        [SerializeField] private CreditsData m_creditsData = default;
        [SerializeField] private TextMeshProUGUI m_scrollingTextObject = default;

        private void Awake()
        {
            m_scrollingTextObject.text = m_creditsData.GetString();
        }
    }
}