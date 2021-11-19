using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SymptomBubble : MonoBehaviour
{
    public CanvasGroup CanvasGroup => m_canvasGroup;

    [SerializeField] private CanvasGroup m_canvasGroup = default;
    [SerializeField] private TextMeshProUGUI m_writtenSymptomText = default;
    [SerializeField] private Image m_iconSymptomImage = default;

    public void SetWrittenSymptomText(string text)
    {
        m_writtenSymptomText.text = text;
        m_writtenSymptomText.enabled = true;
        m_iconSymptomImage.enabled = false;
    }

    public void SetIconSymptomSprite(Sprite sprite)
    {
        m_iconSymptomImage.sprite = sprite;
        m_iconSymptomImage.enabled = true;
        m_writtenSymptomText.enabled = false;
    }
}
