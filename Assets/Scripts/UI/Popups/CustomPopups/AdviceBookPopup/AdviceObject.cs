using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdviceObject : MonoBehaviour
{
    public Button SelectButton => m_selectButton;

    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private TextMeshProUGUI m_adviceText = default;

    private void OnEnable()
    {
        m_selectButton.onClick.AddListener(() => AudioManager.Instance.Play(EAudioClipType.CLICK));
    }

    private void OnDisable()
    {
        m_selectButton.onClick.RemoveListener(() => AudioManager.Instance.Play(EAudioClipType.CLICK));
    }

    public void UpdateText(string text)
    {
        m_adviceText.text = text;
    }
}