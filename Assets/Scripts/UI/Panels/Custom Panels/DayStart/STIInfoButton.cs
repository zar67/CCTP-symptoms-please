using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class STIInfoButton : MonoBehaviour
{
    public Button SelectButton => m_selectButton;

    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private TextMeshProUGUI m_naneText = default;

    public void SetNameText(string text)
    {
        m_naneText.text = text;
    }
}
