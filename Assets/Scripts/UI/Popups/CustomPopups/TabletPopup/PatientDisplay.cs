using SymptomsPlease.UI.Popups;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatientDisplay : MonoBehaviour
{
    public static event Action<PatientData> OnPatientSelected;

    [Header("Colour References")]
    [SerializeField] private Color m_defaultColour = Color.white;
    [SerializeField] private Color m_oneStrikeColour = Color.yellow;
    [SerializeField] private Color m_twoStrikesColour = Color.red;

    [SerializeField] private float m_defaultAlpha = 0.75f;
    [SerializeField] private float m_highlightedAlpha = 1.0f;

    [Header("Display References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_appointmentInfoText = default;
    [SerializeField] private Image m_backgroundImage = default;

    [Header("Patient Selection")]
    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private PopupTriggerComponent m_popupTrigger = default;

    public PatientData PatientData
    {
        get;
        private set;
    }

    public void SetData(PatientData data)
    {
        PatientData = data;
        m_nameText.text = data.Name;
        m_appointmentInfoText.text = data.AppointmentSummary;

        if (data.PlayerStrikes >= 2)
        {
            m_backgroundImage.color = m_twoStrikesColour;
        }
        else if (data.PlayerStrikes >= 1)
        {
            m_backgroundImage.color = m_oneStrikeColour;
        }
        else
        {
            m_backgroundImage.color = m_defaultColour;
        }
    }

    public void SetCurrent(bool isCurrent)
    {
        Color backgroundColour = m_backgroundImage.color;
        backgroundColour.a = isCurrent ? m_highlightedAlpha : m_defaultAlpha;
        m_backgroundImage.color = backgroundColour;

        Color textColour = m_nameText.color;
        textColour.a = isCurrent ? m_highlightedAlpha : m_defaultAlpha;
        m_nameText.color = textColour;
    }

    private void OnEnable()
    {
        m_selectButton.onClick.AddListener(OnSelected);
        m_selectButton.onClick.AddListener(m_popupTrigger.Trigger);
    }

    private void OnDisable()
    {
        m_selectButton.onClick.RemoveListener(OnSelected);
        m_selectButton.onClick.RemoveListener(m_popupTrigger.Trigger);
    }

    private void OnSelected()
    {
        OnPatientSelected?.Invoke(PatientData);
    }
}