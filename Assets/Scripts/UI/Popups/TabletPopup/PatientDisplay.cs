using TMPro;
using UnityEngine;

public class PatientDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nameText = default;
    [SerializeField] private TextMeshProUGUI m_appointmentInfoText = default;

    private PatientData m_patientData = default;

    public void SetData(PatientData data)
    {
        m_patientData = data;
        m_nameText.text = data.PatientName;
        m_appointmentInfoText.text = data.MainAppointmentInfo;
    }
}