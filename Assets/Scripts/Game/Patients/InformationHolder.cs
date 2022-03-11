using TMPro;
using UnityEngine;

public class InformationHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_descriptionText = default;

    public void SetText(string text)
    {
        m_descriptionText.text = text;
    }
}