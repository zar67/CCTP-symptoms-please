using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymptomBubble : MonoBehaviour
{
    public CanvasGroup CanvasGroup => m_canvasGroup;

    [SerializeField] private CanvasGroup m_canvasGroup = default;
    [SerializeField] private TextMeshProUGUI m_writtenSymptomText = default;

    public void SetSymptomData(string text)
    {
        m_writtenSymptomText.text = text;
    }
}
