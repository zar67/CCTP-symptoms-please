using TMPro;
using UnityEngine;

public class SymptomBubble : MonoBehaviour
{
    public CanvasGroup CanvasGroup => m_canvasGroup;

    [SerializeField] private CanvasGroup m_canvasGroup = default;
    [SerializeField] private TextMeshProUGUI m_writtenSymptomText = default;

    public void SetText(string text)
    {
        m_writtenSymptomText.text = text;
    }
}
