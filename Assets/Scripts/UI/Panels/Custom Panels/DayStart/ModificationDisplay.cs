using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModificationDisplay : MonoBehaviour
{
    public Transform InfoButtonHolder => m_infoButtonHolder;

    [SerializeField] private TextMeshProUGUI m_descriptionText = default;
    [SerializeField] private Transform m_infoButtonHolder = default;
    [SerializeField] private RectTransform m_mainRectTransform = default;
    [SerializeField] private RectTransform m_infoRectTransform = default;

    public void SetText(string text)
    {
        m_descriptionText.text = text;
    }

    public void UpdateLayout()
    {
        m_mainRectTransform.sizeDelta = new Vector2(m_infoRectTransform.sizeDelta.x, 275 + (m_infoButtonHolder.childCount * 100));
        m_infoRectTransform.sizeDelta = new Vector2(m_infoRectTransform.sizeDelta.x, m_infoButtonHolder.childCount * 100);
    }
}