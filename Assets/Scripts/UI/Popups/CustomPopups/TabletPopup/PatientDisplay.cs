using SymptomsPlease.UI.Popups;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatientDisplay : MonoBehaviour
{
    public static event Action<PatientData> OnPatientSelected;

    [Header("Display References")]
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_appointmentInfoText = default;

    [Header("Patient Selection")]
    [SerializeField] private Button m_selectButton = default;
    [SerializeField] private PopupTriggerComponent m_popupTrigger = default;

    private PatientData m_patientData;

    public void SetData(PatientData data)
    {
        m_patientData = data;
        m_nameText.text = data.Name;
        m_appointmentInfoText.text = data.AppointmentSummary;
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
        OnPatientSelected?.Invoke(m_patientData);
    }
}