using TMPro;
using UnityEngine;

public class AvatarAttribute : MonoBehaviour
{
    public string CurrentValue => m_attributeValues[m_currentIndex];

    [SerializeField] private TextMeshProUGUI m_currentValueText = default;
    [SerializeField] private bool m_loopValues = false;
    [SerializeField] private string[] m_attributeValues = { };

    private int m_currentIndex = 0;

    public void OnNegativeButton()
    {
        m_currentIndex--;

        if (m_currentIndex < 0)
        {
            if (m_loopValues)
            {
                m_currentIndex = m_attributeValues.Length - 1;
            }
            else
            {
                m_currentIndex = 0;
            }
        }

        UpdateDisplay();
    }

    public void OnPositiveButton()
    {
        m_currentIndex++;

        if (m_currentIndex > m_attributeValues.Length - 1)
        {
            if (m_loopValues)
            {
                m_currentIndex = 0;
            }
            else
            {
                m_currentIndex = m_attributeValues.Length - 1;
            }
        }

        m_currentIndex %= m_attributeValues.Length;

        UpdateDisplay();
    }

    private void Awake()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        m_currentValueText.text = CurrentValue;
    }
}