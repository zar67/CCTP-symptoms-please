using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceAnswer : MonoBehaviour
{
    public static event Action<MultipleChoiceAnswer> OnSelected;

    [SerializeField] private Color m_selectedColour = Color.white;
    [SerializeField] private Color m_deselectedColour = Color.white;

    [SerializeField] private Button m_button = default;
    [SerializeField] private Image m_buttonImage = default;
    [SerializeField] private TextMeshProUGUI m_answerText = default;
    [SerializeField] private Image m_resultIconImage = default;

    [SerializeField] private Sprite m_correctSprite = default;
    [SerializeField] private Sprite m_incorrectSprite = default;
    [SerializeField] private Sprite m_correctBackgroundSprite = default;
    [SerializeField] private Sprite m_incorrectBackgroundSprite = default;

    public string AnswerText => m_answerText.text;

    public void Initialise(string answerText, bool selected = false)
    {
        m_answerText.text = answerText;
        m_resultIconImage.gameObject.SetActive(false);

        if (selected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }

    public void AnswerSubmitted(bool selected, bool correct)
    {
        m_resultIconImage.gameObject.SetActive(true);

        m_resultIconImage.sprite = correct ? m_correctSprite : m_incorrectSprite;

        if (selected)
        {
            m_buttonImage.color = m_deselectedColour;
            m_buttonImage.sprite = correct ? m_correctBackgroundSprite : m_incorrectBackgroundSprite;
        }
    }

    public void Select()
    {
        m_buttonImage.color = m_selectedColour;
    }

    public void Deselect()
    {
        m_buttonImage.color = m_deselectedColour;
    }

    private void OnEnable()
    {
        m_button.onClick.AddListener(OnAnswerClicked);
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(OnAnswerClicked);
    }

    private void OnAnswerClicked()
    {
        AudioManager.Instance.Play(EAudioClipType.CLICK);
        OnSelected?.Invoke(this);
    }
}