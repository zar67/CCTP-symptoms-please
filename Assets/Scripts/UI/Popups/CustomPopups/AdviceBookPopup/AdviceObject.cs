using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdviceObject : MonoBehaviour
{
    public Button SelectButton => m_selectButton;

    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private TextMeshProUGUI m_adviceText = default;

    public void UpdateText(string text)
    {
        m_adviceText.text = text;
    }
}