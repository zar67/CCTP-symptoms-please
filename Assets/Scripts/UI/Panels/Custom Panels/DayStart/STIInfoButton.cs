using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class STIInfoButton : MonoBehaviour
{
    public Button SelectButton => m_selectButton;

    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private TextMeshProUGUI m_naneText = default;

    private void OnEnable()
    {
        m_selectButton.onClick.AddListener(() => AudioManager.Instance.Play(EAudioClipType.CLICK));
    }

    private void OnDisable()
    {
        m_selectButton.onClick.RemoveListener(() => AudioManager.Instance.Play(EAudioClipType.CLICK));
    }

    public void SetNameText(string text)
    {
        m_naneText.text = text;
    }
}
